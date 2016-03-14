using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ConferenceWebAPI.Models;
using ConferenceWebAPI.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using ConferenceWebAPI.Exceptions;

namespace ConferenceWebAPI
{
	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class AccountStore :
		IUserPasswordStore<Account, int>,
		IUserClaimStore<Account, int>,
		IUserRoleStore<Account, int>,
		IUserLoginStore<Account, int>,
		IUserEmailStore<Account, int>
	{

		public AccountStore( DbContext context )
		{
			_dbContext = context;
		}

		public async Task AddToRoleAsync( Account account, string roleName )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "Account is required" );
			if ( roleName == null ) throw new ArgumentNullException( nameof( roleName ), "role name is required" );

			var role = await _dbContext.Set<Role>().SingleOrDefaultAsync( r => r.Name == roleName );
			if ( role == null ) throw new ArgumentException( $"#{roleName} role does not exist" );

			if ( !account.Roles.Contains( role ) )
			{
				_dbContext.Set<Account>().Attach( account );
				account.Roles.Add( role );
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task CreateAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "Account is required" );
			account.Id = _dbContext.Set<Account>().Add( account ).Id;
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "Account is required" );
			_dbContext.Set<Account>().Attach( account );
			_dbContext.Set<Account>().Remove( account );
			await _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			Dispose( true );
		}

		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				//_dbContext.Dispose();
			}
		}

		public Task<Account> FindByIdAsync( int accountId )
		{
			return _dbContext.Set<Account>()
				.Include( a => a.Roles )
				.Include( a => a.Claims )
				.Include( a => a.Logins )
				.Include( a => a.Profile )
				.SingleAsync( a => a.Id == accountId );
		}

		public Task<Account> FindByNameAsync( string userName )
		{
			if ( string.IsNullOrWhiteSpace( userName ) ) throw new ArgumentNullException( nameof( userName ), "Username is required" );
			return _dbContext.Set<Account>()
				.Include( a => a.Roles )
				.Include( a => a.Claims )
				.Include( a => a.Logins )
				.Include( a => a.Profile )
				.SingleOrDefaultAsync( a => a.UserName == userName );
		}

		public async Task<IList<string>> GetRolesAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			var roles = account.Roles ?? ( await _dbContext.Set<Account>().Include( a => a.Roles ).SingleAsync( a => a.Id == account.Id ) ).Roles;
			return roles.Select( r => r.Name ).ToList();
		}

		public Task<bool> IsInRoleAsync( Account account, string roleName )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			if ( roleName == null ) throw new ArgumentNullException( nameof( roleName ), "role name is required" );

			var role = account.Roles.SingleOrDefault( a => a.Name == roleName );
			return Task.FromResult( role != null );
		}

		public Task RemoveFromRoleAsync( Account account, string roleName )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			if ( roleName == null ) throw new ArgumentNullException( nameof( roleName ), "role name is required" );

			var role = account.Roles.SingleOrDefault( r => r.Name == roleName );
			if ( role != null )
			{
				account.Roles.Remove( role );
			}

			return Task.FromResult<object>( null );
		}

		public async Task UpdateAsync( Account account )
		{
			_dbContext.Set<Account>().Attach( account );
			await _dbContext.SaveChangesAsync();
		}

		public Task SetEmailAsync( Account account, string email )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			if ( email == null ) throw new ArgumentNullException( nameof( email ), "email is required" );

			if ( account.EmailEditAllowed )
			{
				account.Email = email;
			}

			return Task.FromResult<object>( null );
		}

		public Task<string> GetEmailAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			return Task.FromResult( account.Email );
		}

		public Task<bool> GetEmailConfirmedAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			return Task.FromResult( account.EmailConfirmed );
		}

		public Task SetEmailConfirmedAsync( Account account, bool confirmed )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ), "account is required" );
			account.EmailConfirmed = confirmed;
			return Task.FromResult<object>( null );
		}

		public async Task<Account> FindByEmailAsync( string email )
		{
			if ( email == null ) throw new ArgumentNullException( nameof( email ), "email is required" );
			return await _dbContext.Set<Account>().SingleOrDefaultAsync( a => a.Email == email );
		}

		public Task AddLoginAsync( Account account, UserLoginInfo login )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			if ( login == null ) throw new ArgumentNullException( nameof( login ) );

			if ( !account.Logins.Any( l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey ) )
			{
				account.Logins.Add( new AccountLogin( login ) );
			}

			return Task.FromResult<object>( null );
		}

		public Task RemoveLoginAsync( Account account, UserLoginInfo login )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			if ( login == null ) throw new ArgumentNullException( nameof( login ) );

			var userLogin = account.Logins.SingleOrDefault( l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey );
			if ( userLogin != null )
			{
				account.Logins.Remove( userLogin );
			}

			return Task.FromResult<object>( null );
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			var logins = account.Claims.Select( claim => new UserLoginInfo( claim.Type, claim.Value ) ).ToList();
			return Task.FromResult<IList<UserLoginInfo>>( logins );
		}

		public async Task<Account> FindAsync( UserLoginInfo login )
		{
			return await _dbContext.Set<Account>().SingleOrDefaultAsync( a => a.Logins.Any( l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey ) );
		}

		public Task<IList<Claim>> GetClaimsAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			return Task.FromResult<IList<Claim>>( account.Claims.Select( c => new Claim( c.Type, c.Value ) ).ToList() );
		}

		public Task AddClaimAsync( Account account, Claim claim )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			if ( claim == null ) throw new ArgumentNullException( nameof( claim ) );

			if ( account.Claims.SingleOrDefault( c => c.Type == claim.Type && c.Value == claim.Value ) == null )
			{
				_dbContext.Set<Account>().Attach( account );
				account.Claims.Add( new AccountClaim( claim ) );
			}

			return Task.FromResult<object>( null );
		}

		public Task RemoveClaimAsync( Account account, Claim claim )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			if ( claim == null ) throw new ArgumentNullException( nameof( claim ) );

			var userClaim = account.Claims.SingleOrDefault( c => c.Type == claim.Type && c.Value == claim.Value );
			if ( userClaim != null )
			{
				account.Claims.Remove( userClaim );
			}
			return Task.FromResult<object>( null );
		}

		public Task SetPasswordHashAsync( Account account, string passwordHash )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			if ( passwordHash == null ) throw new ArgumentNullException( nameof( passwordHash ) );

			account.Password = passwordHash;

			return Task.FromResult<object>( null );
		}

		public Task<string> GetPasswordHashAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			return Task.FromResult( account.Password );
		}

		public Task<bool> HasPasswordAsync( Account account )
		{
			if ( account == null ) throw new ArgumentNullException( nameof( account ) );
			return Task.FromResult( !string.IsNullOrEmpty( account.Password ) );
		}

		private readonly DbContext _dbContext;
	}

	public class ApplicationUserManager : UserManager<Account, int>
	{
		public ApplicationUserManager( IUserStore<Account, int> store )
			: base( store )
		{
		}

		public static ApplicationUserManager Create( IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context )
		{
			var manager = new ApplicationUserManager( new AccountStore( context.Get<ConferenceContext>() ) );
			manager.UserValidator = new UserValidator<Account, int>( manager )
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
				manager.UserTokenProvider = new DataProtectorTokenProvider<Account, int>( dataProtectionProvider.Create( "ASP.NET Identity" ) );
			}
			return manager;
		}
	}
}
