using ExtendedCollections;
using ExtendedCollections.Collections;

namespace ExtendedCollectionsUnitTests.TestData
{
	public class Person
	{
		public string Name { get; set; }

		public CollectionWithSinglePropertyPropertyConstraint<EmailAddress, bool> EmailAddresses { get; set; } =
			new CollectionWithSinglePropertyPropertyConstraint<EmailAddress, bool>("IsDefault", true);
	}
}