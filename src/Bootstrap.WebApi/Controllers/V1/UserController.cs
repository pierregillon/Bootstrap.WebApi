using Bootstrap.Application.Users;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain.Users;
using Bootstrap.WebApi.Configuration.Authentication.Bearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers.V1;

[ApiController]
[Route("v1/users")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserController(ICommandDispatcher commandDispatcher, JwtTokenGenerator jwtTokenGenerator)
    {
        _commandDispatcher = commandDispatcher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <summary>
    ///     Register a new user with Email, Phone number and Password.
    /// </summary>
    /// <param name="body"></param>
    /// <returns>Returns a bearer token to access authorized routes</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(JwtTokenJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserBody body)
    {
        var userId = await _commandDispatcher.Dispatch(body.ToCommand());

        var token = _jwtTokenGenerator.Generate(userId, body.Email);

        return Ok(token);
    }
}

public record RegisterUserBody(string Email, string Password)
{
    public RegisterUserCommand ToCommand() => new(
        EmailAddress.Create(Email),
        Domain.Users.Security.Password.Create(Password)
    );
}
