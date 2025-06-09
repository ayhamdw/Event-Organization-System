using System.Security.Claims;
using Event_Organization_System.Generic;
using Event_Organization_System.IServices;
using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.controller;
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventServices _eventServices;

    public EventController(IEventServices eventServices)
    {
        _eventServices = eventServices;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] GetEventViewModel getEventViewModel)
    {
        var events = await _eventServices.GetAllEventsAsync(getEventViewModel);
        return Ok(GeneralApiResponse<List<EventResponseViewModel>>.Success(events));
    }
    
    [HttpGet("{id:int}")]
    public async Task <IActionResult> GetEventById([FromRoute] int id)
    {
        var eventResponse = await _eventServices.GetEventByIdAsync(id);
        return Ok(GeneralApiResponse<EventResponseViewModel>.Success(eventResponse));
    }
    
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