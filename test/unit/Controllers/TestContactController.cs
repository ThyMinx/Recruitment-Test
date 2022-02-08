using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Vuture.Controllers;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Models.Dtos;
using Vuture.Persistence.Repositories.Interfaces;
using Vuture.Services;

namespace Vuture.Test.Unit.Controllers
{
    [TestFixture]
    [Category("Unit")]
    public class TestContactController
    {
        private Mock<IContactService> _mockService = new Mock<IContactService>();
        private ContactController _controller;

        public ContactController GetContactController()
        {
            _controller = new ContactController(_mockService.Object);
            return _controller;
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
            var createdDbContact = new ReadContactDto()
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

            _mockService.Setup(x => x.CreateContact(It.IsAny<CreateContactDto>())).Returns(createdDbContact);

            var controller = GetContactController();
            var result = controller.CreateContact(dtoPassedIn).Value;
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
        [TestCase("", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase("Fawn", "", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "", "Bank", "Holiday", "Accounts")]
        [TestCase(null, "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase("Fawn", null, "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", null, "Bank", "Holiday", "Accounts")]
        public void Test_CreateContact_Should_ThrowException(string? firstName, string? lastName, string? email, string company, string status, string title)
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

            var controller = GetContactController();
            var result = controller.CreateContact(dtoPassedIn).Result as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new BadRequestExceptionResponse("").StatusCode);
            _mockService.Verify(x => x.CreateContact(dtoPassedIn), Times.Never);
        }
        [Test]
        [TestCase("James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase("Fawn", "Tootington", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase("Ian", "Tufft", "", "Bank", "Holiday", "Accounts")]
        public void Test_CreateContact_Should_Throw400Exception_DuplicateEmail(string? firstName, string? lastName, string? email, string company, string status, string title)
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

            var controller = GetContactController();
            _mockService.Setup(m => m.CreateContact(It.IsAny<CreateContactDto>())).Throws(new BadRequestExceptionResponse("There is already a contact with the email: " + email));
            var result = controller.CreateContact(dtoPassedIn).Result as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new BadRequestExceptionResponse("").StatusCode);
        }
        #endregion

        #region Tests for Read
        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Massey", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "some@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_GetContactById_ShouldReceive_ReadContactDto(int id, string firstName, string lastName, string email, string company, string status, string title)
        {
            var contact = new ReadContactDto()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Company = company,
                Status = status,
                Title = title
            };
            _mockService.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(contact);
            var controller = GetContactController();

            var result = controller.GetContactById(id) as JsonResult;
            var resultObj = result.Value as ReadContactDto;

            Assert.IsNotNull(resultObj);
            Assert.AreEqual(resultObj.FirstName, contact.FirstName);
            Assert.AreEqual(resultObj.LastName, contact.LastName);
            Assert.AreEqual(resultObj.EmailAddress, contact.EmailAddress);
            Assert.AreEqual(resultObj.Company, contact.Company);
            Assert.AreEqual(resultObj.Status, contact.Status);
            Assert.AreEqual(resultObj.Title, contact.Title);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        public void Test_GetContactById_Should_ThrowException(int id)
        {
            _mockService.Setup(x => x.GetContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            var controller = GetContactController();

            var result = controller.GetContactById(id) as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new NotFoundRequestExceptionResponse("").StatusCode);
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

            var updatedDbContact = new ReadContactDto()
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

            _mockService.Setup(x => x.UpdateContactById(It.IsAny<int>(), It.IsAny<UpdateContactDto>())).Returns(updatedDbContact);

            var controller = GetContactController();
            var result = controller.UpdateContactById(id, dtoPassedIn).Result as JsonResult;
            var resultObj = result.Value as ReadContactDto;
            Assert.IsTrue(result.Value.GetType().Equals(typeof(ReadContactDto)));
            Assert.AreEqual(expectedResult.Id, resultObj.Id);
            Assert.AreEqual(expectedResult.FirstName, resultObj.FirstName);
            Assert.AreEqual(expectedResult.LastName, resultObj.LastName);
            Assert.AreEqual(expectedResult.EmailAddress, resultObj.EmailAddress);
            Assert.AreEqual(expectedResult.Company, resultObj.Company);
            Assert.AreEqual(expectedResult.Status, resultObj.Status);
            Assert.AreEqual(expectedResult.Title, resultObj.Title);
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

            _mockService.Setup(x => x.UpdateContactById(It.IsAny<int>(), It.IsAny<UpdateContactDto>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id)); 

            var controller = GetContactController();
            var result = controller.UpdateContactById(id, dtoPassedIn).Result as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new NotFoundRequestExceptionResponse("").StatusCode);
        }

        [Test]
        [TestCase(1, "", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "", "Bank", "Holiday", "Accounts")]
        [TestCase(1, null, "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", null, "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", null, "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_Throw400Exception(int id, string? firstName, string? lastName, string? email, string company, string status, string title)
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

            _mockService.Setup(x => x.UpdateContactById(It.IsAny<int>(), It.IsAny<UpdateContactDto>()));

            var controller = GetContactController();
            var result = controller.UpdateContactById(id, dtoPassedIn).Result as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new BadRequestExceptionResponse("").StatusCode);
        }

        [Test]
        [TestCase(1, "James", "Cairns", "cairns.james@email.com", "Vuture", "Holiday", "Lead")]
        [TestCase(3, "Fawn", "Beep", "mrs@email.com", "Vuture", "Working", "Lead")]
        [TestCase(4, "Ian", "Tufft", "email@email.com", "Bank", "Holiday", "Accounts")]
        public void Test_UpdateContact_Should_Throw400Exception_DuplicateEmail(int id, string? firstName, string? lastName, string? email, string company, string status, string title)
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

            _mockService.Setup(x => x.UpdateContactById(It.IsAny<int>(), It.IsAny<UpdateContactDto>())).Throws(new BadRequestExceptionResponse("There is already a contact with the email: " + email));

            var controller = GetContactController();
            var result = controller.UpdateContactById(id, dtoPassedIn).Result as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new BadRequestExceptionResponse("").StatusCode);
        }
        #endregion

        #region Tests for Delete
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        public void Test_DeleteContactById_Should_ThrowException(int id)
        {
            _mockService.Setup(x => x.DeleteContactById(It.IsAny<int>())).Throws(new NotFoundRequestExceptionResponse("No contact with the id: " + id));
            var controller = GetContactController();

            //Assert.Throws<NotFoundRequestExceptionResponse>(() => controller.DeleteContactById(id));
            var result = controller.DeleteContactById(id) as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, new NotFoundRequestExceptionResponse("").StatusCode);
            _mockService.Verify(x => x.DeleteContactById(id), Times.Once);
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
            _mockService.Setup(x => x.DeleteContactById(It.IsAny<int>()));
            var controller = GetContactController();

            var result = controller.DeleteContactById(id) as StatusCodeResult;
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            _mockService.Verify(x => x.DeleteContactById(id), Times.Once);
        }
        #endregion
    }
}
