using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;

namespace ConferenceWebAPI.Controllers
{
	public class UsersController : ODataController
	{
		private readonly ConferenceContext _db = new ConferenceContext();

		// GET: odata/Users
		[EnableQuery]
		public IQueryable<User> GetUsers()
		{
			return _db.Users;
		}

		// GET: odata/Users(5)
		[EnableQuery]
		public SingleResult<User> GetUser( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Users.Where( applicationUser => applicationUser.Id == key ) );
		}

		// PUT: odata/Users(5)
		public async Task<IHttpActionResult> Put( [FromODataUri] int key, Delta<User> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var user = await _db.Users.SingleOrDefaultAsync( u => u.Id == key );
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
		public async Task<IHttpActionResult> Post( User user )
		{
			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			_db.Users.Add( user );
			await _db.SaveChangesAsync();

			return Created( user );
		}

		// PATCH: odata/Users(5)
		[AcceptVerbs( "PATCH", "MERGE" )]
		public async Task<IHttpActionResult> Patch( [FromODataUri] int key, Delta<User> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var user = await _db.Users.SingleOrDefaultAsync( u => u.Id == key );
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
			var user = await _db.Users.SingleOrDefaultAsync( u => u.Id == key );
			if ( user == null )
			{
				return NotFound();
			}

			_db.Users.Remove( user );
			await _db.SaveChangesAsync();

			return StatusCode( HttpStatusCode.NoContent );
		}

		// GET: odata/Users(5)/Profile
		[EnableQuery]
		public SingleResult<Profile> GetProfile( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Users.Where( m => m.Id == key ).Select( m => m.Profile ) );
		}

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
			return _db.Users.Count( e => e.Id == key ) > 0;
		}
	}
}
