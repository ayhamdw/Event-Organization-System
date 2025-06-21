using System.Security.Claims;
using EventOrganizationSystem.Generic;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizationSystem.Controllers;
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
    /// <summary>
    /// Books a ticket for an event (Authenticated users only)
    /// </summary>
    /// <param name="bookTicketViewModel">Ticket booking details</param>
    /// <returns>Confirmation of successful booking</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/ticket
    ///     {
    ///         "eventId": 5,
    ///         "seatNumber": "A12",
    ///         "paymentMethod": "CreditCard"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Ticket booked successfully</response>
    /// <response code="400">Invalid request data or user ID format</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
    /// <response code="404">Event or seat not available</response>
    /// <response code="409">Seat already booked</response>
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
    /// <summary>
    /// Cancels an existing ticket booking (Authenticated users only)
    /// </summary>
    /// <param name="ticketId">ID of the ticket to cancel</param>
    /// <returns>Confirmation of cancellation</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/ticket/8
    ///
    /// </remarks>
    /// <response code="200">Ticket cancelled successfully</response>
    /// <response code="400">Invalid user ID format</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
    /// <response code="403">Forbidden - user doesn't own this ticket</response>
    /// <response code="404">Ticket not found</response>
    /// <response code="409">Cancellation not allowed (past deadline)</response>
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
    /// <summary>
    /// Retrieves all tickets booked by the current user (Authenticated users only)
    /// </summary>
    /// <returns>List of user's booked tickets</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/ticket/personal
    ///
    /// </remarks>
    /// <response code="200">Successfully retrieved user's tickets</response>
    /// <response code="400">Invalid user ID format</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
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