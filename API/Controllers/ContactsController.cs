using API.DTOs;
using API.DTOs.Contacts;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ContactResponse>> CreateContact(CreateContactRequest request)
    {
        var createdContact = await contactService.CreateContactAsync(request);
        return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<ContactResponse>>> GetContacts([FromQuery] ContactsQuery query)
    {
        var contacts = await contactService.GetContactsAsync(query);
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactResponse>> GetContactById(Guid id)
    {
        var contact = await contactService.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        return Ok(contact);
    }
}
