using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Vuture.Controllers;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Persistence.Repositories.Interfaces;
using Vuture.Services;

namespace Vuture.Test.Unit.Controllers
{
    [TestFixture]
    [Category("Unit")]
    public class TestContactController
    {
        private readonly Mock<IContactRepository> _mockRepo;
        private Mock<IContactService> _mockService;
        private static List<Contact> _testContacts = new List<Contact>()
        {
            new Contact
            {
                FirstName = "Charly",
                LastName = "Webster",
                Title = "Head of Engineering",
                EmailAddress = "charly.webster@vutu.re",
                Company = "Vuture"
            },
            new Contact
            {
                FirstName = "Simon",
                LastName = "Humphries",
                Title = "Engineering Team Lead",
                EmailAddress = "simon.humphries@vutu.re",
                Company = "Vuture"
            },
            new Contact
            {
                FirstName = "Tufan",
                LastName = "Unal",
                Title = "CTO/Founder",
                EmailAddress = "tufan.unal@vutu.re",
                Company = "Vuture"
            },
            new Contact
            {
                FirstName = "Tom",
                LastName = "Janofsky",
                Title = "Group CTO",
                EmailAddress = "tjanofsky@campaignmonitor.com",
                Company = "CM Group"
            }
        };

        public ContactController GetController()
        {
            _mockService = new Mock<IContactService>();
            var contactController = new ContactController(_mockService.Object);
            return contactController;
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Test_ShouldCall_GetContactById(int id)
        {
            ContactController controller = GetController();

            var result = controller.GetContactById(id);

            //Assert
            //Check get's called once.
            _mockService.Verify(m => m.GetContactById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Test_ShouldReceive_Exception404(int id)
        {
            ContactController controller = GetController();

            var result = controller.GetContactById(id);

            //Assert
            Assert.That(() => controller.GetContactById(id), Throws.TypeOf<NotFoundRequestExceptionResponse>());
        }

        //[Test]
        //[TestCase(-1)]
        //[TestCase(0)]
        //public void GetContactById_FailTest(int id)
        //{
        //    var result = _contactController.GetContactById(id);

        //    Assert.IsNull(result);
        //}

        //[Test]
        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(3)]
        //[TestCase(4)]
        //public void GetContactById_PassTest(int id)
        //{
        //    var result = _contactController.GetContactById(id);
        //    var comparison = new JsonResult(_testContacts.Where(x => x.Id == id).FirstOrDefault());

        //    if (result.Equals(comparison))
        //    {
        //        Assert.Pass();
        //    }
        //    else
        //    {
        //        Assert.Fail();
        //    }
        //}

        ///// <summary>
        ///// This test should not be able to delete an item and it should get a 404 Not Found.
        ///// </summary>
        ///// <param name="id">This is the id of a contact that is passed in by [TestCase()]</param>
        //[Test]
        //[TestCase(-1)]
        //public void DeleteContactById_FailTest(int id)
        //{
        //    var result = (StatusCodeResult)_contactController.DeleteContactById(id);

        //    if (result.StatusCode.Equals(404))
        //        Assert.Pass();
        //    else
        //        Assert.Fail();
        //}

        ///// <summary>
        ///// This test should be able to delete an item and it should get a 200 ok.
        ///// </summary>
        ///// <param name="id">This is the id of a contact that is passed in by [TestCase()]</param>
        //public void DeleteContactById_PassTest(int id)
        //{

        //}
    }
}
