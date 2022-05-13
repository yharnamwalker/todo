using Microsoft.EntityFrameworkCore;
using Todo.Api.Handlers;
using Todo.Api.Requests;
using Todo.Data;
using Todo.Data.Models;

namespace Todo.Api.Tests.Handlers
{
    [TestClass]
    public class ListTodoItemsHandlerTests
    {
        [TestMethod]
        public async Task ReturnsListWithCompletedItems()
        {
            // Arrange
            var mockData = GetTestTodoItems(10, 10);
            var context = await GetDbContextForTests(nameof(ReturnsListWithCompletedItems), mockData);
            var _handler = new ListTodoItemsHandler(new TodoRepository(context));

            // Act
            var result = await _handler.Handle(new ListTodoItemsRequest() { ShowCompletedItems = true }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(mockData);
            Assert.IsTrue(mockData.Any(x => x.Completed.HasValue) && mockData.Any(x => !x.Completed.HasValue));

            var resultIds = result.Select(x => x.Id);
            var mockDataIds = mockData.Select(x => x.Id);
            Assert.IsTrue(mockDataIds.All(resultIds.Contains));
        }        

        [TestMethod]
        public async Task ReturnsListWithoutCompletedItems()
        {
            // Arrange
            var mockData = GetTestTodoItems(10, 10);
            var context = await GetDbContextForTests(nameof(ReturnsListWithCompletedItems), mockData);
            var _handler = new ListTodoItemsHandler(new TodoRepository(context));

            // Act
            var result = await _handler.Handle(new ListTodoItemsRequest() { ShowCompletedItems = false }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(mockData);
            Assert.IsTrue(mockData.Any(x => x.Completed.HasValue) && mockData.Any(x => !x.Completed.HasValue));

            var resultIds = result.Select(x => x.Id);
            var mockDataIds = mockData.Where(x => !x.Completed.HasValue).Select(x => x.Id);
            Assert.IsTrue(mockDataIds.All(resultIds.Contains));
        }

        [TestMethod]
        public async Task ReturnsListOrderedByCreatedDescending()
        {
            // Arrange
            var mockData = GetTestTodoItems(10, 10);
            var context = await GetDbContextForTests(nameof(ReturnsListWithCompletedItems), mockData);
            var _handler = new ListTodoItemsHandler(new TodoRepository(context));

            // Act
            var result = await _handler.Handle(new ListTodoItemsRequest() { ShowCompletedItems = true }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(mockData);
            Assert.IsTrue(mockData.Any(x => x.Completed.HasValue) && mockData.Any(x => !x.Completed.HasValue));

            var resultIds = result.Select(x => x.Id);
            var mockDataIds = mockData.OrderByDescending(x => x.Created).Select(x => x.Id);
            Assert.IsTrue(mockDataIds.SequenceEqual(resultIds));
        }

        private static IEnumerable<TodoItem> GetTestTodoItems(int incompleteItems, int completedItems)
        {
            static IEnumerable<TodoItem> CreateItems(int amount, bool completed)
            {
                var itemTextDescriptor = completed ? "Completed" : "Incomplete";
                var items = new List<TodoItem>();
                for (int x = 1; x <= amount; x++)
                {
                    items.Add(new TodoItem()
                    {
                        Id = Guid.NewGuid(),
                        Text = $"Random {itemTextDescriptor} Item {x}",
                        Created = DateTime.UtcNow,
                        Completed = completed ? DateTime.UtcNow : null
                    });
                }
                return items;
            }

            var result = new List<TodoItem>();
            result.AddRange(CreateItems(incompleteItems, false));
            result.AddRange(CreateItems(completedItems, true));

            return result;
        }

        private static async Task<TodoContext> GetDbContextForTests(string dbName, IEnumerable<TodoItem> todoItemsSeedData)
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // seed data
            using (var context = new TodoContext(options))
            {
                context.TodoItems.AddRange(todoItemsSeedData);
                await context.SaveChangesAsync();
            }

            return new TodoContext(options);
        }
    }
}
