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

    public async Task<TodoItem> GetTodoItemByIdAsync(Guid id)
    {
        return await _context.TodoItems.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<IEnumerable<TodoItem>> ListAsync(bool retrieveCompletedItems)
    {
        return await _context
            .TodoItems
            .Where(x => retrieveCompletedItems || !x.Completed.HasValue)
            .ToArrayAsync();
    }

    public async Task<Guid> CreateAsync(TodoItem newItem)
    {
        await _context.TodoItems.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id;
    }

    public async Task UpdateAsync(TodoItem item)
    {
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }
}