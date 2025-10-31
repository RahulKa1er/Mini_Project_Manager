
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;
    public AuthController(AppDbContext db, TokenService tokenService) { _db = db; _tokenService = tokenService; }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username)) return BadRequest("Username exists");
        using var hmac = new HMACSHA256();
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)))
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(new { token = _tokenService.CreateToken(user) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null) return Unauthorized("Invalid credentials");
        using var hmac = new HMACSHA256();
        var computed = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));
        if (computed != user.PasswordHash) return Unauthorized("Invalid credentials");
        return Ok(new { token = _tokenService.CreateToken(user) });
    }
}
