using System.Collections.Generic;
using System.ComponentModel;
using ExtendedCollections.Enums;
using ExtendedCollections.Exceptions;

namespace ExtendedCollections.Collections
{
	public class CollectionWithSingleValuePropertyConstraint<T, TPt> :
		CollectionWithPropertyConstraintBase<T, TPt> where T : INotifyPropertyChanged
	{
		#region Protected Overriden Methods

		protected override void CheckPropertyConstraint(T item)
		{
			if (!GetNewItemPropertyValue(item).Equals(_singleValue))
			{
				return;
			}
			if (!IsPropertyAlreadyUsed(_singleValue))
			{
				return;
			}
			if (_handlerType == SinglePropertyHandlerType.RESET_OTHERS_TO_DEFAULT)
			{
				foreach (var collectionItem in Items)
				{
					if (!GetNewItemPropertyValue(collectionItem).Equals(_singleValue))
					{
						continue;
					}
					UnRegisterPropertyChange(collectionItem);
					collectionItem.GetType().GetProperty(_propertyName).SetValue(collectionItem, _defaultValue);
					RegisterPropertyChange(collectionItem);
					break;
				}
			}
			else
			{
				throw new MultipleSingleValuePropertyException<T>(item, _propertyName);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		///     Instaniate a new instance of CollectionWithSingleValuePropertyConstraint
		/// </summary>
		/// <param name="propertyName">The property name of the T Item that can only have a single default value</param>
		/// <param name="singleValue">The single value for T</param>
		/// <param name="defaultValue">The default value for T to use</param>
		/// <param name="defaultHandler">Handler if more than one T Item has the single value</param>
		public CollectionWithSingleValuePropertyConstraint(string propertyName, TPt singleValue,
			TPt defaultValue = default(TPt),
			SinglePropertyHandlerType defaultHandler = SinglePropertyHandlerType.RESET_OTHERS_TO_DEFAULT)
			: base(propertyName)
		{
			_singleValue = singleValue;
			_defaultValue = defaultValue;
			_handlerType = defaultHandler;
		}

		/// <summary>
		///     Instaniate a new instance of CollectionWithSingleValuePropertyConstraint
		/// </summary>
		/// <param name="anotherCollection">Another Enumerable Collection Type</param>
		/// <param name="propertyName">The property name of the T Item that can only have a single default value</param>
		/// <param name="singleValue">The single value for T</param>
		/// <param name="defaultValue">The default value for T to use</param>
		/// <param name="defaultHandler">Handler if more than one T Item has the single value</param>
		public CollectionWithSingleValuePropertyConstraint(IEnumerable<T> anotherCollection, string propertyName,
			TPt singleValue,
			TPt defaultValue = default(TPt),
			SinglePropertyHandlerType defaultHandler = SinglePropertyHandlerType.RESET_OTHERS_TO_DEFAULT)
			: this(propertyName, singleValue, defaultValue, defaultHandler)
		{
			foreach (var item in anotherCollection)
			{
				CheckPropertyConstraint(item);
				RegisterPropertyChange(item);
				Items.Add(item);
			}
		}

		#endregion

		#region Private Fields

		private readonly TPt _defaultValue;
		private readonly SinglePropertyHandlerType _handlerType;
		private readonly TPt _singleValue;

		#endregion
	}
}