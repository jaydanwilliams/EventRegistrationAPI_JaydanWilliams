using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_JaydanWilliams.DTOs
{
    /// <summary>
    /// Data the client sends to register for an event.
    /// EventId comes from the URL, and RegisteredOn is set by the service.
    /// </summary>
    public class CreateRegistrationDto
    {
        [Required(ErrorMessage = "Student name is required.")]
        [StringLength(100, ErrorMessage = "Student name cannot be longer than 100 characters.")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
        public string Email { get; set; } = string.Empty;
    }
}