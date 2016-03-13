using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity;

namespace ConferenceWebAPI.Controllers
{
	public class UsersController : ODataController
	{
		private readonly ConferenceContext _db = new ConferenceContext();

		// GET: odata/Users
		[EnableQuery]
		public IQueryable<Account> GetUsers()
		{
			return _db.Accounts;
		}

		// GET: odata/Users(5)
		[EnableQuery]
		public SingleResult<Account> GetUser( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Accounts.Where( applicationUser => applicationUser.Id == key ) );
		}

		// PUT: odata/Users(5)
		public async Task<IHttpActionResult> Put( [FromODataUri] int key, Delta<Account> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var user = await _db.Accounts.SingleOrDefaultAsync( u => u.Id == key );
			if ( user == null )
			{
				return NotFound();
			}

			patch.Put( user );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !ApplicationUserExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( user );
		}

		// POST: odata/Users
		public async Task<IHttpActionResult> Post( Account user )
		{
			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			_db.Accounts.Add( user );
			await _db.SaveChangesAsync();

			return Created( user );
		}

		// PATCH: odata/Users(5)
		[AcceptVerbs( "PATCH", "MERGE" )]
		public async Task<IHttpActionResult> Patch( [FromODataUri] int key, Delta<Account> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var user = await _db.Accounts.SingleOrDefaultAsync( u => u.Id == key );
			if ( user == null )
			{
				return NotFound();
			}

			patch.Patch( user );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !ApplicationUserExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( user );
		}

		// DELETE: odata/Users(5)
		public async Task<IHttpActionResult> Delete( [FromODataUri] int key )
		{
			var user = await _db.Accounts.SingleOrDefaultAsync( u => u.Id == key );
			if ( user == null )
			{
				return NotFound();
			}

			_db.Accounts.Remove( user );
			await _db.SaveChangesAsync();

			return StatusCode( HttpStatusCode.NoContent );
		}

		// GET: odata/Users(5)/Profile
		[EnableQuery]
		public SingleResult<Profile> GetProfile( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Accounts.Where( m => m.Id == key ).Select( m => m.Profile ) );
		}

		//[EnableQuery]
		//[ODataRoute( "odata/Users/Current/Profile" )]
		//public SingleResult<Profile> GetProfile()
		//{
		//	var userId = User.Identity.GetUserId<int>();
		//	return SingleResult.Create( _db.Users.Where( m => m.Id == userId ).Select( m => m.Profile ) );

		//}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_db.Dispose();
			}
			base.Dispose( disposing );
		}

		private bool ApplicationUserExists( int key )
		{
			return _db.Accounts.Count( e => e.Id == key ) > 0;
		}
	}
}
