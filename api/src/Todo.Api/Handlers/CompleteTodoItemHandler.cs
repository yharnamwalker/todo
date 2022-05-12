using Todo.Api.Requests;

namespace Todo.Api.Handlers;

public class CompleteTodoItemHandler : IRequestHandler<CompleteTodoItemRequest, DateTime?>
{
    private readonly ITodoRepository _todoRepository;

    public CompleteTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<DateTime?> Handle(CompleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var item = await _todoRepository.GetTodoItemByIdAsync(request.Id);
        if (item == null)
            return null;

        if (!item.Completed.HasValue)
        {
            item.Completed = DateTime.UtcNow;
            await _todoRepository.UpdateAsync(item);
        }

        return item.Completed;
    }
}