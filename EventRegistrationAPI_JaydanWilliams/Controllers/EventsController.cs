using EventRegistrationAPI_JaydanWilliams.DTOs;
using EventRegistrationAPI_JaydanWilliams.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI_JaydanWilliams.Controllers
{
    /// <summary>
    /// Endpoints for campus events. Calls IEventService for all work and only
    /// decides which HTTP status code to return. Never uses the DbContext.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/events
        /// <summary>Returns all events sorted by date.</summary>
        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events); // 200
        }

        // GET: api/events/5
        /// <summary>Returns one event, or 404 if it doesn't exist.</summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);

            if (eventDto == null)
                return NotFound(); // 404

            return Ok(eventDto); // 200
        }
    }
}