using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.Api.Controllers;
using Todo.Api.Requests;

namespace Todo.Api.Tests.Controllers
{
    [TestClass]
    public class TodoControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new TodoController(_mediator.Object);
        }

        [TestMethod]
        public async Task CompleteItem_ReturnsOkResultWithCompletionDate_WhenItemExistsWithoutCompletionDate()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<CompleteTodoItemRequest>(), CancellationToken.None)).ReturnsAsync(DateTime.UtcNow);

            // Act
            var result = await _controller.CompleteItem(new CompleteTodoItemRequest());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var responseContent = ((OkObjectResult)result).Value;
            Assert.IsInstanceOfType(responseContent, typeof(DateTime?));
            Assert.IsNotNull((DateTime?)responseContent);           
        }

        [TestMethod]
        public async Task CompleteItem_ReturnsNotFoundResult_WhenItemDoesNotExist()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<CompleteTodoItemRequest>(), CancellationToken.None)).ReturnsAsync(null as DateTime?);

            // Act
            var result = await _controller.CompleteItem(new CompleteTodoItemRequest());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
