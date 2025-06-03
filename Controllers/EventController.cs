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
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventServices.GetAllEventsAsync();
        return Ok(GeneralApiResponse<List<EventResponseViewModel>>.Success(events));
    }
    
    [HttpPost]
    [Authorize (Roles = "Admin , Organizer")]
    public async Task<IActionResult> CreateEvent([FromBody] EventViewModel eventViewModel)
    {

        var createdEvent = await _eventServices.CreateEventAsync(eventViewModel);
        return Created("",
            GeneralApiResponse<EventResponseViewModel>.Success(createdEvent, "Event created successfully" , 201));
    }
}