using Messenger.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.DataTransferObjects.AccountDto;
using Shared.DataTransferObjects.ChatDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Controllers
{
    public class ChatsControllerTests
    {
        private readonly Mock<IServiceManager> _service;
        public ChatsControllerTests()
        {
            _service = new Mock<IServiceManager>();
        }

        [Fact]
        public async Task ChatsController_GetChats_ReturnOk()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var parameters = new Mock<ChatParameters>();
            var trackChanges = false;

            var coll = new Mock<IEnumerable<ExpandoObject>>();
            var metaData = new Mock<MetaData>();
            _service.Setup(c => c.ChatService.GetChatsAsync(parameters.Object, trackChanges)).Returns(Task.FromResult((coll.Object, metaData.Object)));

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

            var result = await controller.GetChats(parameters.Object);
            var okResult = result as ObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ChatsController_GetAccountChats_ReturnOk()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var parameters = new Mock<ChatParameters>();
            var trackChanges = false;
            var guid = Guid.NewGuid();

            var coll = new Mock<IEnumerable<ExpandoObject>>();
            var metaData = new Mock<MetaData>();
            _service.Setup(c => c.ChatService.GetAccountChatsAsync(guid, parameters.Object, trackChanges)).Returns(Task.FromResult((coll.Object, metaData.Object)));

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

            var result = await controller.GetAccountChats(guid, parameters.Object);
            var okResult = result as ObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ChatsController_GetChat_ReturnOk()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var chatDto = new Mock<ChatDto>();
            var trackChanges = false;
            var guid = Guid.NewGuid();
            _service.Setup(c => c.ChatService.GetChatAsync(guid, trackChanges)).Returns(Task.FromResult(chatDto.Object));

            //Act

            var result = await controller.GetChat(guid);
            var okResult = result as ObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ChatsController_CreateChat_ReturnCreatedAtRoute()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var chatDto = new Mock<ChatDto>();
            var trackChanges = false;
            var guid = Guid.NewGuid();
            var chat = new Mock<ChatForCreationDto>();
            _service.Setup(c => c.ChatService.CreateChatAsync(guid, chat.Object, trackChanges)).Returns(Task.FromResult(chatDto.Object));

            //Act

            var result = await controller.CreateChat(guid, chat.Object);
            var сreatedAtRouteResult = result as ObjectResult;

            //Assert

            Assert.NotNull(сreatedAtRouteResult);
            Assert.IsType<CreatedAtRouteResult>(result);
            Assert.IsAssignableFrom<ChatDto>(сreatedAtRouteResult.Value);
        }

        [Fact]
        public async Task ChatsController_AddMemberToChat_ReturnOk()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var chatTrackChanges = true;
            var accountTrackChanges = false;
            var chatGuid = Guid.NewGuid();
            var accountGuid = Guid.NewGuid();
            var chat = new Mock<ChatForCreationDto>();
            _service.Setup(c => c.ChatService.AddMemberToChatAsync(accountGuid, chatGuid, chatTrackChanges, accountTrackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.AddMemberToChat(accountGuid, chatGuid);
            var okResult = result as ObjectResult;

            //Assert

            Assert.Null(okResult);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AccountsController_UpdateChat_NoContentResult()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var chat = new Mock<ChatForUpdateDto>();
            var guid = Guid.NewGuid();
            var chatTrackChanges = true;
            var accountTrackChanges = false;
            _service.Setup(c => c.ChatService.UpdateChatAsync(guid, chat.Object, chatTrackChanges, accountTrackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.UpdateChat(guid, chat.Object);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChatsController_DeleteChat_NoContentResult()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var trackChanges = false;
            var guid = Guid.NewGuid();
            _service.Setup(c => c.ChatService.DeleteChatAsync(guid, trackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.DeleteChat(guid);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChatsController_LeaveChat_NoContentResult()
        {
            //Arrange

            var controller = new ChatsController(_service.Object);
            var trackChanges = false;
            var chatGuid = Guid.NewGuid();
            var accountGuid = Guid.NewGuid();
            _service.Setup(c => c.ChatService.LeaveChatAsync(accountGuid, chatGuid, trackChanges)).Returns(Task.CompletedTask);

            //Act

            var result = await controller.LeaveChat(accountGuid, chatGuid);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }
    }
}
