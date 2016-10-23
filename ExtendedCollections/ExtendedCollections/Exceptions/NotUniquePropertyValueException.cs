using System;
using System.ComponentModel;

namespace ExtendedCollections.Exceptions
{
	public class NotUniquePropertyValueException<T> : Exception where T : INotifyPropertyChanged
	{
		public NotUniquePropertyValueException(T item, string propertyName)
			: this($"More than one items in the collection has the same `{propertyName}` value.")
		{
			ItemInError = item;
		}

		public NotUniquePropertyValueException(string message)
			: base(message)
		{
		}

		public NotUniquePropertyValueException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public T ItemInError { get; set; }
	}
}