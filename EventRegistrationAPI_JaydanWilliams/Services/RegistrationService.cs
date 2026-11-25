using EventRegistrationAPI_JaydanWilliams.Data;
using EventRegistrationAPI_JaydanWilliams.DTOs;
using EventRegistrationAPI_JaydanWilliams.Exceptions;
using EventRegistrationAPI_JaydanWilliams.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_JaydanWilliams.Services
{
    /// <summary>
    /// Handles all database work, DTO mapping and business rules for registrations.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;

        public RegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RegistrationDto>?> GetRegistrationsForEventAsync(int eventId)
        {
            // Distinguish "event doesn't exist" (null -> 404) from "event has no registrations" (empty list -> 200)
            bool eventExists = await _context.Events.AnyAsync(e => e.EventId == eventId);
            if (!eventExists)
                return null;

            return await _context.Registrations
                .Where(r => r.EventId == eventId)
                .OrderBy(r => r.RegisteredOn)
                .Select(r => new RegistrationDto
                {
                    RegistrationId = r.RegistrationId,
                    EventTitle = r.Event.Title,
                    StudentName = r.StudentName,
                    Email = r.Email,
                    RegisteredOn = r.RegisteredOn
                })
                .ToListAsync();
        }

        /// <summary>
        /// Registers a person for an event. Business rules are checked in this order:
        ///   1. Event must exist                 -> 404
        ///   2. Event must not be in the past    -> 400
        ///   3. Email must not already be registered for this event (case-insensitive) -> 409
        ///   4. Event must not be full           -> 409
        /// The order matters: an email that is already registered gets the duplicate
        /// message even if the event is also full.
        /// </summary>
        public async Task<RegistrationDto> RegisterAsync(int eventId, CreateRegistrationDto dto)
        {
            // Rule 1: Event must exist
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null)
                throw new BusinessRuleException(404, "Event not found.");

            // Rule 2: No registering for events whose date and time have already passed
            if (ev.EventDate <= DateTime.Now)
                throw new BusinessRuleException(400, "Registration is closed for past events.");

            // Rule 3: No duplicate emails for the same event.
            // Both sides are lowercased so "Priya@X.ca" and "priya@x.ca" count as the same email.
            string email = dto.Email.Trim();
            string normalizedEmail = email.ToLower();

            bool alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.EventId == eventId && r.Email.ToLower() == normalizedEmail);
            if (alreadyRegistered)
                throw new BusinessRuleException(409, "This email is already registered for this event.");

            // Rule 4: Capacity. The event is full when registrations >= capacity.
            int registeredCount = await _context.Registrations.CountAsync(r => r.EventId == eventId);
            if (registeredCount >= ev.Capacity)
                throw new BusinessRuleException(409, "This event is full.");

            // All rules passed: create the registration.
            // RegisteredOn is set here by the service, never by the client.
            var registration = new Registration
            {
                EventId = eventId,
                StudentName = dto.StudentName.Trim(),
                Email = email,
                RegisteredOn = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return new RegistrationDto
            {
                RegistrationId = registration.RegistrationId,
                EventTitle = ev.Title,
                StudentName = registration.StudentName,
                Email = registration.Email,
                RegisteredOn = registration.RegisteredOn
            };
        }

        /// <summary>
        /// Cancels (deletes) a registration, which frees one spot for that event.
        /// SpotsLeft goes up automatically because it's calculated from the registration count.
        /// </summary>
        public async Task<bool> CancelRegistrationAsync(int registrationId)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);
            if (registration == null)
                return false;

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}