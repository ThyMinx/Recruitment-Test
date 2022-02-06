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
            try
            {
                Contact contact = new Contact()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EmailAddress = dto.EmailAddress,
                    Title = dto.Title,
                    Company = dto.Company,
                    Status = dto.Status
                };
                Contact createdContact = _contactRepository.CreateContact(contact);
                ReadContactDto readContact = new ReadContactDto()
                {
                    Id = createdContact.Id,
                    FirstName = createdContact.FirstName,
                    LastName = createdContact.LastName,
                    EmailAddress = createdContact.EmailAddress,
                    Title = createdContact.Title,
                    Company = createdContact.Company,
                    Status = createdContact.Status
                };
                return readContact;
            }
            catch (Exception ex)
            {

                throw;
            }
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
            try
            {
                _contactRepository.DeleteContactById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}