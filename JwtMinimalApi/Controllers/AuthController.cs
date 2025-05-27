using JwtMinimalApi.Models;
using JwtMinimalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtMinimalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthController(JwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }


    [HttpPost("login")]
    public IActionResult Login(User requestUser)
    {
        var token = _tokenGenerator.GenerateToken(requestUser);

        if (token == null)
            return Unauthorized("Invalid username or password");

        return Ok(new { token });
    }


    [Authorize]
    [HttpGet("welcome")]
    public IActionResult GetWelcomeMessage()
    {
        return Ok($"Welcome!!!");
    }
}
