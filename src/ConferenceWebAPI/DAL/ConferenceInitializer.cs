using System;
using ConferenceWebAPI.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace ConferenceWebAPI.DAL
{
	public class ConferenceInitializer : System.Data.Entity.DropCreateDatabaseAlways<ConferenceContext>
	{
		protected async override void Seed( ConferenceContext context )
		{

			context.Roles.Add( new Role( AccountRole.Attendee ) );
			context.Roles.Add( new Role( AccountRole.Organizer ) );
			context.Roles.Add( new Role( AccountRole.Speaker ) );
			context.Roles.Add( new Role( AccountRole.Volunteer ) );

			var uersEmail = Faker.Internet.FreeEmail();
			var passwordHasher = new PasswordHasher();
			var user = new Account
			{
				UserName = uersEmail,
				EmailConfirmed = true,
				Email = uersEmail,
				Password = passwordHasher.HashPassword( "secretpassword" )
			};
			
			user.AddRole( AccountRole.Speaker );
			user.Profile = new Profile
			{
				FirstName = Faker.Name.First(),
				LastName = Faker.Name.Last(),
				Bio = string.Join( Environment.NewLine, Faker.Lorem.Paragraphs( 5 ) ),
				Email = Faker.Internet.Email(),
				GithubUsername = Faker.Internet.UserName(),
				LinkedInProfile = Faker.Internet.DomainName(),
				TwitterHandle = Faker.Internet.UserName(),
				Website = Faker.Internet.DomainName()
			};
			context.Accounts.Add( user );

			var sessions = new List<Session>
			{
				new Session()
				{
					Abstract = "Abstract 1",
					AudienceLevel = "Beginner",
					Notes = "Notes 1",
					Outline = "Outline 1",
					Speaker = user,
					Title = "Angular Deep Dive"
				},
				new Session()
				{
					Abstract = "Abstract 2",
					AudienceLevel = "Beginner",
					Notes = "Notes 2",
					Outline = "Outline 2",
					Speaker = user,
					Title = "Why Elixir is cool"
				},
				new Session()
				{
					Abstract = "Abstract 3",
					AudienceLevel = "Beginner",
					Notes = "Notes 3",
					Outline = "Outline 3",
					Speaker = user,
					Title = "Learn OData"
				},
				new Session()
				{
					Abstract = "Abstract 4",
					AudienceLevel = "Beginner",
					Notes = "Notes 4",
					Outline = "Outline 4",
					Speaker = user,
					Title = "Lets Build Something"
				}
			};

			context.Sessions.AddRange( sessions );


			context.SaveChanges();
		}
	}
}