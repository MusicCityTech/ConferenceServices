using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;

namespace ConferenceWebAPI.Controllers
{
	public class SessionsController : ODataController
	{
		private readonly ConferenceContext _db = new ConferenceContext();

		// GET: odata/Sessions
		[EnableQuery]
		[Authorize]
		public IQueryable<Session> GetSessions()
		{
			return _db.Sessions;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_db.Dispose();
			}
			base.Dispose( disposing );
		}
	}
}
