using Todo.Api.Requests;

namespace Todo.Api.Handlers;

public class ListTodoItemsHandler : IRequestHandler<ListTodoItemsRequest, IEnumerable<TodoItem>>
{
    private readonly ITodoRepository _todoRepository;

    public ListTodoItemsHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoItem>> Handle(ListTodoItemsRequest request, CancellationToken cancellationToken)
    {
        var result =  await _todoRepository.List(request.ShowCompletedItems);
        return result.OrderByDescending(x => x.Created);
    }
}