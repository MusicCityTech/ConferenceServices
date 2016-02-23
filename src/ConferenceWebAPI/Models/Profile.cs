using System.ComponentModel.DataAnnotations;

namespace ConferenceWebAPI.Models
{
	public class Profile
	{
		public int Id { get; set; }

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
	}
}