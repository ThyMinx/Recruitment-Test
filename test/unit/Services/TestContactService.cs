using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuture.Models.Dtos;
using Vuture.Persistence.Repositories.Interfaces;
using Vuture.Services;

namespace Vuture.Test.Unit.Services
{
    public class TestContactService
    {
        private Mock<IContactRepository> _contactRepositoryMock;
        private IContactService _contactService;
        private List<Contact> _contacts;
        private Contact _contact;

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactRepositoryMock.Setup(x => x.GetContactById(It.IsAny<int>())).Returns(_contact);
            _contactService = new ContactService(_contactRepositoryMock.Object);
            _contacts = new List<Contact>()
            {
                    new Contact{FirstName = "Charly",LastName = "Webster",Title = "Head of Engineering",EmailAddress = "charly.webster@vutu.re",Company = "Vuture"},
                    new Contact{FirstName = "Simon",LastName = "Humphries",Title = "Engineering Team Lead",EmailAddress = "simon.humphries@vutu.re",Company = "Vuture"},
                    new Contact{FirstName = "Tufan",LastName = "Unal",Title = "CTO/Founder",EmailAddress = "tufan.unal@vutu.re",Company = "Vuture"},
                    new Contact{FirstName = "Tom",LastName = "Janofsky",Title = "Group CTO",EmailAddress = "tjanofsky@campaignmonitor.com",Company = "CM Group"}
            };
        }

        [Test]
        [TestCase(0)]
        public void GetContactById_FailTest(int id)
        {
            var result = _contactService.GetContactById(id);

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(1)]
        public void GetContactById_PassTest(int id)
        {
            Contact contact = new Contact();

            _contactRepositoryMock.Setup(c => c.GetContactById(id)).Returns(contact);

            ReadContactDto contactDto = _contactService.GetContactById(id);

            Assert.AreSame(contact.FirstName, contactDto.FirstName);
            Assert.AreSame(contact.LastName, contactDto.LastName);
            Assert.AreSame(contact.EmailAddress, contactDto.EmailAddress);
            Assert.AreSame(contact.Title, contactDto.Title);
            Assert.AreSame(contact.Company, contactDto.Company);
            Assert.AreSame(contact.Status, contactDto.Status);
            Assert.AreSame(contact.Id, contactDto.Id);
        }
    }
}
