using Moq;
using Todo.Api.Handlers;
using Todo.Api.Requests;
using Todo.Data;
using Todo.Data.Models;

namespace Todo.Api.Tests.Handlers
{
    [TestClass]
    public class CompleteTodoItemHandlerTests
    {
        private readonly Mock<ITodoRepository> _repository;
        private readonly CompleteTodoItemHandler _handler;

        public CompleteTodoItemHandlerTests()
        {
            _repository = new Mock<ITodoRepository>();
            _handler = new CompleteTodoItemHandler(_repository.Object);
        }

        [TestMethod]
        public async Task ReturnsNewDateTime_WhenItemExistsAndIsNotCompleted()
        {
            // Arrange
            var incompleteItem = GetTodoItem(false);
            var incompleteItemCompletedDate = incompleteItem?.Completed;
            _repository.Setup(x => x.GetTodoItemByIdAsync(It.IsAny<Guid>())).ReturnsAsync(incompleteItem);

            // Act
            var result = await _handler.Handle(new CompleteTodoItemRequest(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(incompleteItem);
            Assert.IsNull(incompleteItemCompletedDate);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ReturnsCompletedDate_WhenItemExistsAndIsAlreadyCompleted()
        {
            // Arrange
            var completedItem = GetTodoItem(true);
            var completedItemCompletedDate = completedItem?.Completed;
            _repository.Setup(x => x.GetTodoItemByIdAsync(It.IsAny<Guid>())).ReturnsAsync(completedItem);

            // Act
            var result = await _handler.Handle(new CompleteTodoItemRequest(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(completedItem);
            Assert.IsNotNull(result);            
            Assert.AreEqual(completedItemCompletedDate, result);
        }

        [TestMethod]
        public async Task ReturnsNull_WhenItemDoesNotExist()
        {
            // Arrange
            _repository.Setup(x => x.GetTodoItemByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as TodoItem);

            // Act
            var result = await _handler.Handle(new CompleteTodoItemRequest(), CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }

        private static TodoItem GetTodoItem(bool completedItem)
        {
            return new TodoItem()
            {
                Id = Guid.NewGuid(),
                Text = $"Random Item",
                Created = DateTime.UtcNow,
                Completed = completedItem ? DateTime.UtcNow : null
            };
        }
    }
}
