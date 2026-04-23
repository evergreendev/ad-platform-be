using API.DTOs.Contacts;

namespace API.Services;

public interface IContactService
{
    Task<ContactResponse> CreateContactAsync(CreateContactRequest request);
    Task<IEnumerable<ContactResponse>> GetContactsAsync();
    Task<ContactResponse?> GetContactByIdAsync(Guid id);
}
