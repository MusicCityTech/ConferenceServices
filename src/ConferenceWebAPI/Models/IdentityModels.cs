using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ConferenceWebAPI.Models
{
	public class Login : IdentityUserLogin<int>
	{

	}

	public class UserRole : IdentityUserRole<int>
	{

	}

	public class Claim : IdentityUserClaim<int>
	{

	}

	public class Role : IdentityRole<int, UserRole>
	{

	}

	// You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class User : IdentityUser<int, Login, UserRole, Claim>
	{
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<User, int> manager, string authenticationType )
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync( this, authenticationType );
			// Add custom user claims here
			return userIdentity;
		}

		public virtual Profile Profile { get; set; }
	}
}