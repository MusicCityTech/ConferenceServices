using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;

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

		// GET: odata/profiles('email@domain.com')
		[ODataRoute( "Profiles({email})" )]
		[EnableQuery]
		public SingleResult<Profile> GetProfile( [FromODataUri] string email )
		{
			return SingleResult.Create( _db.Profiles.Where( p => p.Email == email ) );
		}

		// PUT: odata/Profiles(5)
		public async Task<IHttpActionResult> Put( [FromODataUri] string key, Delta<Profile> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var profile = await _db.Profiles.FindAsync( key );
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

			_db.Profiles.Add( profile );
			await _db.SaveChangesAsync();

			return Created( profile );
		}

		// PATCH: odata/Profiles(5)
		[AcceptVerbs( "PATCH", "MERGE" )]
		public async Task<IHttpActionResult> Patch( [FromODataUri] string key, Delta<Profile> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			var profile = await _db.Profiles.FindAsync( key );
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
		public async Task<IHttpActionResult> Delete( [FromODataUri] string key )
		{
			var profile = await _db.Profiles.FindAsync( key );
			if ( profile == null )
			{
				return NotFound();
			}

			_db.Profiles.Remove( profile );
			await _db.SaveChangesAsync();

			return StatusCode( HttpStatusCode.NoContent );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_db.Dispose();
			}
			base.Dispose( disposing );
		}

		private bool ProfileExists( string key )
		{
			return _db.Profiles.Count( e => e.Email == key ) > 0;
		}
	}
}
