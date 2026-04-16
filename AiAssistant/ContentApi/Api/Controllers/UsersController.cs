using ContentApi.DTO;
using ContentApi.Features.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public  class UsersController(IUserQueries queries,ISender sender, ILogger<UsersController> logger) : ControllerBase
{
    
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUserQueries _queries = queries;
    private readonly ISender _sender = sender;



    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetById(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user {UserId}", id);

        var result = await _queries.GetUserByIdAsync(id, ct);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Create(
        [FromBody] UserRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Creating user {Username}", request.Username);

        var command = new CreateUserCommand(request.Username, request.Password);

        var result = await _sender.Send(command, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },  
            result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> Search(
        [FromQuery] string term,
        CancellationToken ct)
    {
        _logger.LogInformation("Searching users with term {SearchTerm}", term);

        var result = await _queries.SearchUsersAsync(term, ct);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Update(Guid id,[FromBody] UpdateUserRequest request,CancellationToken ct)
    {
        _logger.LogInformation("Updating user with the id:  {UserId}", id);
        var command = new UpdateUserCommand(id, request.Username, request.Password);
        var result = await _sender.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Deleting user with the id: {UserId}", id);
        var command = new DeleteUserCommand(id);
        await _sender.Send(command, ct);
        return NoContent();

    }

}

