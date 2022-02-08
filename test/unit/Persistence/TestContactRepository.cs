using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Persistence;
using Vuture.Persistence.Repositories;

namespace Vuture.Test.Unit.Persistence
{
    public class TestContactRepository
    {
        private ContactDbContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<ContactDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            return new ContactDbContext(options);
        }

        [SetUp]
        public void SetUp()
        {
        }
        #region Tests for Create
        [Test]
        [TestCase("James", "Cairns", "james.cairns@email.com", "Vuture", "Working", "Developer")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_CreateContact_Should_AddContactToDb(string firstName, string lastName, string email, string company, string status, string title)
        {
            var contact = new Contact()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };
            var db = GetTestDbContext();
            var contactRepo = new ContactRepository(db);
            contactRepo.CreateContact(contact);
        }
        #endregion

        #region Tests for Read
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Test_GetContactById_ShouldReceive_CorrectContact(int id)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            var contactRepo = new ContactRepository(db);

            //Act
            var result = contactRepo.GetContactById(id);

            //Assert
            if (result.Id == id)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(7)]
        public void Test_GetContactById_ShouldReceive_NotFoundRequestExceptionResponse(int id)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            var contactRepo = new ContactRepository(db);

            //Assert
            Assert.Throws<NotFoundRequestExceptionResponse>(() => contactRepo.GetContactById(id));
        }
        #endregion

        #region Tests for Update
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_UpdateContactInDb(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            var contact = new Contact()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();
            var contactRepo = new ContactRepository(db);
            contactRepo.UpdateContact(contact);
        }
        #endregion

        #region Tests for Delete
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Test_DeleteContactById_ShouldNot_ThrowException(int id)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            var contactRepo = new ContactRepository(db);

            //Act
            Assert.DoesNotThrow(() => contactRepo.DeleteContactById(id));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(7)]
        public void Test_DeleteContactById_ShouldReceive_NotFoundRequestExceptionResponse(int id)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            var contactRepo = new ContactRepository(db);

            //Assert
            Assert.Throws<NotFoundRequestExceptionResponse>(() => contactRepo.DeleteContactById(id));
        }
        #endregion
    }
}
