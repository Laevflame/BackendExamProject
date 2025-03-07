using ExamProject.Commands;
using ExamProject.Models;
using ExamProject.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/get-booked-ticket/{BookedTicketId}")]
    [ApiController]
    public class BookedTicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookedTicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedTicketsAsync([FromRoute] string BookedTicketId)
        {
            try
            {
                var bookedTicketRequest = new BookedTicketsGet { BookedTicketId = BookedTicketId };
                var command = new GetBookedTicketCommand(bookedTicketRequest);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error",
                    type: "https://tools.ietf.org/html/rfc7807"
                );
            }
        }
    }
}
