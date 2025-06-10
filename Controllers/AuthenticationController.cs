using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;
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
    
    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="model">Login credentials containing email and password</param>
    /// <returns>Authentication result with JWT token if successful</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///         "email": "ayham.1396@gmail.com",
    ///         "password": "1234Ayham**"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Login successful - returns JWT token and user information</response>
    /// <response code="401">Login failed - invalid credentials</response>
    /// <response code="400">Bad request - invalid input format</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(GeneralApiResponse<LoginResponseViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GeneralApiResponse<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GeneralApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var response = await _authServices.Login(model);
        return Ok(GeneralApiResponse<object>.Success(response, "Uses Successfully Logged In"));
    }
    
    /// <summary>
    /// Registers a new user account
    /// </summary>
    /// <param name="model">User registration information including name, email, and password</param>
    /// <returns>Registration result with JWT token if successful</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /register
    ///     {
    ///         "name": "Ayham",
    ///         "email": "ayham.1395@gmail.com",
    ///         "password": "1234Ayham**"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Registration successful - returns JWT token and user information</response>
    /// <response code="400">Bad request - validation errors or user already exists</response>
    /// <response code="409">Conflict - email already registered</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(GeneralApiResponse<RegisterResponseViewModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(GeneralApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GeneralApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var response = await _authServices.Register(model);
        return Created("", GeneralApiResponse<object>.Success(response, "User Successfully Registered", 201));
    }

}