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
    }
}