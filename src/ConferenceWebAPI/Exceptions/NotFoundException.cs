using System;

namespace ConferenceWebAPI.Exceptions
{
	public class NotFoundException  : Exception
	{
		public NotFoundException(string message)
			: base( message )
		{

		}
	}
}