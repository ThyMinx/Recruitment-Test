using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Models.Dtos;
using Vuture.Persistence.Repositories.Interfaces;
using Vuture.Services;

namespace Vuture.Test.Unit.Services
{
    public class TestContactService
    {
        private Mock<IContactRepository> _mockRepo = new Mock<IContactRepository>();
        private ContactService _contactService;

        public ContactService GetContactService()
        {
            _contactService = new ContactService(_mockRepo.Object);
            return _contactService;
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
            var createdDbContact = new Contact()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            var dtoPassedIn = new CreateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            _mockRepo.Setup(x => x.CreateContact(It.IsAny<Contact>())).Returns(createdDbContact);

            var service = GetContactService();
            var result = service.CreateContact(dtoPassedIn);
            Assert.IsTrue(result.GetType().Equals(typeof(ReadContactDto)));
            Assert.AreEqual(createdDbContact.Id, result.Id);
            Assert.AreEqual(createdDbContact.FirstName, result.FirstName);
            Assert.AreEqual(createdDbContact.LastName, result.LastName);
            Assert.AreEqual(createdDbContact.EmailAddress, result.EmailAddress);
            Assert.AreEqual(createdDbContact.Company, result.Company);
            Assert.AreEqual(createdDbContact.Status, result.Status);
            Assert.AreEqual(createdDbContact.Title, result.Title);
        }
        [Test]
        [TestCase("James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase("Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_CreateContact_Should_Throw400Exception(string firstName, string lastName, string email, string company, string status, string title)
        {
            var dtoPassedIn = new CreateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            _mockRepo.Setup(x => x.CreateContact(It.IsAny<Contact>())).Throws(new BadRequestExceptionResponse("There is already a contact with the email: " + email));

            var service = GetContactService();
            Assert.Throws<BadRequestExceptionResponse>(() => service.CreateContact(dtoPassedIn));
        }
        #endregion

        #region Tests for Read Single
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_GetContactById_ShouldReceive_ReadContactDto(int id, string firstName, string lastName, string email, string company, string status, string title)
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
            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(contact);
            var service = GetContactService();

            var result = service.GetContactById(id);

            Assert.IsTrue(result.GetType().Equals(typeof(ReadContactDto)));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        public void Test_GetContactById_Should_ThrowException(int id)
        {
            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            var service = GetContactService();

            Assert.Throws<NotFoundRequestExceptionResponse>(() => service.GetContactById(id));
        }
        #endregion

        #region Tests for Read Multiple
        [Test]
        [TestCase("Business")]
        [TestCase("Vuture")]
        [TestCase("Bank")]
        [TestCase("Company")]
        public void Test_GetContactsByCompany_ShouldReceive_ReadContactDtos(string company)
        {
            List<Contact> data = new List<Contact>()
            {
                new Contact { Id = 1,FirstName = "James",LastName = "Cairns",EmailAddress = "James.Cairns@email.com",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 2,FirstName = "Judy",LastName = "Law",EmailAddress = "Judy.Law@email.com",Company = company,Status = "Working",Title = "Lead" },
                new Contact { Id = 3,FirstName = "Aaron",LastName = "Aaronson",EmailAddress = "Aaron@email.co.uk",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 4,FirstName = "Daniel",LastName = "Dealer",EmailAddress = "Deals@email.au",Company = "unknown",Status = "Working",Title = "Accounts" },
                new Contact { Id = 5,FirstName = "Dave",LastName = "Bonting",EmailAddress = "Dave@email.co",Company = company,Status = "Working",Title = "Developer" },
                new Contact { Id = 6,FirstName = "Chris",LastName = "Drews",EmailAddress = "Chris@email.com",Company = "unkown",Status = "Working",Title = "Lead" }
            };
            List<Contact> returned = data.Where(c => c.Company == company).ToList();
            List<ReadContactDto> expected = new List<ReadContactDto>();
            returned.ForEach(c => expected.Add(new ReadContactDto() { 
                Id = c.Id, FirstName = c.FirstName, LastName = c.LastName, EmailAddress = c.EmailAddress, Company = c.Company, Status = c.Status, Title = c.Title 
            }));
            _mockRepo.Setup(x => x.GetContactsByCompany(It.IsAny<string>())).Returns(returned);
            var service = GetContactService();

            var result = service.GetContactsByCompany(company);

            Assert.AreEqual(result.Count, expected.Count);
            for (int i = 1; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Id, expected[i].Id);
                Assert.AreEqual(result[i].FirstName, expected[i].FirstName);
                Assert.AreEqual(result[i].LastName, expected[i].LastName);
                Assert.AreEqual(result[i].EmailAddress, expected[i].EmailAddress);
                Assert.AreEqual(result[i].Company, expected[i].Company);
                Assert.AreEqual(result[i].Status, expected[i].Status);
                Assert.AreEqual(result[i].Title, expected[i].Title);
            }
        }

        [Test]
        [TestCase("something")]
        [TestCase("microsoft")]
        [TestCase("apple")]
        public void Test_GetContactsByCompany_Should_ThrowException(string company)
        {
            _mockRepo.Setup(x => x.GetContactsByCompany(It.IsAny<string>())).Throws(new NotFoundRequestExceptionResponse("No contact with the company: " + company));
            var service = GetContactService();

            Assert.Throws<NotFoundRequestExceptionResponse>(() => service.GetContactsByCompany(company));
        }
        #endregion

        #region Tests for Update
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_AddContactToDb(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            var dbContact = new Contact()
            {
                Id = id,
                FirstName = "a",
                LastName = "b",
                EmailAddress = "c",
                Company = "d",
                Status = "e",
                Title = "f"
            };

            var updatedDbContact = new Contact()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            var dtoPassedIn = new UpdateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(dbContact);
            _mockRepo.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(updatedDbContact);

            var service = GetContactService();
            var result = service.UpdateContactById(id, dtoPassedIn);
            Assert.IsTrue(result.GetType().Equals(typeof(ReadContactDto)));
            Assert.AreEqual(updatedDbContact.Id, result.Id);
            Assert.AreEqual(updatedDbContact.FirstName, result.FirstName);
            Assert.AreEqual(updatedDbContact.LastName, result.LastName);
            Assert.AreEqual(updatedDbContact.EmailAddress, result.EmailAddress);
            Assert.AreEqual(updatedDbContact.Company, result.Company);
            Assert.AreEqual(updatedDbContact.Status, result.Status);
            Assert.AreEqual(updatedDbContact.Title, result.Title);
        }
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_Throw404Exception(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            var dtoPassedIn = new UpdateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            _mockRepo.Setup(x => x.UpdateContact(It.IsAny<Contact>()));

            var service = GetContactService();
            Assert.Throws<NotFoundRequestExceptionResponse>(() => service.UpdateContactById(id, dtoPassedIn));
        }
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_Throw400Exception(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            var dbContact = new Contact()
            {
                Id = id,
                FirstName = "a",
                LastName = "b",
                EmailAddress = "c",
                Company = "d",
                Status = "e",
                Title = "f"
            };

            var dtoPassedIn = new UpdateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };

            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(dbContact);
            _mockRepo.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Throws(new BadRequestExceptionResponse("There is already a contact with the email: " + email));

            var service = GetContactService();
            Assert.Throws<BadRequestExceptionResponse>(() => service.UpdateContactById(id, dtoPassedIn));
        }
        #endregion

        #region Tests for Delete
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        public void Test_DeleteContactById_Should_ThrowException(int id)
        {
            _mockRepo.Setup(x => x.DeleteContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            var service = GetContactService();

            Assert.Throws<NotFoundRequestExceptionResponse>(() => service.DeleteContactById(id));
            _mockRepo.Verify(x => x.DeleteContactById(id), Times.Once);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Test_DeleteContactById_ShouldNot_ThrowException(int id)
        {
            _mockRepo.Setup(x => x.DeleteContactById(It.IsAny<int>()));
            var service = GetContactService();

            Assert.DoesNotThrow(() => service.DeleteContactById(id));
            _mockRepo.Verify(x => x.DeleteContactById(id), Times.Once);
        }
        #endregion
    }
}
