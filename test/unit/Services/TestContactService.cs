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
        #endregion

        #region Tests for Read
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

            var expectedResult = new ReadContactDto()
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
            Assert.AreEqual(expectedResult.Id, result.Id);
            Assert.AreEqual(expectedResult.FirstName, result.FirstName);
            Assert.AreEqual(expectedResult.LastName, result.LastName);
            Assert.AreEqual(expectedResult.EmailAddress, result.EmailAddress);
            Assert.AreEqual(expectedResult.Company, result.Company);
            Assert.AreEqual(expectedResult.Status, result.Status);
            Assert.AreEqual(expectedResult.Title, result.Title);
        }
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_ThrowException(int id, string firstName, string lastName, string email, string company, string status, string title)
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

            var expectedResult = new ReadContactDto()
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

            _mockRepo.Setup(x => x.GetContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            _mockRepo.Setup(x => x.UpdateContact(It.IsAny<Contact>()));

            var service = GetContactService();
            Assert.Throws<NotFoundRequestExceptionResponse>(() => service.UpdateContactById(id, dtoPassedIn));
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
