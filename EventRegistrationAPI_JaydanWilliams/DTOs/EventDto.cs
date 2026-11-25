namespace EventRegistrationAPI_JaydanWilliams.DTOs
{
    /// <summary>
    /// Event data returned to the client.
    /// RegisteredCount and SpotsLeft are calculated by the service, not stored in the database.
    /// </summary>
    public class EventDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public int SpotsLeft { get; set; }
    }
}