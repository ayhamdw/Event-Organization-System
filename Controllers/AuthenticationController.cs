using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.controller;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly  IAuthServices _authServices;

    public AuthenticationController(IAuthServices authServices)
    {
        _authServices = authServices;
    }
    
    [HttpPost("login")]
    public async Task <IActionResult> Login([FromBody] LoginViewModel model)
    {
        var response = await _authServices.Login(model);
        return Ok(GeneralApiResponse<object>.Success( response, "Uses Successfully Logged In" ));
        
    }
    
    [HttpPost("register")]
    public async Task <IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var response = await _authServices.Register(model);
        return Created("" , GeneralApiResponse<object>.Success(response, "User Successfully Registered" , 201));
    }
}