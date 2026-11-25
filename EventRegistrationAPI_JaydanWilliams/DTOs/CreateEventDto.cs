using EventRegistrationAPI_JaydanWilliams.Validation;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_JaydanWilliams.DTOs
{
    /// <summary>
    /// Data the client sends to create an event.
    /// No EventId (the database generates it) and no counts (those are calculated).
    /// </summary>
    public class CreateEventDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(150, ErrorMessage = "Location cannot be longer than 150 characters.")]
        public string Location { get; set; } = string.Empty;

        [FutureDate(ErrorMessage = "Event date must be in the future.")]
        public DateTime EventDate { get; set; }

        [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
        public int Capacity { get; set; }
    }
}