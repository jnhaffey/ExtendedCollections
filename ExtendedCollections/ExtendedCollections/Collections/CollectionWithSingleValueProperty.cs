using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ExtendedCollections.Enums;
using ExtendedCollections.Exceptions;

namespace ExtendedCollections.Collections
{
	public sealed class CollectionWithSingleValueProperty<T, TPt> : Collection<T> where T : INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler ItemPropertyChanged;

		#endregion

		#region Constructors

		/// <summary>
		///     Instaniate a new instance of CollectionWithSingleValueProperty
		/// </summary>
		/// <param name="propertyName">The property name of the T Item that can only have a single default value</param>
		/// <param name="singleValue">The single value for T</param>
		/// <param name="defaultValue">The default value for T to use</param>
		/// <param name="defaultHandler">Handler if more than one T Item has the single value</param>
		public CollectionWithSingleValueProperty(string propertyName, TPt singleValue,
			TPt defaultValue = default(TPt),
			SinglePropertyHandlerType defaultHandler = SinglePropertyHandlerType.RESET_OTHERS_TO_DEFAULT)
		{
			_propertyName = propertyName;
			_singleValue = singleValue;
			_defaultValue = defaultValue;
			_handlerType = defaultHandler;
		}

		/// <summary>
		///     Instaniate a new instance of CollectionWithSingleValueProperty
		/// </summary>
		/// <param name="anotherCollection">Another Enumerable Collection Type</param>
		/// <param name="propertyName">The property name of the T Item that can only have a single default value</param>
		/// <param name="singleValue">The single value for T</param>
		/// <param name="defaultValue">The default value for T to use</param>
		/// <param name="defaultHandler">Handler if more than one T Item has the single value</param>
		public CollectionWithSingleValueProperty(IEnumerable<T> anotherCollection, string propertyName, TPt singleValue,
			TPt defaultValue = default(TPt),
			SinglePropertyHandlerType defaultHandler = SinglePropertyHandlerType.RESET_OTHERS_TO_DEFAULT)
			: this(propertyName, singleValue, defaultValue, defaultHandler)
		{
			foreach (var item in anotherCollection)
			{
				CheckSingleValueProperty(item);
				RegisterPropertyChange(item);
				Items.Add(item);
			}
		}

		#endregion

		#region Private Fields

		private readonly TPt _singleValue;
		private readonly string _propertyName;
		private readonly TPt _defaultValue;
		private readonly SinglePropertyHandlerType _handlerType;

		#endregion

		#region Overrides of Collection<T>

		protected override void ClearItems()
		{
			UnRegisterPropertyChange(Items);
			base.ClearItems();
		}


		protected override void InsertItem(int index, T item)
		{
			CheckSingleValueProperty(item);
			RegisterPropertyChange(item);
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			CheckSingleValueProperty(item);
			RegisterPropertyChange(item);
			base.SetItem(index, item);
		}


		protected override void RemoveItem(int index)
		{
			UnRegisterPropertyChange(Items[index]);
			base.RemoveItem(index);
		}

		#endregion

		#region Private Methods

		private void RegisterPropertyChange(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				if (item != null)
				{
					RegisterPropertyChange(item);
				}
			}
		}

		private void RegisterPropertyChange(T item)
		{
			item.PropertyChanged += OnPropertyChanged;
		}

		private void UnRegisterPropertyChange(IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				if (item != null)
				{
					UnRegisterPropertyChange(item);
				}
			}
		}

		private void UnRegisterPropertyChange(T item)
		{
			item.PropertyChanged -= OnPropertyChanged;
		}

		private bool IsPropertyAlreadyUsed()
		{
			var listedValues = Items.Select(item => new {item, propertyInfo = item.GetType().GetProperty(_propertyName)})
				.Where(t => t.propertyInfo != null)
				.Select(t => (TPt) t.propertyInfo.GetValue(t.item, null)).ToList();
			return listedValues.Any(value => listedValues.Count(p => p.Equals(_singleValue)) >= 1);
		}

		private TPt GetNewItemPropertyValue(T item)
		{
			return (TPt) item.GetType().GetProperty(_propertyName).GetValue(item);
		}

		private void CheckSingleValueProperty(T item)
		{
			if (!GetNewItemPropertyValue(item).Equals(_singleValue))
			{
				return;
			}
			if (!IsPropertyAlreadyUsed())
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
				throw new MultipleSingleValueException();
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			CheckSingleValueProperty((T) sender);
			ItemPropertyChanged?.Invoke(sender, e);
		}

		#endregion
	}
}