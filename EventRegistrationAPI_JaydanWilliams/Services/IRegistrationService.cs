using EventRegistrationAPI_JaydanWilliams.DTOs;

namespace EventRegistrationAPI_JaydanWilliams.Services
{
    /// <summary>
    /// Contract for registration operations, including the registration business rules.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>Returns the event's registrations, or null if the event doesn't exist.</summary>
        Task<List<RegistrationDto>?> GetRegistrationsForEventAsync(int eventId);

        /// <summary>Registers someone for an event. Throws BusinessRuleException if a rule is broken.</summary>
        Task<RegistrationDto> RegisterAsync(int eventId, CreateRegistrationDto dto);

        /// <summary>Cancels a registration. Returns false if it doesn't exist.</summary>
        Task<bool> CancelRegistrationAsync(int registrationId);
    }
}