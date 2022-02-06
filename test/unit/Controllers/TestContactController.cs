using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Vuture.Controllers;
using Vuture.Persistence.Repositories.Interfaces;
using Vuture.Services;

namespace Vuture.Test.Unit.Controllers
{
    [TestFixture]
    [Category("Unit")]
    public class TestContactController
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock = new Mock<IContactRepository>();
        private readonly Mock<IContactService> _contactServiceMock = new Mock<IContactService>();
        private ContactController _contactController;
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

        [SetUp]
        public void SetUp()
        {
            _contactController = new ContactController(_contactServiceMock.Object);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void GetContactById_FailTest(int id)
        {
            var result = _contactController.GetContactById(id);

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GetContactById_PassTest(int id)
        {
            var result = _contactController.GetContactById(id);
            var comparison = new JsonResult(_testContacts.Where(x => x.Id == id).FirstOrDefault());

            if (result.Equals(comparison))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// This test should not be able to delete an item and it should get a 404 Not Found.
        /// </summary>
        /// <param name="id">This is the id of a contact that is passed in by [TestCase()]</param>
        public void DeleteContactById_FailTest(int id)
        {

        }

        /// <summary>
        /// This test should be able to delete an item and it should get a 200 ok.
        /// </summary>
        /// <param name="id">This is the id of a contact that is passed in by [TestCase()]</param>
        public void DeleteContactById_PassTest(int id)
        {

        }
    }
}
