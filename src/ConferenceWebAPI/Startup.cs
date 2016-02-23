using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup( typeof( ConferenceWebAPI.Startup ) )]

namespace ConferenceWebAPI
{
	public partial class Startup
	{
		public void Configuration( IAppBuilder app )
		{
			var config = new HttpConfiguration();
			app.UseCors(CorsOptions.AllowAll);
			ConfigureAuth( app );
			WebApiConfig.Register(config);
			app.UseWebApi( config );
		}
	}
}
