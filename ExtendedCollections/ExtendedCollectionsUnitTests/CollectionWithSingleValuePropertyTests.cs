using System.Collections.Generic;
using ExtendedCollections.Collections;
using ExtendedCollections.Enums;
using ExtendedCollections.Exceptions;
using ExtendedCollectionsUnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedCollectionsUnitTests
{
	[TestClass]
	public class CollectionWithSingleValuePropertyTests
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
		public void Test_CollectionWithSingleValueProperty_ModifySingleObject_SinglePropertyChangedEventRaised()
		{
			// ARRANGE
			_person.EmailAddresses = new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true)
			{
				new EmailAddress {IsDefault = true, Address = "john.smith@example.com"}
			};
			_person.EmailAddresses.ItemPropertyChanged += (sender, args) => { _recievedEvents.Add(args.PropertyName); };

			// ACT
			_person.EmailAddresses[0].IsDefault = false;

			// ASSERT
			Assert.AreEqual(1, _recievedEvents.Count);
			Assert.AreEqual("IsDefault", _recievedEvents[0]);
		}

		[TestMethod]
		public void Test_CollectionWithSingleValueProperty_AddToCollection_Valid_HandlerDefault()
		{
			// ARRANGE
			_person.EmailAddresses = new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true)
			{
				new EmailAddress {IsDefault = true, Address = "john.smith@example.com"}
			};

			// ACT
			_person.EmailAddresses.Add(new EmailAddress
			{
				IsDefault = false,
				Address = "john.w.smith@example.com"
			});

			// ASSERT
			Assert.AreEqual(2, _person.EmailAddresses.Count);
			Assert.IsTrue(_person.EmailAddresses[0].IsDefault);
			Assert.IsFalse(_person.EmailAddresses[1].IsDefault);
		}

		[TestMethod]
		public void Test_CollectionWithSingleValueProperty_AddToCollection_Invalid_HandlerDefault()
		{
			// ARRANGE
			_person.EmailAddresses = new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true)
			{
				new EmailAddress {IsDefault = true, Address = "john.smith@example.com"}
			};

			// ACT
			_person.EmailAddresses.Add(new EmailAddress
			{
				IsDefault = true,
				Address = "john.w.smith@example.com"
			});

			// ASSERT
			Assert.AreEqual(2, _person.EmailAddresses.Count);
			Assert.IsFalse(_person.EmailAddresses[0].IsDefault);
			Assert.IsTrue(_person.EmailAddresses[1].IsDefault);
		}

		[TestMethod]
		[ExpectedException(typeof(MultipleSingleValueException), "More than one single value was found in the collection.")]
		public void Test_CollectionWithSingleValueProperty_AddToCollection_Invalid_HandlerThrowException()
		{
			// ARRANGE
			_person.EmailAddresses = new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true, false,
				SinglePropertyHandlerType.THROW_EXCEPTION)
			{
				new EmailAddress {IsDefault = true, Address = "john.smith@example.com"}
			};

			// ACT
			_person.EmailAddresses.Add(new EmailAddress
			{
				IsDefault = true,
				Address = "john.w.smith@example.com"
			});

			// ASSERT
			Assert.Fail();
		}

		[TestMethod]
		[ExpectedException(typeof(MultipleSingleValueException), "More than one single value was found in the collection.")]
		public void Test_CollectionWithSingleValueProperty_ChangeToCollection_Invalid_HandlerThrowException()
		{
			// ARRANGE
			_person.EmailAddresses = new CollectionWithSingleValueProperty<EmailAddress, bool>("IsDefault", true, false,
				SinglePropertyHandlerType.THROW_EXCEPTION)
			{
				new EmailAddress {IsDefault = true, Address = "john.smith@example.com"},
				new EmailAddress {IsDefault = false, Address = "john.w.smith@example.com"}
			};

			// ACT
			_person.EmailAddresses[1].IsDefault = true;

			// ASSERT
			Assert.Fail();
		}
	}
}