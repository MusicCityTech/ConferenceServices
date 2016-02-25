using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ConferenceWebAPI.DAL
{
	public class ConferenceContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
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

		public DbSet<Speaker> Speakers { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<Profile> Profiles { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<User>()
				.HasOptional( u => u.Profile )
				.WithRequired( p => p.User );

			base.OnModelCreating( modelBuilder );
		}
	}
}