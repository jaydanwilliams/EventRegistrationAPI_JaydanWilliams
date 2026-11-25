using EventRegistrationAPI_JaydanWilliams.DTOs;

namespace EventRegistrationAPI_JaydanWilliams.Services
{
    /// <summary>
    /// Contract for event-related operations. Controllers depend on this interface,
    /// not on the concrete class, so the implementation can change without touching them.
    /// </summary>
    public interface IEventService
    {
        Task<List<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(int id);
    }
}