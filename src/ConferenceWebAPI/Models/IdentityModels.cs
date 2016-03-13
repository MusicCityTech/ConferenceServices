using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace ConferenceWebAPI.Models
{
	public class AccountLogin
	{
		public int Id { get; set; }
		public virtual string LoginProvider { get; set; }
		public virtual string ProviderKey { get; set; }

		public AccountLogin()
		{
			// Empty
		}

		public AccountLogin( UserLoginInfo userLoginInfo )
		{
			LoginProvider = userLoginInfo.LoginProvider;
			ProviderKey = userLoginInfo.ProviderKey;
		}
	}

	public class AccountClaim
	{
		public int Id { get; set; }
		public AccountClaim()
		{
			// Empty
		}

		public AccountClaim( Claim claim )
		{
			Type = claim.Type;
			Value = claim.Value;
		}

		public virtual string Type { get; set; }
		public virtual string Value { get; set; }
	}

	public enum AccountRole
	{
		Unkown = 0,
		Organizer = 1,
		Volunteer = 2,
		Speaker = 3,
		Attendee = 4
	}

	public class Role
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public Role()
		{
			// Empty
		}

		public Role( AccountRole role )
		{
			Id = (int)role;
			Name = role.ToString();
		}

		public static Role CreateRole( AccountRole role )
		{
			return new Role
			{
				Id = (int)role,
				Name = role.ToString()
			};
		}
	}

	public class Account : IUser<int>
	{

		public virtual int Id { get; set; }
		public virtual string UserName { get; set; }
		public virtual string Password { get; set; }
		public virtual string Email { get; set; }
		public virtual bool EmailConfirmed { get; set; }
		public virtual string DisplayName { get; set; }
		public virtual DateTimeOffset CreatedAt { get; set; }
		public virtual DateTimeOffset? LastActivity { get; set; }
		public virtual int FailedLoginAttempts { get; set; }
		public virtual bool LockoutEnabled { get; set; }
		public virtual DateTimeOffset? LockoutEndDate { get; set; }
		public virtual string PublicKey { get; set; }
		public virtual Profile Profile { get; set; }

		public virtual IList<Role> Roles
		{
			get
			{
				_roles = _roles ?? new List<Role>();
				return _roles;
			}
			set { _roles = value; }
		}

		public virtual IList<AccountClaim> Claims
		{
			get
			{
				_claims = _claims ?? new List<AccountClaim>();
				return _claims;
			}
			set { _claims = value; }
		}

		public virtual IList<AccountLogin> Logins
		{
			get
			{
				_logins = _logins ?? new List<AccountLogin>();
				return _logins;
			}
			set { _logins = value; }
		}

		public virtual Account AddRole( AccountRole role )
		{
			if ( Roles.All( r => r.Id != (int)role ) )
			{
				Roles.Add( Role.CreateRole( role ) );
			}
			return this;
		}

		public virtual Account AddRoles( IList<AccountRole> roles )
		{
			foreach ( var role in roles.Where( role => Roles.All( r => r.Id != (int)role ) ) )
			{
				Roles.Add( Role.CreateRole( role ) );
			}
			return this;
		}

		public virtual Account RemoveRole( AccountRole role )
		{
			var roleToRemove = Roles.SingleOrDefault( r => r.Id == (int)role );
			if ( roleToRemove != null )
			{
				Roles.Remove( roleToRemove );
			}
			return this;
		}

		public Account RemoveRoles( IList<AccountRole> roles )
		{
			foreach ( var role in roles.Select( role => Roles.SingleOrDefault( r => r.Id == (int)role ) ).Where( role => role != null ) )
			{
				Roles.Remove( role );
			}
			return this;
		}

		public virtual bool HasRole( AccountRole role )
		{
			return Roles.Any( r => r.Id == (int)role );
		}

		public virtual bool EmailEditAllowed => !Logins.Any();

		public virtual bool UserNameEditAllowed => !Logins.Any();

		public virtual bool PasswordResetAllowed => !Logins.Any();

		public static int SystemAccountId = 1;

		private IList<Role> _roles;
		private IList<AccountClaim> _claims;
		private IList<AccountLogin> _logins;
	}
}