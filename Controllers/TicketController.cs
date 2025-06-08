using System.Security.Claims;
using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.model;
using Event_Organization_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.controller;
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

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] BookTicketViewModel bookTicketViewModel)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Console.WriteLine("User ID: " + userId);
        var result = await _ticketServices.BookSeatAsync(bookTicketViewModel , userId);
        return Created("" , GeneralApiResponse<bool>.Success("Ticket booked successfully", 201));
    }

    [HttpDelete("{ticketId:int}")]
    public async Task<IActionResult> CancelTicket(int ticketId)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _ticketServices.CancelTicketAsync(ticketId, userId);
        if (!result)
        {
            return NotFound(GeneralApiResponse<bool>.Failure("Ticket not found or could not be cancelled"));
        }
        return Ok(GeneralApiResponse<bool>.Success("Ticket cancelled successfully"));
    }
}