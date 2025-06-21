using System.Security.Claims;
using EventOrganizationSystem.Generic;
using EventOrganizationSystem.IServices;
using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizationSystem.controller;
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventServices _eventServices;

    public EventController(IEventServices eventServices)
    {
        _eventServices = eventServices;
    }
    /// <summary>
    /// Retrieves a paginated list of all events with optional filtering and sorting
    /// </summary>
    /// <param name="getEventViewModel">Pagination, filtering and sorting parameters</param>
    /// <returns>Paginated list of events</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/event?pageNumber=1&pageSize=10&sortBy=Date
    ///
    /// </remarks>
    /// <response code="200">Successfully retrieved list of events</response>
    /// <response code="400">Invalid query parameters</response>
    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] GetEventViewModel getEventViewModel)
    {
        var events = await _eventServices.GetAllEventsAsync(getEventViewModel);
        return Ok(GeneralApiResponse<List<EventResponseViewModel>>.Success(events));
    }
    /// <summary>
    /// Retrieves detailed information about a specific event
    /// </summary>
    /// <param name="id">The ID of the event to retrieve</param>
    /// <returns>Complete event details</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/event/5
    ///
    /// </remarks>
    /// <response code="200">Successfully retrieved event details</response>
    /// <response code="404">Event not found</response>
    [HttpGet("{id:int}")]
    public async Task <IActionResult> GetEventById([FromRoute] int id)
    {
        var eventResponse = await _eventServices.GetEventByIdAsync(id);
        return Ok(GeneralApiResponse<EventResponseViewModel>.Success(eventResponse));
    }
    /// <summary>
    /// Creates a new event (Organizer role required)
    /// </summary>
    /// <param name="createEventViewModel">Event creation data</param>
    /// <returns>Newly created event details</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/event
    ///     {
    ///         "name": "Tech Conference",
    ///         "description": "Annual technology conference",
    ///         "location": "Convention Center",
    ///         "Time": "2023-12-15T09:00:00",
    ///         "Location": "Nablus",
    ///         "TotalSeats": 100,
    ///         "RemainingSeats": 25,
    ///         "CreatedBy": 1,
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Event created successfully</response>
    /// <response code="400">Invalid event data</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
    /// <response code="403">Forbidden - user not an organizer</response>
    [HttpPost]
    [Authorize (Roles = "Organizer")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventViewModel createEventViewModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated", 401));
        
        if (!int.TryParse(userId, out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Success("Invalid user ID format", 400));
        
        var createdEvent = await _eventServices.CreateEventAsync(createEventViewModel , parsedUserId);
        return Created("",
            GeneralApiResponse<EventResponseViewModel>.Success(createdEvent, "Event created successfully" , 201));
    }
    /// <summary>
    /// Updates an existing event (Organizer role required)
    /// </summary>
    /// <param name="id">ID of the event to update</param>
    /// <param name="createEventViewModel">Updated event data</param>
    /// <returns>Updated event details</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/event/5
    ///     {
    ///         "name": "Tech Conference updated",
    ///         "description": "Annual technology conference updated",
    ///         "location": "Convention Center updated" ,
    ///         "Time": "2025-2-20T09:00:00",
    ///         "Location": "Ramallah ",
    ///         "TotalSeats": 100,
    ///         "RemainingSeats": 68,
    ///         "CreatedBy": 1,
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Event updated successfully</response>
    /// <response code="400">Invalid event data</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
    /// <response code="403">Forbidden - user not an organizer or not event owner</response>
    /// <response code="404">Event not found</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> UpdateEvent([FromRoute]int id, [FromBody] CreateEventViewModel createEventViewModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated", 401));
        
        if (!int.TryParse(userId, out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Success("Invalid user ID format", 400));
        var response = await _eventServices.UpdateEventAsync(createEventViewModel, id, parsedUserId);
        return Ok(GeneralApiResponse<EventResponseViewModel>.Success(response));
    }
    /// <summary>
    /// Deletes an existing event (Organizer role required)
    /// </summary>
    /// <param name="id">ID of the event to delete</param>
    /// <returns>Success status</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/event/5
    ///
    /// </remarks>
    /// <response code="200">Event deleted successfully</response>
    /// <response code="401">Unauthorized - user not authenticated</response>
    /// <response code="403">Forbidden - user not an organizer or not event owner</response>
    /// <response code="404">Event not found</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) 
            return Unauthorized(GeneralApiResponse<string>.Failure("User not authenticated", 401));
        
        if (!int.TryParse(userId, out var parsedUserId)) 
            return BadRequest(GeneralApiResponse<string>.Success("Invalid user ID format", 400));
        await _eventServices.DeleteEventAsync(id, parsedUserId);
        return Ok(GeneralApiResponse<bool>.Success());
    }
    
}