using System;
using System.Linq;
using System.Net;
using CalorieTracker.Backend.Models;
using CalorieTracker.Backend.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace CalorieTracker.Backend.Tests.Tests
{
	/// <summary>
	/// Tests entries api.
	/// </summary>
	[TestClass]
	public class EntriesTests
	{
		/// <summary>
		/// Basic actions.
		/// </summary>
		[TestMethod]
		public void CreateReadUpdateEntry()
		{
			var api = Api.RegisterAndLogon();

			// create new entry
			var entry = CreateEntry();
			var savedEntry = api.Entries.Save(entry);
			// make sure non-empty id is returned
			savedEntry.Id.ShouldNotEqual(0);
			entry.Id = savedEntry.Id;
			checkEntriesAreEqual(savedEntry, entry);

			// check that entry now appears in the list
			var entries = api.Entries.List();
			entries.ShouldContain(e => e.Id == savedEntry.Id);
			checkEntriesAreEqual(entries.Single(e => e.Id == savedEntry.Id), entry);

			// check get method
			checkEntriesAreEqual(api.Entries.Get(savedEntry.Id), entry);

			// update existing entry
			entry.Comment = "Another comment";
			entry.Calories += 100;
			savedEntry = api.Entries.Save(entry);
			checkEntriesAreEqual(savedEntry, entry);

			// check that updated entry is persisted
			checkEntriesAreEqual(api.Entries.Get(entry.Id), entry);

			// delete entry
			api.Entries.Delete(entry.Id);
			// can't delete it twice
			api.Entries.Delete(entry.Id, HttpStatusCode.NotFound);
		}

		public static Entry CreateEntry(string comment = "Comment", DateTime? date = null)
		{
			return new Entry
			{
				Comment = comment,
				Calories = 100,
				Date = date ?? DateTime.Now
			};
		}

		private void checkEntriesAreEqual(Entry actual, Entry expected)
		{
			actual.Id.ShouldEqual(expected.Id);
			actual.Comment.ShouldEqual(expected.Comment);
			actual.Calories.ShouldEqual(expected.Calories);
			actual.Date.ShouldEqual(expected.Date);
			actual.User.ShouldEqual(expected.User);
		}

		/// <summary>
		/// Ensures that entries from different users are visible only to their owners.
		/// </summary>
		[TestMethod]
		public void TwoUsersEntries()
		{
			// create user, authorize and create entry
			var api1 = Api.RegisterAndLogon();
			var entry1 = api1.Entries.Save(CreateEntry());

			var api2 = Api.RegisterAndLogon();
			var entry2 = api2.Entries.Save(CreateEntry());

			// make sure that created entries appear in the entry list
			api1.Entries.List().ShouldContain(e => e.Id == entry1.Id);
			api2.Entries.List().ShouldContain(e => e.Id == entry2.Id);

			// make sure that created entries do not appear in the list of another user
			api2.Entries.List().ShouldNotContain(e => e.Id == entry1.Id);
			api1.Entries.List().ShouldNotContain(e => e.Id == entry2.Id);

			// can delete only own entries
			api1.Entries.Delete(entry2.Id, HttpStatusCode.NotFound);
			api2.Entries.Delete(entry2.Id);
		}

		[TestMethod]
		public void Validation()
		{
			var api = Api.RegisterAndLogon();
			var entry = CreateEntry();
			entry.Date = null;
			api.Entries.SaveWithError(entry, HttpStatusCode.BadRequest);

			entry = CreateEntry();
			entry.Calories = null;
			api.Entries.SaveWithError(entry, HttpStatusCode.BadRequest);
			entry.Calories = -1;
			api.Entries.SaveWithError(entry, HttpStatusCode.BadRequest);

			entry = CreateEntry();
			entry.Comment = null;
			api.Entries.SaveWithError(entry, HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public void Filtering()
		{
			var api = Api.RegisterAndLogon();
			var date1 = new DateTime(2012, 10, 06, 10, 55, 0);
			var entry1 = api.Entries.Save(CreateEntry(date: date1));

			var date2 = new DateTime(2014, 10, 06, 17, 55, 0);
			var entry2 = api.Entries.Save(CreateEntry(date: date2));

			var entries = api.Entries.List();
			entries.ShouldContain(e => e.Id == entry1.Id);
			entries.ShouldContain(e => e.Id == entry2.Id);

			entries = api.Entries.List(dateFrom: date2);
			entries.ShouldNotContain(e => e.Id == entry1.Id);
			entries.ShouldContain(e => e.Id == entry2.Id);

			entries = api.Entries.List(dateTo: date1);
			entries.ShouldContain(e => e.Id == entry1.Id);
			entries.ShouldNotContain(e => e.Id == entry2.Id);

			entries = api.Entries.List(timeFrom: date2.TimeOfDay);
			entries.ShouldNotContain(e => e.Id == entry1.Id);
			entries.ShouldContain(e => e.Id == entry2.Id);

			entries = api.Entries.List(timeTo: date1.TimeOfDay);
			entries.ShouldContain(e => e.Id == entry1.Id);
			entries.ShouldNotContain(e => e.Id == entry2.Id);
		}

		[TestMethod]
		public void IfLimitIsSetSearchShouldReturnSetNumberOfEntities()
		{
			// Arrange
			var api = Api.RegisterAndLogon();
			for (var i = 0; i < 10; i++)
			{
				api.Entries.Save(CreateEntry());
			}
			
			// Act
			var entries = api.Entries.Search("Comment", 4);
			
			// Assert
			Assert.IsTrue(entries.Length == 4);
		}
		
		[TestMethod]
		public void IfCommentIsNewSearchShouldReturEmptyArray()
		{
			// Arrange
			var api = Api.RegisterAndLogon();
			for (var i = 0; i < 10; i++)
			{
				api.Entries.Save(CreateEntry());
			}
			
			// Act
			var entries = api.Entries.Search("Commentator", 4);
			
			// Assert
			Assert.IsTrue(entries.Length == 0);
		}
		
		[TestMethod]
		public void SearchShouldIgnoreCase()
		{
			// Arrange
			var api = Api.RegisterAndLogon();
			for (var i = 0; i < 10; i++)
			{
				api.Entries.Save(CreateEntry());
			}
			
			// Act
			var entries = api.Entries.Search("com", 4);
			
			// Assert
			Assert.IsTrue(entries.Length == 4);
		}
	}
}
