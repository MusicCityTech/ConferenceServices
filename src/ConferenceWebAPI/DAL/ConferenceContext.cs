using ConferenceWebAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ConferenceWebAPI.DAL
{
	public class ConferenceContext : IdentityDbContext<ApplicationUser>
	{
		public ConferenceContext()
			: base( "DefaultConnection", throwIfV1Schema: false )
		{
			Database.SetInitializer( new ConferenceInitializer() );
		}

		public static ConferenceContext Create()
		{
			return new ConferenceContext();
		}

		public DbSet<Speaker> Speakers { get; set; }
		public DbSet<Session> Sessions { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			base.OnModelCreating( modelBuilder );
		}
	}
}