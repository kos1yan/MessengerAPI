using Messenger.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Service.Contracts;
using Shared.DataTransferObjects.AccountDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IServiceManager> _service;
        public AccountsControllerTests()
        {
            _service = new Mock<IServiceManager>();
        }

        [Fact]
        public async Task AccountsController_GetAccounts_ReturnOk()
        {
            //Arrange

            var controller = new AccountsController(_service.Object);
            var parameters = new Mock<AccountParameters>();
            var trackChanges = false;

            var coll = new Mock<IEnumerable<ExpandoObject>>();
            var metaData = new Mock<MetaData>();
            _service.Setup(c => c.AccountService.GetAccountsAsync(parameters.Object, trackChanges)).Returns(Task.FromResult((coll.Object, metaData.Object)));

            var mockResponse = new Mock<HttpResponse>();
            mockResponse.SetupAllProperties();
            mockResponse.Setup(r => r.Headers).Returns(new HeaderDictionary());
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            //Act

            var result = await controller.GetAccounts(parameters.Object);
            var okResult = result as ObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AccountsController_GetAccount_ReturnOk()
        {
            //Arrange

            var controller = new AccountsController(_service.Object);
            var accountDto = new Mock<AccountDto>();
            var guid = Guid.NewGuid();
            var trackChanges = false;
            _service.Setup(c => c.AccountService.GetAccountAsync(guid, trackChanges)).Returns(Task.FromResult(accountDto.Object));

            //Act

            var result = await controller.GetAccount(guid);
            var okResult = result as ObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AccountsController_DeleteAccount_NoContentResult()
        {
            //Arrange

            var controller = new AccountsController(_service.Object);
            var guid = Guid.NewGuid();
            var trackChanges = false;
            _service.Setup(c => c.AccountService.DeleteAccountAsync(guid, trackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.DeleteAccount(guid);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AccountsController_UpdateAccount_NoContentResult()
        {
            //Arrange

            var controller = new AccountsController(_service.Object);
            var account = new Mock<AccountForUpdateDto>();
            var guid = Guid.NewGuid();
            var trackChanges = true;
            _service.Setup(c => c.AccountService.UpdateAccountAsync(guid, account.Object, trackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.UpdateAccount(guid, account.Object);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }
    }
}
