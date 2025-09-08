using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses.App.API.Data;
using Expenses.App.API.Dtos.auth;
using Expenses.App.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Expenses.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, PasswordHasher<User> passwordHasher, IConfiguration config)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _config = config;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] PostRegisterUserDto payload)
    {
        if (await _context.Users.AnyAsync(n => n.Email == payload.Email))
        {
            return BadRequest(new { message = "The Email is already taken" });
        }

        var hashPassword = _passwordHasher.HashPassword(new User(), payload.Password);

        var newUser = new User()
        {
            Name = payload.Name,
            Email = payload.Email,
            Password = hashPassword, // hashed password
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(newUser);

        return Ok(new { Token = token, message = "Register Berhasil" });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] PostLoginUserDto payload)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid Credential" });
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, payload.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid Credential" });
        }

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token, message = "login Berhasil" });
    }

    // jwt token ------------------------------------

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // pull secret key from appsettings.json

        var secretKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
