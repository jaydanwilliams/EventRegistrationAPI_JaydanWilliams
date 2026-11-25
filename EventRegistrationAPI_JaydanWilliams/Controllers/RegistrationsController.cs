using EventRegistrationAPI_JaydanWilliams.DTOs;
using EventRegistrationAPI_JaydanWilliams.Exceptions;
using EventRegistrationAPI_JaydanWilliams.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI_JaydanWilliams.Controllers
{
    /// <summary>
    /// Endpoints for event registrations. Calls IRegistrationService for all work and
    /// turns business rule exceptions into HTTP status codes. Never uses the DbContext.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // GET: api/events/5/registrations
        /// <summary>Returns all registrations for one event, or 404 if the event doesn't exist.</summary>
        [HttpGet("events/{eventId:int}/registrations")]
        public async Task<ActionResult<List<RegistrationDto>>> GetRegistrationsForEvent(int eventId)
        {
            var registrations = await _registrationService.GetRegistrationsForEventAsync(eventId);

            if (registrations == null)
                return NotFound(); // 404: event doesn't exist

            return Ok(registrations); // 200
        }

        // POST: api/events/5/registrations
        /// <summary>
        /// Registers someone for an event. Returns 201 with the new RegistrationDto,
        /// or the status code from whichever business rule was broken.
        /// </summary>
        [HttpPost("events/{eventId:int}/registrations")]
        public async Task<ActionResult<RegistrationDto>> Register(int eventId, CreateRegistrationDto dto)
        {
            try
            {
                var registration = await _registrationService.RegisterAsync(eventId, dto);
                return StatusCode(StatusCodes.Status201Created, registration); // 201
            }
            catch (BusinessRuleException ex)
            {
                // 404, 400 or 409 with the rule's message, decided by the service
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        // DELETE: api/registrations/5
        /// <summary>Cancels a registration. Returns 204, or 404 if it doesn't exist.</summary>
        [HttpDelete("registrations/{id:int}")]
        public async Task<IActionResult> CancelRegistration(int id)
        {
            bool deleted = await _registrationService.CancelRegistrationAsync(id);

            if (!deleted)
                return NotFound(); // 404

            return NoContent(); // 204
        }
    }
}