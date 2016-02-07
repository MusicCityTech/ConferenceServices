using System.Linq;
using System.Web.OData;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;

namespace ConferenceWebAPI.Controllers
{
    public class SessionsController : ODataController
    {
        private readonly ConferenceContext _db  = new ConferenceContext();

        // GET: odata/Sessions
        [EnableQuery]
        public IQueryable<Session> GetSessions()
        {
            return _db.Sessions;
        }
    }
}
