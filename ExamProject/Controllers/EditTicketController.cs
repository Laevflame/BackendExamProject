using ExamProject.Models;
using ExamProject.Services;
using MediatR;
using ExamProject.Commands;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/edit-booked-ticket")]
    [ApiController]
    public class EditTicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EditTicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{bookedTicketId}")]
        public async Task<IActionResult> EditBookedTicket(string bookedTicketId, [FromBody] EditTicketRequest request)
        {
            try
            {
                var command = new EditTicketCommand(bookedTicketId, request);
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Edit Failed",
                        Status = 400,
                        Detail = "The edit request could not be processed."
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
