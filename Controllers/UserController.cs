using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.controller;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly  ILoginService loginService;

    public UserController(ILoginService loginService)
    {
        this.loginService = loginService;
    }
    
    [HttpPost("login")]
    public async Task <IActionResult> Login([FromBody] LoginViewModel model)
    {
        var check = loginService.Login(model);
        if (check)
        {
            return Ok(GeneralApiResponse<object>.Success("Uses Successfully Logged In"));
        }
        return Unauthorized(GeneralApiResponse<object>.Failure("Invalid username or password" , 401));
    }
}