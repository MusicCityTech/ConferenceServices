using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceWebAPI.Models
{
	public class Profile
	{
		public int Id { get; set; }

		public virtual Account User { get; set; }

		[Display( Name = "Email Address" )]
		[Required]
		[DataType( DataType.EmailAddress )]
		public string Email { get; set; }

		[Display( Name = "First Name" )]
		[Required]
		[MaxLength( 50 )]
		public string FirstName { get; set; }

		[Display( Name = "Last Name" )]
		[Required]
		[MaxLength( 50 )]
		public string LastName { get; set; }

		[Display( Name = "Blog/Website URL" )]
		[Required]
		[MaxLength( 255 )]
		public string Website { get; set; }

		[Display( Name = "Short Biography" )]
		[Required]
		[MaxLength( 3000 )]
		public string Bio { get; set; }

		[Display( Name = "Twitter Handle" )]
		[MaxLength( 18 )]
		public string TwitterHandle { get; set; }

		[Display( Name = "GitHub Username" )]
		[MaxLength( 39 )]
		public string GithubUsername { get; set; }

		[Display( Name = "LinkedIn Profile Url" )]
		[MaxLength( 255 )]
		public string LinkedInProfile { get; set; }
	}
}