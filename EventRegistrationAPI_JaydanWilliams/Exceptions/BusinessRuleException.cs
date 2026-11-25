namespace EventRegistrationAPI_JaydanWilliams.Exceptions
{
    /// <summary>
    /// Thrown by the service layer when a business rule is broken.
    /// Carries the HTTP status code and message so the controller can return them
    /// without containing any business logic itself.
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public int StatusCode { get; }

        public BusinessRuleException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}