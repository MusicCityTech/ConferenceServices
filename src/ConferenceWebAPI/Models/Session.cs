using System.ComponentModel.DataAnnotations;

namespace ConferenceWebAPI.Models
{
	public class Session
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }
		[Required]
		public string AudienceLevel { get; set; }
		[Required]
		public string Outline { get; set; }
		[Required]
		public string Abstract { get; set; }
		public string Notes { get; set; }
		
		public virtual Account Speaker { get; set; }
	}
}