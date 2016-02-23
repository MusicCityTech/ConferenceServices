using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData;
using Microsoft.Owin.Security.OAuth;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using ConferenceWebAPI.Models;

namespace ConferenceWebAPI
{
	public static class WebApiConfig
	{
		public static void Register( HttpConfiguration config )
		{
			// Web API configuration and services
			// Configure Web API to use only bearer token authentication.
			config.SuppressDefaultHostAuthentication();
			config.Filters.Add( new HostAuthenticationFilter( OAuthDefaults.AuthenticationType ) );

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<Speaker>( "Speakers" );
			builder.EntitySet<Session>( "Sessions" );
			builder.EntitySet<Profile>( "Profiles" )
				.EntityType
				.HasKey( e => e.Email );

			config.MapODataServiceRoute(
				"odata",
				"odata",
				builder.GetEdmModel(),
				new ODataNullValueMessageHandler()
				{
					InnerHandler = new HttpControllerDispatcher( config )
				} );
		}
	}
}
