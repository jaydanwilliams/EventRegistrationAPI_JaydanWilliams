using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_JaydanWilliams.Validation
{
    /// <summary>
    /// Custom validation attribute: the date must be later than the current date and time.
    /// Used on CreateEventDto.EventDate so events can't be created in the past.
    /// </summary>
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // Valid only if the value is a DateTime that hasn't happened yet.
            // A missing date defaults to 0001-01-01, which is in the past, so it fails too.
            return value is DateTime date && date > DateTime.Now;
        }
    }
}