using ExamProject.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExamProject.Commands;
using FluentValidation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/revoke-ticket")]
    [ApiController]
    public class RevokeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RevokeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{bookedTicketId}/{ticketCode}/{bookedTicketDetailsQuantity}")]
        public async Task<IActionResult> RevokeTicket(string bookedTicketId, string ticketCode, int bookedTicketDetailsQuantity)
        {
            try
            {
                var command = new RevokeTicketCommand(bookedTicketId, ticketCode, bookedTicketDetailsQuantity);
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Revoke Failed",
                        Status = 400,
                        Detail = "The revoke request could not be processed."
                    });
                }

                return Ok(new { success = true, data = result });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = 400,
                    Detail = "One or more validation errors occurred.",
                    Extensions = { ["errors"] = ex.Errors }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = 500,
                    Detail = "An unexpected error occurred.",
                    Extensions = { ["exceptionMessage"] = ex.Message }
                });
            }
        }
    }
}
