using System.Security.Claims;
using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger <TicketController> _logger;
    private readonly ITicketServices _ticketServices;

    public TicketController(ILogger<TicketController> logger, ITicketServices ticketServices)
    {
        _logger = logger;
        _ticketServices = ticketServices;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] BookTicketViewModel bookTicketViewModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated" , 401));
        if (!int.TryParse(userId , out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Failure("Invalid user ID format" , 400));
        await _ticketServices.BookSeatAsync(bookTicketViewModel , parsedUserId);
        return Created("" , GeneralApiResponse<bool>.Success("Ticket booked successfully", 201));
    }
    
    [Authorize]
    [HttpDelete("{ticketId:int}")]
    public async Task<IActionResult> CancelTicket(int ticketId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated" , 401));
        if (!int.TryParse(userId , out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Failure("Invalid user ID format" , 400));
        var result = await _ticketServices.CancelTicketAsync(ticketId, parsedUserId);
        if (!result)
        {
            _logger.LogWarning("Ticket cancellation failed for Ticket ID: {TicketId} and User ID: {UserId}", ticketId, userId);
            return NotFound(GeneralApiResponse<bool>.Failure("Ticket not found or could not be cancelled"));
        }
        return Ok(GeneralApiResponse<bool>.Success("Ticket cancelled successfully"));
    }

    [Authorize]
    [HttpGet("personal")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated" , 401));
        if (!int.TryParse(userId , out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Failure("Invalid user ID format" , 400));
        var tickets = await _ticketServices.GetMyBookingAsync(parsedUserId);
        return Ok(GeneralApiResponse<List<PersonalTicketResponseViewModel>>.Success(tickets, "Successfully retrieved your bookings"));
    }
}