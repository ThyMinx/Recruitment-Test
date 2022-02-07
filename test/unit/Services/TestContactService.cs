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
        private Mock<IContactRepository> _mockRepo;
        private ContactService _contactService;
        private List<Contact> _contacts;
        private Contact _contact;

        public ContactService GetContactService()
        {
            _mockRepo = new Mock<IContactRepository>();
            _contactService = new ContactService(_mockRepo.Object);
            return _contactService;
        }

        [SetUp]
        public void SetUp()
        {
            //_contactRepositoryMock = new Mock<IContactRepository>();
            //_contactRepositoryMock.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(_contact);
            //_contactService = new ContactService(_contactRepositoryMock.Object);
            //_contacts = new List<Contact>()
            //{
            //        new Contact{FirstName = "Charly",LastName = "Webster",Title = "Head of Engineering",EmailAddress = "charly.webster@vutu.re",Company = "Vuture"},
            //        new Contact{FirstName = "Simon",LastName = "Humphries",Title = "Engineering Team Lead",EmailAddress = "simon.humphries@vutu.re",Company = "Vuture"},
            //        new Contact{FirstName = "Tufan",LastName = "Unal",Title = "CTO/Founder",EmailAddress = "tufan.unal@vutu.re",Company = "Vuture"},
            //        new Contact{FirstName = "Tom",LastName = "Janofsky",Title = "Group CTO",EmailAddress = "tjanofsky@campaignmonitor.com",Company = "CM Group"}
            //};
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Test_ShouldCall_GetContactById(int id)
        {
            ContactService service = GetContactService();

            var result = service.GetContactById(id);

            //Assert
            //Check get's called once.
            _mockRepo.Verify(m => m.GetContactById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Test_ShouldReceive_Exception404(int id)
        {
            ContactService service = GetContactService();

            var result = service.GetContactById(id);

            //Assert
            var ex = Assert.Throws<NotFoundRequestExceptionResponse>(() => service.GetContactById(id));
            Assert.That(ex.Message, Is.EqualTo("No contact with the id: " + id));
        }

        //[Test]
        //[TestCase(0)]
        //public void GetContactById_FailTest(int id)
        //{
        //    var result = _contactService.GetContactById(id);

        //    Assert.IsNull(result);
        //}

        //[Test]
        //[TestCase(1)]
        //public void GetContactById_PassTest(int id)
        //{
        //    Contact contact = new Contact();

        //    _contactRepositoryMock.Setup(c => c.GetContactById(id)).Returns(contact);

        //    ReadContactDto contactDto = _contactService.GetContactById(id);

        //    Assert.AreSame(contact.FirstName, contactDto.FirstName);
        //    Assert.AreSame(contact.LastName, contactDto.LastName);
        //    Assert.AreSame(contact.EmailAddress, contactDto.EmailAddress);
        //    Assert.AreSame(contact.Title, contactDto.Title);
        //    Assert.AreSame(contact.Company, contactDto.Company);
        //    Assert.AreSame(contact.Status, contactDto.Status);
        //    Assert.AreSame(contact.Id, contactDto.Id);
        //}


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
