using Todo.Data.Models;

namespace Todo.Api.Requests;

public class ListTodoItemsRequest : IRequest<IEnumerable<TodoItem>>
{
    public bool ShowCompletedItems { get; set; }
}