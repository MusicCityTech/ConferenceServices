using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceWebAPI.DAL
{
	public class ConferenceContext : DbContext
	{
		public ConferenceContext()
			: base( "DefaultConnection" )
		{
			Database.SetInitializer( new ConferenceInitializer() );
		}

		public static ConferenceContext Create()
		{
			return new ConferenceContext();
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<AccountClaim> AccountClaims { get; set; }
		public DbSet<AccountLogin> AccountLogins { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<Profile> Profiles { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Account>()
						.Property( m => m.Id )
						.HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );
			modelBuilder.Entity<Account>()
						.Property( m => m.UserName )
						.IsRequired()
						.HasMaxLength( 256 );
			modelBuilder.Entity<Account>()
						.Property( m => m.Password )
						.HasMaxLength( 256 );
			modelBuilder.Entity<Account>()
						.Property( m => m.Email )
						.HasMaxLength( 254 );
			modelBuilder.Entity<Account>()
						.HasMany( m => m.Roles )
						.WithMany()
						.Map( m =>
						{
							m.ToTable( "AccountRole" );
							m.MapLeftKey( "AccountId" );
							m.MapRightKey( "RoleId" );
						} );
			modelBuilder.Entity<Account>()
						.HasMany( m => m.Claims )
						.WithRequired()
						.Map( m => m.MapKey( "AccountId" ) )
						.WillCascadeOnDelete();
			modelBuilder.Entity<Account>()
						.HasMany( m => m.Logins )
						.WithRequired()
						.Map( m => m.MapKey( "AccountId" ) )
						.WillCascadeOnDelete();


			modelBuilder.Entity<Role>()
						.Property( m => m.Id )
						.HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );
			modelBuilder.Entity<Role>()
						.Property( m => m.Name )
						.IsRequired()
						.HasMaxLength( 256 );

			modelBuilder.Entity<AccountClaim>()
						.Property( m => m.Id )
						.HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );
			modelBuilder.Entity<AccountClaim>()
						.Property( m => m.Type )
						.IsRequired()
						.HasMaxLength( 256 );
			modelBuilder.Entity<AccountClaim>()
						.Property( m => m.Value )
						.HasMaxLength( 256 );

			modelBuilder.Entity<AccountLogin>()
						.Property( m => m.Id )
						.HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );
			modelBuilder.Entity<AccountLogin>()
						.Property( m => m.LoginProvider )
						.IsRequired()
						.HasMaxLength( 50 );
			modelBuilder.Entity<AccountLogin>()
						.Property( m => m.ProviderKey )
						.IsRequired()
						.HasMaxLength( 400 );

			modelBuilder.Entity<Profile>()
						.HasRequired( m => m.User )
						.WithOptional( m => m.Profile );

		}
	}
}