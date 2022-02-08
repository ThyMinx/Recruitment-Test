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

        #region Tests for Create
        [Test]
        [TestCase("James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        [TestCase("James", "Cairns", "cairns.james@email.com", "", "Holiday", "Lead")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", "", "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", "")]
        [TestCase("James", "Cairns", "cairns.james@email.com", null, "Holiday", "Lead")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", null, "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", null)]
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

        [Test]
        [TestCase("James", "Cairns", "james.cairns@email.com", "Vuture", "Working", "Developer")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_CreateContact_Should_ThrowException(string firstName, string lastName, string email, string company, string status, string title)
        {
            var dbContact = new Contact()
            {
                Id = 1,
                FirstName = "a",
                LastName = "a",
                EmailAddress = email,
                Company = "a",
                Status = "a",
                Title = "a"
            };
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
            db.Contacts.Add(dbContact);
            db.SaveChanges();
            var contactRepo = new ContactRepository(db);
            Assert.Throws<BadRequestExceptionResponse>(() => contactRepo.CreateContact(contact));
        }
        #endregion

        #region Tests for Read Single
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

        #region Tests for Read Multiple
        [Test]
        [TestCase("Vuture")]
        [TestCase("Smiths")]
        [TestCase("Total")]
        [TestCase("vuture")]
        [TestCase("stuff")]
        [TestCase("company")]
        public void Test_GetContactsByCompany_ShouldReceive_CorrectContacts(string company)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = company,Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "unknown",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "unkown",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            List<Contact> expected = data.Where(d => d.Company == company).ToList();

            var contactRepo = new ContactRepository(db);

            //Act
            var result = contactRepo.GetContactsByCompany(company);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("nothing")]
        [TestCase("something")]
        [TestCase("microsoft")]
        public void Test_GetContactsByCompany_ShouldReceive_NotFoundRequestExceptionResponse(string company)
        {
            //Arrange
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "unknown",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "unkown",Status = "Working",Title = "Lead" }
            };
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();

            var contactRepo = new ContactRepository(db);

            //Assert
            Assert.Throws<NotFoundRequestExceptionResponse>(() => contactRepo.GetContactsByCompany(company));
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

        [Test]
        [TestCase(1, "James", "Cairns", "Judy.Law@email.com", "Vuture", "Working", "Developer")]
        [TestCase(3, "Fawn", "Massey", "Deals@email.au", "Vuture", "Working", "Lead")]
        [TestCase(5, "Ian", "Tufft", "Chris@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_ThrowException(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = "Vuture",Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = "Vuture",Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "Vuture",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = "Something",Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "Something",Status = "Working",Title = "Lead" }
            };
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
            var db = GetTestDbContext();
            data.ForEach(c => db.Contacts.Add(c));
            db.SaveChanges();
            var contactRepo = new ContactRepository(db);
            Assert.Throws<BadRequestExceptionResponse>(() => contactRepo.UpdateContact(contact));
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
