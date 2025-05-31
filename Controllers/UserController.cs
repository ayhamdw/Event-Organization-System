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
            return Accepted("Login successful");
        }
        return Unauthorized("Invalid email or password");
    }
}