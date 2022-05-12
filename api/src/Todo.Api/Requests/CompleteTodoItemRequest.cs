namespace Todo.Api.Requests;

public class CompleteTodoItemRequest : IRequest<DateTime?>
{
    public Guid Id { get; set; }
}