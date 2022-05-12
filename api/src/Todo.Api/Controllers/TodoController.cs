using Microsoft.AspNetCore.Mvc;
using Todo.Api.Requests;

namespace Todo.Api.Controllers;

[ApiController]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("todo/list")]
    public async Task<IActionResult> List([FromQuery] ListTodoItemsRequest request) =>
        Ok(await _mediator.Send(request));
    
    [HttpPost("todo/create")]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request) =>
        Ok(await _mediator.Send(request));

    [HttpPost("todo/completeItem")]
    public async Task<IActionResult> CompleteItem([FromBody] CompleteTodoItemRequest request)
    {
        var result = await _mediator.Send(request);
        if(result.HasValue)
            return Ok(await _mediator.Send(request));

        return NotFound();
    }
}