using System;
using System.ComponentModel;

namespace ExtendedCollections.Exceptions
{
	public class MultipleSingleValuePropertyException<T> : Exception where T: INotifyPropertyChanged
	{
		public MultipleSingleValuePropertyException(T item, string propertyName)
			: this($"Another item in the collection has the same `{propertyName}` value.")
		{
			ItemInError = item;
		}

		public MultipleSingleValuePropertyException(string message)
			: base(message)
		{
		}

		public MultipleSingleValuePropertyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public T ItemInError { get; set; }
	}
}