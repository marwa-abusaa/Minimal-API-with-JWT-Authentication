using JwtMinimalApi.Models;
using JwtMinimalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtMinimalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthenticationController(JwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }


    [HttpPost("login")]
    public IActionResult Login(User userRequest)
    {
        var token = _tokenGenerator.GenerateToken(userRequest);

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


    [HttpPost("validate-token")]
    public IActionResult ValidateTokenManually([FromBody] string token, JwtTokenGenerator jwtTokenGenerator)
    {
        var principal = jwtTokenGenerator.ValidateToken(token);
        if (principal == null)
            return Unauthorized("Token is invalid or expired");

        var username = principal.Identity!.Name;
        return Ok($"Token is valid. Hello, {username}!");
    }

}
