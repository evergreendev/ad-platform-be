using API.DTOs;
using API.DTOs.Contacts;

namespace API.Services;

public interface IContactService
{
    Task<ContactResponse> CreateContactAsync(CreateContactRequest request);
    Task<PagedResponse<ContactResponse>> GetContactsAsync(ContactsQuery query);
    Task<ContactResponse?> GetContactByIdAsync(Guid id);
}
