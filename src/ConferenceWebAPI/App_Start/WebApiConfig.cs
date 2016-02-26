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

			config.MapHttpAttributeRoutes();

			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<Speaker>( "Speakers" );
			builder.EntitySet<Session>( "Sessions" );
			var userEntityType = builder.EntitySet<User>( "Users" ).EntityType;
			userEntityType.Ignore( e => e.Claims );
			userEntityType.Ignore( e => e.Logins );
			userEntityType.Ignore( u => u.Roles );
			userEntityType.Ignore( u => u.PasswordHash );
			userEntityType.Ignore( u => u.SecurityStamp );

			//builder.EntitySet<Claim>( "UserClaims" );
			//builder.EntitySet<Login>( "UserLogins" );
			builder.EntitySet<Profile>( "Profiles" );

			config.MapODataServiceRoute(
				"odata",
				"odata",
				builder.GetEdmModel(),
				new ODataNullValueMessageHandler()
				{
					InnerHandler = new HttpControllerDispatcher( config )
				} );

			// Web API routes


			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
