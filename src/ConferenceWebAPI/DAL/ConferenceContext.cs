using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ConferenceWebAPI.DAL
{
	public class ConferenceContext : IdentityDbContext<User, Role, int, Login, UserRole, Claim>
	{
		public ConferenceContext()
			: base( "DefaultConnection" )
		{
			//Database.SetInitializer( new ConferenceInitializer() );
		}

		public static ConferenceContext Create()
		{
			return new ConferenceContext();
		}

		public DbSet<Speaker> Speakers { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<Profile> Profiles { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<User>()
				.ToTable( "Users" )
				.HasOptional( u => u.Profile )
				.WithRequired( p => p.User );

			modelBuilder.Entity<Role>()
				.ToTable( "Role" );

			modelBuilder.Entity<UserRole>()
				.ToTable( "UserRole" )
				.HasKey( r => new { r.UserId, r.RoleId } );

			modelBuilder.Entity<Claim>()
				.ToTable( "UserClaim" );

			modelBuilder.Entity<Login>()
				.HasKey( l => new { l.LoginProvider, l.ProviderKey, l.UserId } )
				.ToTable( "UserLogin" );
		}
	}
}