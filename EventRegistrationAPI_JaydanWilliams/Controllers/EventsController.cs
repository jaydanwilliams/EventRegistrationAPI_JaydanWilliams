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

        // POST: api/events
        /// <summary>
        /// Creates a new event. Invalid input is rejected automatically with 400
        /// by [ApiController] using the validation attributes on CreateEventDto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
        {
            var createdEvent = await _eventService.CreateEventAsync(dto);

            // 201 Created, with a Location header pointing to GET api/events/{id}
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.EventId }, createdEvent);
        }
    }
}