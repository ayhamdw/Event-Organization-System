using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetUsersViewModel getUsersViewModel)
        {
            var users =await _adminServices.GetAllUsersAsync(getUsersViewModel);
            return Ok(users);
        }
    }
}