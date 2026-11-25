using EventRegistrationAPI_JaydanWilliams.Data;
using EventRegistrationAPI_JaydanWilliams.DTOs;
using EventRegistrationAPI_JaydanWilliams.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventRegistrationAPI_JaydanWilliams.Services
{
    /// <summary>
    /// Handles all database work and DTO mapping for events.
    /// Controllers call this service and never use the DbContext directly.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Maps an Event entity to an EventDto. Written as an Expression so EF Core
        /// can translate it to SQL: the registration count is calculated in the
        /// database with COUNT, so registrations don't have to be loaded into memory.
        /// </summary>
        private static readonly Expression<Func<Event, EventDto>> ToEventDto = e => new EventDto
        {
            EventId = e.EventId,
            Title = e.Title,
            Location = e.Location,
            EventDate = e.EventDate,
            Capacity = e.Capacity,
            RegisteredCount = e.Registrations.Count,
            SpotsLeft = e.Capacity - e.Registrations.Count
        };

        /// <summary>Returns all events, sorted by date (earliest first).</summary>
        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            return await _context.Events
                .OrderBy(e => e.EventDate)
                .Select(ToEventDto)
                .ToListAsync();
        }

        /// <summary>Returns one event, or null if no event has that ID.</summary>
        public async Task<EventDto?> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Where(e => e.EventId == id)
                .Select(ToEventDto)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new event from the input DTO and returns it as an EventDto.
        /// A new event has no registrations yet, so RegisteredCount is 0 and all spots are open.
        /// </summary>
        public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
        {
            // Map the input DTO to a new entity
            var newEvent = new Event
            {
                Title = dto.Title,
                Location = dto.Location,
                EventDate = dto.EventDate,
                Capacity = dto.Capacity
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync(); // EF fills in newEvent.EventId after saving

            // Map the saved entity back to an output DTO
            return new EventDto
            {
                EventId = newEvent.EventId,
                Title = newEvent.Title,
                Location = newEvent.Location,
                EventDate = newEvent.EventDate,
                Capacity = newEvent.Capacity,
                RegisteredCount = 0,
                SpotsLeft = newEvent.Capacity
            };
        }
    }
}