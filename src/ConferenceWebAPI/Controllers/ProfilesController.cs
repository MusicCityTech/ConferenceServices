using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity;

namespace ConferenceWebAPI.Controllers
{
	public class ProfilesController : ODataController
	{
		private readonly ConferenceContext _db = new ConferenceContext();

		// GET: odata/Profiles
		[EnableQuery]
		public IQueryable<Profile> GetProfiles()
		{
			return _db.Profiles;
		}

		// GET: odata/Profiles(5)
		[EnableQuery]
		public SingleResult<Profile> GetProfile( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Profiles.Where( profile => profile.Id == key ) );
		}

		// PUT: odata/Profiles(5)
		public async Task<IHttpActionResult> Put( [FromODataUri] int key, Delta<Profile> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			Profile profile = await _db.Profiles.FindAsync( key );
			if ( profile == null )
			{
				return NotFound();
			}

			patch.Put( profile );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !ProfileExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( profile );
		}

		// POST: odata/Profiles
		public async Task<IHttpActionResult> Post( Profile profile )
		{
			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			if ( profile.User == null )
			{
				var users = ( _db.Accounts as DbSet<Account> );
				if ( users != null )
					profile.User = await users.FindAsync( User.Identity.GetUserId<int>() );
			}

			_db.Profiles.Add( profile );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateException )
			{
				if ( ProfileExists( profile.Id ) )
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return Created( profile );
		}

		// PATCH: odata/Profiles(5)
		[AcceptVerbs( "PATCH", "MERGE" )]
		public async Task<IHttpActionResult> Patch( [FromODataUri] int key, Delta<Profile> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			Profile profile = await _db.Profiles.FindAsync( key );
			if ( profile == null )
			{
				return NotFound();
			}

			patch.Patch( profile );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !ProfileExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( profile );
		}

		// DELETE: odata/Profiles(5)
		public async Task<IHttpActionResult> Delete( [FromODataUri] int key )
		{
			Profile profile = await _db.Profiles.FindAsync( key );
			if ( profile == null )
			{
				return NotFound();
			}

			_db.Profiles.Remove( profile );
			await _db.SaveChangesAsync();

			return StatusCode( HttpStatusCode.NoContent );
		}

		// GET: odata/Profiles(5)/User
		[EnableQuery]
		public SingleResult<Account> GetUser( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Profiles.Where( m => m.Id == key ).Select( m => m.User ) );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_db.Dispose();
			}
			base.Dispose( disposing );
		}

		private bool ProfileExists( int key )
		{
			return _db.Profiles.Count( e => e.Id == key ) > 0;
		}
	}
}
