using ExamProject.Models;
using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentValidation;
using ExamProject.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/ticket-booking")]
    [ApiController]
    public class TicketBookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketBookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> BookTicket([FromBody] List<BookingListModelRequestBody> ticketRequests)
        {
            try
            {
                var command = new BookTicketCommand(ticketRequests);
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Booking Failed",
                        Status = 400,
                        Detail = "The booking request could not be processed.",
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
