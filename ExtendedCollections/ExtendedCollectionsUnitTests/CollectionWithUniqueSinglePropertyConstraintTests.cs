using System.Collections.Generic;
using ExtendedCollections.Collections;
using ExtendedCollections.Exceptions;
using ExtendedCollectionsUnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedCollectionsUnitTests
{
	[TestClass]
	public class CollectionWithUniqueSinglePropertyConstraintTests
	{
		private Person _person;
		private List<string> _recievedEvents;

		[TestInitialize]
		public void InitializeTestData()
		{
			_recievedEvents = new List<string>();

			_person = new Person {Name = "John Doe"};
		}

		[TestCleanup]
		public void CleanupTestData()
		{
			_person = null;
			_recievedEvents = null;
		}

		[TestMethod]
		public void Test_CollectionWithUniqueSinglePropertyConstraint_ModifySingleObject_SinglePropertyChangedEventRaised()
		{
			// ARRANGE
			_person.Demographics =
				new CollectionWithUniqueValuePropertyConstraint<Demographic, DemographicType>("DemographicType")
				{
					new Demographic {DemographicType = DemographicType.AGE, Value = "21"}
				};
			_person.Demographics.ItemPropertyChanged += (sender, args) => { _recievedEvents.Add(args.PropertyName); };

			// ACT
			_person.Demographics[0].DemographicType = DemographicType.GENDER;

			// ASSERT
			Assert.AreEqual(1, _recievedEvents.Count);
			Assert.AreEqual("DemographicType", _recievedEvents[0]);
		}

		[TestMethod]
		public void Test_CollectionWithUniqueSinglePropertyConstraint_AddToCollection_Valid()
		{
			// ARRANGE
			_person.Demographics =
				new CollectionWithUniqueValuePropertyConstraint<Demographic, DemographicType>("DemographicType")
				{
					new Demographic {DemographicType = DemographicType.AGE, Value = "21"}
				};

			// ACT
			_person.Demographics.Add(new Demographic
			{
				DemographicType = DemographicType.GENDER,
				Value = "Male"
			});

			// ASSERT
			Assert.AreEqual(2, _person.Demographics.Count);
			Assert.AreEqual(DemographicType.AGE, _person.Demographics[0].DemographicType);
			Assert.AreEqual("21", _person.Demographics[0].Value);
			Assert.AreEqual(DemographicType.GENDER, _person.Demographics[1].DemographicType);
			Assert.AreEqual("Male", _person.Demographics[1].Value);
		}

		[TestMethod]
		[ExpectedException(typeof(NotUniquePropertyValueException<Demographic>))]
		public void Test_CollectionWithUniqueSinglePropertyConstraint_AddToCollection_Invalid()
		{
			// ARRANGE
			_person.Demographics =
				new CollectionWithUniqueValuePropertyConstraint<Demographic, DemographicType>("DemographicType")
				{
					new Demographic {DemographicType = DemographicType.AGE, Value = "21"}
				};

			// ACT
			_person.Demographics.Add(new Demographic
			{
				DemographicType = DemographicType.AGE,
				Value = "Male"
			});

			// ASSERT
			Assert.Fail();
		}
	}
}