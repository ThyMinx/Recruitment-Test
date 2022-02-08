using Vuture.Exceptions.ExceptionResponses;
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
                ReadContactDto readContact = ContactToReadContactDto(createdContact);
                return readContact;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ReadContactDto UpdateContactById(int id, UpdateContactDto dto)
        {
            try
            {
                ReadContactDto readContactDto = GetContactById(id);
                Contact contact = new Contact()
                {
                    Id = id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    EmailAddress = dto.EmailAddress,
                    Title = dto.Title,
                    Company = dto.Company,
                    Status = dto.Status
                };
                Contact updatedContact = _contactRepository.UpdateContact(contact);
                ReadContactDto readContact = ContactToReadContactDto(updatedContact);
                if (readContact == null)
                {
                    throw new NotFoundRequestExceptionResponse("No contact with the id: " + id);
                }
                return readContact;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReadContactDto GetContactById(int id)
        {
            try
            {
                Contact contact = _contactRepository.GetContactById(id);
                if (contact != null)
                {
                    ReadContactDto dto = ContactToReadContactDto(contact);
                    return dto;
                }
                else
                {
                    throw new NotFoundRequestExceptionResponse("No contact with the id: " + id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //throw new NotImplementedException();
        }

        public List<ReadContactDto> GetContactsByCompany(string company)
        {
            try
            {
                List<ReadContactDto> contactDtos = new List<ReadContactDto>();
                List<Contact> contacts = _contactRepository.GetContactsByCompany(company);
                contacts.ForEach(c => contactDtos.Add(ContactToReadContactDto(c)));

                return contactDtos;
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

        public ReadContactDto ContactToReadContactDto(Contact contact)
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
    }
}