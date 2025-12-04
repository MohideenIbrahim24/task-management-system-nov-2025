using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _auth.ValidateUserAsync(dto.UserName, dto.Password);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        var token = _auth.GenerateJwtToken(user);
        return Ok(new { token, user = new { id = user.Id, username = user.UserName, role = user.Role }});
    }

    // POST: api/auth/register
    [HttpPost("register")]
    [Authorize(Roles = "Admin")] // only Admin can create users
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _auth.CreateUserAsync(dto);
            return Ok(new 
            {
                user.Id,
                user.UserName,
                user.Role
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}