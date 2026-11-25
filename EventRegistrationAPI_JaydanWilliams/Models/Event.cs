using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_JaydanWilliams.Models
{
    /// <summary>
    /// A campus event that students and staff can register for.
    /// One Event has many Registrations (one-to-many).
    /// </summary>
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public int Capacity { get; set; }

        // Navigation collection: all registrations for this event
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}