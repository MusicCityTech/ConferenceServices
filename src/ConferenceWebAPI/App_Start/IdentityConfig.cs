using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ConferenceWebAPI.Models;
using ConferenceWebAPI.DAL;

namespace ConferenceWebAPI
{
	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class ApplicationUserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>, IUserStore<User, int>
	{
		public ApplicationUserStore( DbContext context ) : base( context )
		{
		}
	}

	public class ApplicationUserManager : UserManager<User, int>
	{
		public ApplicationUserManager( IUserStore<User, int> store )
			: base( store )
		{
		}

		public static ApplicationUserManager Create( IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context )
		{
			var manager = new ApplicationUserManager( new ApplicationUserStore( context.Get<ConferenceContext>() ) );
			manager.UserValidator = new UserValidator<User, int>( manager )
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 8,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false
			};

			var dataProtectionProvider = options.DataProtectionProvider;
			if ( dataProtectionProvider != null )
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
			}
			return manager;
		}
	}
}
