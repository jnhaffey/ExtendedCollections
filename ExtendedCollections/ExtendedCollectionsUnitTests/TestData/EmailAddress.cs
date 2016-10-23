using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExtendedCollectionsUnitTests.Annotations;

namespace ExtendedCollectionsUnitTests.TestData
{
	public class EmailAddress : INotifyPropertyChanged
	{
		private bool _isDefault;
		public string Address { get; set; }

		public bool IsDefault
		{
			get { return _isDefault; }
			set
			{
				if (value == _isDefault) return;
				_isDefault = value;
				OnPropertyChanged(nameof(IsDefault));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}