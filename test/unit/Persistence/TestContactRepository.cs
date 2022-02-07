using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuture.Persistence;
using Vuture.Persistence.Repositories;

namespace Vuture.Test.Unit.Persistence
{
    public class TestContactRepository
    {
        private readonly ContactDbContext _context;

        public TestContactRepository()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString());
            DbContextOptions<ContactDbContext> options = new DbContextOptions<ContactDbContext>();
            _context = new ContactDbContext(options);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Test1()
        {
            //Arrange
            var contactId = 1;
            _context.Contacts.Add(new Contact { Id = contactId, FirstName = "James", LastName = "Cairns", EmailAddress = "james.cairns@outlook.com", Company = "Vuture", Status = "Working", Title = "Developer" });
            _context.SaveChanges();

            var sut = new ContactRepository(_context);

            //Act
            Contact result = sut.GetContactById(contactId);

            //Assert
            Assert.NotNull(result);
        }
    }
}
