using ExtendedCollections;
using ExtendedCollections.Collections;

namespace ExtendedCollectionsUnitTests.TestData
{
	public class Person
	{
		public string Name { get; set; }

		public CollectionWithSingleValueProperty<EmailAddress, bool> EmailAddresses { get; set; } =
			new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true);
	}
}