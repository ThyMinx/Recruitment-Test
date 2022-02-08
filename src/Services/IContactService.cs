using Vuture.Models.Dtos;

namespace Vuture.Services
{
    public interface IContactService
    {
        ReadContactDto GetContactById(int id);
        List<ReadContactDto> GetContactsByCompany(string company);
        ReadContactDto UpdateContactById(int id, UpdateContactDto dto);
        ReadContactDto CreateContact(CreateContactDto dto);
        void DeleteContactById(int id);
    }
}