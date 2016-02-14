namespace ConferenceWebAPI.Models
{
	public class Session : ISubmission
	{
		public int ID { get; set; }
		public string ShortDescription { get; set; }
		public string Title { get; set; }
		public virtual Speaker Speaker { get; set; }
	}
}