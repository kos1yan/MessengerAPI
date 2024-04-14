using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.ContactDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.Design;
using Asp.Versioning;

namespace Messenger.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/accounts/{accountId}/contacts")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class ContactsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public ContactsController(IServiceManager service) => _service = service;

        /// <summary>
        /// Returns contacs of an account
        /// </summary>
        /// <param name="contactParameters">Request parameters</param>
        /// <param name="accountId">The account id</param>
        /// <response code="200">Returns all contacs of the account</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAccountContacts(Guid accountId, [FromQuery] ContactParameters contactParameters)
        {
            var pagedResult = await _service.AccountService.GetAccountContactsAsync(accountId, contactParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.contacts);
        }

        /// <summary>
        /// Creates a contact for an account 
        /// </summary>
        /// <param name="contact">The contact data for creation</param>
        /// <param name="accountId">The account id</param>
        /// <response code="201">Successful creation</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        /// <response code="422">If the contact data for creation is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateAccountContact(Guid accountId, [FromBody] ContactForCreationDto contact)
        {
            await _service.AccountService.CreateAccountContactAsync(accountId, contact, trackChanges: false);
            return Created();
            
        }

        /// <summary>
        /// Deletes a contact of an account 
        /// </summary>
        /// <param name="id">The contact id</param>
        /// <param name="accountId">The account id</param>
        /// <response code="204">Successful removal</response>
        /// <response code="404">If the account doesn't exist in the database or if the contact doesn't exist in the database</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccountContact(Guid accountId, Guid id)
        {
            await _service.AccountService.DeleteAccountContactAsync(accountId, id, trackChanges: false);
            return NoContent();
        }
    }
}
