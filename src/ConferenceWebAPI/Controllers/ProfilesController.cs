using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ConferenceWebAPI.DAL;
using ConferenceWebAPI.Models;

namespace ConferenceWebAPI.Controllers
{
	//public class ProfilesController : ODataController
	//{
	//	private readonly ConferenceContext _db = new ConferenceContext();

	//	// GET: odata/Speakers(5)/profile
	//	[ODataRoute( "Speakers({key})/profile/" )]
	//	public SingleResult<Profile> GetProfile( [FromODataUri] int key )
	//	{
	//		return SingleResult.Create( _db.Users.Where( u => u.Id == key ).Select( u => u.Profile ) );
	//	}

	//	protected override void Dispose( bool disposing )
	//	{
	//		if ( disposing )
	//		{
	//			_db.Dispose();
	//		}
	//		base.Dispose( disposing );
	//	}
	//}
}
