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
	public class SessionsController : ODataController
	{
		private readonly ConferenceContext _db = new ConferenceContext();

		// GET: odata/Sessions
		[EnableQuery]
		public IQueryable<Session> GetSessions()
		{
			return _db.Sessions;
		}

		// GET: odata/Sessions(5)
		[EnableQuery]
		public SingleResult<Session> GetSession( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Sessions.Where( session => session.Id == key ) );
		}

		// PUT: odata/Sessions(5)
		public async Task<IHttpActionResult> Put( [FromODataUri] int key, Delta<Session> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			Session session = await _db.Sessions.FindAsync( key );
			if ( session == null )
			{
				return NotFound();
			}

			patch.Put( session );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !SessionExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( session );
		}

		// POST: odata/Sessions
		public async Task<IHttpActionResult> Post( Session session )
		{
			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			if ( session.Speaker == null )
			{
				var users = ( _db.Users as DbSet<User> );
				if ( users != null )
					session.Speaker = await users.FindAsync( User.Identity.GetUserId<int>() );
			}

			_db.Sessions.Add( session );
			await _db.SaveChangesAsync();

			return Created( session );
		}

		// PATCH: odata/Sessions(5)
		[AcceptVerbs( "PATCH", "MERGE" )]
		public async Task<IHttpActionResult> Patch( [FromODataUri] int key, Delta<Session> patch )
		{
			Validate( patch.GetEntity() );

			if ( !ModelState.IsValid )
			{
				return BadRequest( ModelState );
			}

			Session session = await _db.Sessions.FindAsync( key );
			if ( session == null )
			{
				return NotFound();
			}

			patch.Patch( session );

			try
			{
				await _db.SaveChangesAsync();
			}
			catch ( DbUpdateConcurrencyException )
			{
				if ( !SessionExists( key ) )
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Updated( session );
		}

		// DELETE: odata/Sessions(5)
		public async Task<IHttpActionResult> Delete( [FromODataUri] int key )
		{
			Session session = await _db.Sessions.FindAsync( key );
			if ( session == null )
			{
				return NotFound();
			}

			_db.Sessions.Remove( session );
			await _db.SaveChangesAsync();

			return StatusCode( HttpStatusCode.NoContent );
		}

		// GET: odata/Sessions(5)/Speaker
		[EnableQuery]
		public SingleResult<User> GetSpeaker( [FromODataUri] int key )
		{
			return SingleResult.Create( _db.Sessions.Where( m => m.Id == key ).Select( m => m.Speaker ) );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_db.Dispose();
			}
			base.Dispose( disposing );
		}

		private bool SessionExists( int key )
		{
			return _db.Sessions.Count( e => e.Id == key ) > 0;
		}
	}
}
