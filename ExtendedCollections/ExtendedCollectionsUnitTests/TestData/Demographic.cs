using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExtendedCollectionsUnitTests.Annotations;

namespace ExtendedCollectionsUnitTests.TestData
{
	public class Demographic : INotifyPropertyChanged
	{
		private DemographicType _demographicType;

		public DemographicType DemographicType
		{
			get { return _demographicType; }
			set
			{
				if (_demographicType == value)
				{
					return;
				}
				_demographicType = value;
				OnPropertyChanged(nameof(DemographicType));
			}
		}

		public string Value { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}