using Todo.Api.Requests;

namespace Todo.Data;

public interface ITodoRepository
{
    /// <summary>
    /// Retrieves a listing of TodoItems
    /// </summary>
    /// <param name="retrieveCompletedItems">Whether to retrieve completed items as part of the lookup</param>
    /// <returns>A collection of TodoItem</returns>
    Task<IEnumerable<TodoItem>> List(bool retrieveCompletedItems);
    
    /// <summary>
    /// Creates a new TodoItem
    /// </summary>
    /// <param name="newItem">The TodoItem to create</param>
    /// <returns>The Id of the added TodoItem created</returns>
    Task<Guid> Create(TodoItem newItem);
}