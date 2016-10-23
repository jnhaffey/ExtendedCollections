using System.ComponentModel;
using ExtendedCollections.Exceptions;

namespace ExtendedCollections.Collections
{
	public class CollectionWithUniqueValuePropertyConstraint<T, TPt> :
		CollectionWithPropertyConstraintBase<T, TPt> where T : INotifyPropertyChanged
	{
		public CollectionWithUniqueValuePropertyConstraint(string propertyName)
			: base(propertyName)
		{
		}

		#region Overrides of CollectionWithPropertyConstraintBase<T,TPt>

		protected override void CheckPropertyConstraint(T item)
		{
			var newItemValue = GetNewItemPropertyValue(item);
			foreach (var collectionItem in Items)
			{
				if (item.Equals(collectionItem))
				{
					continue;
				}
				var currentItemValue = GetNewItemPropertyValue(collectionItem);
				if (currentItemValue.Equals(newItemValue))
				{
					throw new NotUniquePropertyValueException<T>(_propertyName);
				}
			}
		}

		#endregion
	}
}