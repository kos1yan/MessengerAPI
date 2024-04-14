using Asp.Versioning;
using Messenger.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Contracts;
using Shared.DataTransferObjects.AccountDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/accounts")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1.0")]
    public class AccountsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AccountsController(IServiceManager service) => _service = service;


        /// <summary>
        /// Returns accounts
        /// </summary>
        /// <param name="accountParameters">Request parameters</param>
        /// <response code="200">Returns all accounts</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAccounts([FromQuery] AccountParameters accountParameters)
        {
            var pagedResult = await _service.AccountService.GetAccountsAsync(accountParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.accounts);
        }


        /// <summary>
        /// Returns an account by id
        /// </summary>
        /// <param name="id">The account id</param>
        /// <response code="200">Returns the account</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        [HttpGet("{id:guid}", Name = "AccountById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            var account = await _service.AccountService.GetAccountAsync(id, trackChanges: false);
            return Ok(account);
        }


        /// <summary>
        /// Deletes an account by id
        /// </summary>
        /// <param name="id">The account id</param>
        /// <response code="200">Deletes the account</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            await _service.AccountService.DeleteAccountAsync(id, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Updates an account
        /// </summary>
        /// <param name="id">The account id</param>
        /// <param name="account">The account data for update</param>
        /// <response code="204">Updated successfully</response>
        /// <response code="400">If the request body is null</response>
        /// <response code="404">If the account doesn't exist in the database</response>
        /// <response code="422">If the account data for update is invalid</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] AccountForUpdateDto account)
        {
            await _service.AccountService.UpdateAccountAsync(id, account, trackChanges: true);
            return NoContent();
        }

    }
}
