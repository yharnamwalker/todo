using Todo.Data.Models;

namespace Todo.Data;

public interface ITodoRepository
{
    /// <summary>
    /// Returns a todoitem by the provided id
    /// </summary>
    /// <param name="id">The id of the TodoItem to retrieve</param>
    /// <returns>A TodoItem, if one is found</returns>
    Task<TodoItem> GetTodoItemByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a listing of TodoItems
    /// </summary>
    /// <param name="retrieveCompletedItems">Whether to retrieve completed items as part of the lookup</param>
    /// <returns>A collection of TodoItem</returns>
    Task<IEnumerable<TodoItem>> ListAsync(bool retrieveCompletedItems);
    
    /// <summary>
    /// Creates a new TodoItem
    /// </summary>
    /// <param name="newItem">The TodoItem to create</param>
    /// <returns>The Id of the added TodoItem created</returns>
    Task<Guid> CreateAsync(TodoItem newItem);

    /// <summary>
    /// Updates an existing TodoItem
    /// </summary>
    /// <param name="item">The TodoItem to update</param>
    /// <returns></returns>
    Task UpdateAsync(TodoItem item);
}