namespace EventRegistrationAPI_JaydanWilliams.DTOs
{
    /// <summary>
    /// Registration data returned to the client.
    /// Shows the event's title instead of exposing the Event entity.
    /// </summary>
    public class RegistrationDto
    {
        public int RegistrationId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredOn { get; set; }
    }
}