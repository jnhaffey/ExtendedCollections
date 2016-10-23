using ExtendedCollections.Collections;

namespace ExtendedCollectionsUnitTests.TestData
{
	public class Person
	{
		public string Name { get; set; }

		public CollectionWithSingleValuePropertyConstraint<EmailAddress, bool> EmailAddresses { get; set; } =
			new CollectionWithSingleValuePropertyConstraint<EmailAddress, bool>("IsDefault", true);

		public CollectionWithUniqueValuePropertyConstraint<Demographic, DemographicType> Demographics { get; set; } =
			new CollectionWithUniqueValuePropertyConstraint<Demographic, DemographicType>("DemographicType");
	}
}