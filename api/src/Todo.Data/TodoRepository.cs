using Microsoft.EntityFrameworkCore;
using Todo.Api.Requests;

namespace Todo.Data;

public class TodoRepository: ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> List(bool retrieveCompletedItems)
    {
        return await _context
            .TodoItems
            .Where(x => retrieveCompletedItems || !x.Completed.HasValue)
            .ToArrayAsync();
    }

    public async Task<Guid> Create(TodoItem newItem)
    {
        await _context.TodoItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }
}