using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_JaydanWilliams.Models
{
    /// <summary>
    /// One person's registration for one event (the "many" side of the relationship).
    /// </summary>
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        // Foreign key to Event
        public int EventId { get; set; }

        [Required]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        // Set by the service when a registration is created (not by the client)
        public DateTime RegisteredOn { get; set; }

        // Navigation property back to the parent event
        public Event Event { get; set; } = null!;
    }
}