using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// Handles authentication requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private const string DefaultUsername = "admin";
    private const string DefaultPassword = "admin";

    /// <summary>
    /// Signs in with the default administrator account.
    /// </summary>
    /// <param name="request">Login account and password.</param>
    /// <returns>The login result.</returns>
    /// <response code="200">Login succeeded.</response>
    /// <response code="401">The account or password is invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (request.Username.Trim() != DefaultUsername || request.Password != DefaultPassword)
        {
            return Unauthorized(new LoginResponse(false, string.Empty, "帳號或密碼錯誤"));
        }

        return Ok(new LoginResponse(true, DefaultUsername, "登入成功"));
    }
}

/// <summary>
/// Login request payload.
/// </summary>
/// <param name="Username">The account name.</param>
/// <param name="Password">The account password.</param>
public record LoginRequest(string Username, string Password);

/// <summary>
/// Login response payload.
/// </summary>
/// <param name="Success">Whether login succeeded.</param>
/// <param name="Username">The signed-in user name.</param>
/// <param name="Message">A human-readable result message.</param>
public record LoginResponse(bool Success, string Username, string Message);
