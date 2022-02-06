using Vuture.Models.Dtos;
using Vuture.Persistence.Repositories.Interfaces;

namespace Vuture.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ReadContactDto CreateContact(CreateContactDto dto)
        {
            throw new NotImplementedException();
        }

        public ReadContactDto UpdateContactById(int id, UpdateContactDto dto)
        {
            throw new NotImplementedException();
        }

        public ReadContactDto GetContactById(int id)
        {
            try
            {
                Contact contact = _contactRepository.GetContactById(id);
                if (contact != null)
                {
                    ReadContactDto dto = new ReadContactDto()
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        EmailAddress = contact.EmailAddress,
                        Title = contact.Title,
                        Company = contact.Company,
                        Status = contact.Status
                    };
                    return dto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //throw new NotImplementedException();
        }

        public void DeleteContactById(int id)
        {
            throw new NotImplementedException();
        }
    }
}