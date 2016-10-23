using System;

namespace ExtendedCollections.Exceptions
{
	public class MultipleSingleValueException : Exception
	{
		public MultipleSingleValueException()
			: this("More than one single value was found in the collection.")
		{
		}

		public MultipleSingleValueException(string message)
			: base(message)
		{
		}

		public MultipleSingleValueException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}