using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ExtendedCollections.Collections
{
	public abstract class CollectionWithPropertyConstraintBase<T, TPt> : Collection<T> where T : INotifyPropertyChanged
	{
		#region Protected Fields

		protected readonly string _propertyName;

		#endregion

		protected CollectionWithPropertyConstraintBase(string propertyName)
		{
			_propertyName = propertyName;
		}

		#region Public Events

		public event PropertyChangedEventHandler ItemPropertyChanged;

		#endregion

		#region Protected Abstract Methods

		protected abstract void CheckPropertyConstraint(T item);

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

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			CheckPropertyConstraint((T) sender);
			ItemPropertyChanged?.Invoke(sender, e);
		}

		#endregion

		#region Protected Methods

		protected void RegisterPropertyChange(T item)
		{
			item.PropertyChanged += OnPropertyChanged;
		}

		protected void UnRegisterPropertyChange(T item)
		{
			item.PropertyChanged -= OnPropertyChanged;
		}

		protected bool IsPropertyAlreadyUsed(TPt checkForValue)
		{
			var listedValues = Items.Select(item => new {item, propertyInfo = item.GetType().GetProperty(_propertyName)})
				.Where(t => t.propertyInfo != null)
				.Select(t => (TPt) t.propertyInfo.GetValue(t.item, null)).ToList();
			return listedValues.Any(value => listedValues.Count(p => p.Equals(checkForValue)) >= 1);
		}

		protected TPt GetNewItemPropertyValue(T item)
		{
			return (TPt) item.GetType().GetProperty(_propertyName).GetValue(item);
		}

		#endregion

		#region Protected Override Methods

		protected override void ClearItems()
		{
			UnRegisterPropertyChange(Items);
			base.ClearItems();
		}

		protected override void InsertItem(int index, T item)
		{
			CheckPropertyConstraint(item);
			RegisterPropertyChange(item);
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			CheckPropertyConstraint(item);
			RegisterPropertyChange(item);
			base.SetItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			UnRegisterPropertyChange(Items[index]);
			base.RemoveItem(index);
		}

		#endregion
	}
}