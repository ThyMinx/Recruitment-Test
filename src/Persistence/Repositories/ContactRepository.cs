using Vuture.Exceptions.ExceptionResponses;
using Vuture.Persistence.Repositories.Interfaces;

namespace Vuture.Persistence.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }

        void IContactRepository.SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Contact GetContactById(int Id)
        {
            try
            {
                Contact contact = _context.Contacts.Where(c => c.Id == Id).FirstOrDefault();
                if (contact == null)
                {
                    throw new NotFoundRequestExceptionResponse("No contact with the id: " + Id);
                }
                return contact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //throw new NotImplementedException();
        }

        public void DeleteContactById(int Id)
        {
            throw new NotImplementedException();
        }

        public Contact CreateContact(Contact Contact)
        {
            throw new NotImplementedException();
        }

        public Contact UpdateContact(Contact Contact)
        {
            throw new NotImplementedException();
        }
    }
}