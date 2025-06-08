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
}