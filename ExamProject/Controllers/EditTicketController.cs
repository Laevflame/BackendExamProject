using ExamProject.Models;
using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/edit-booked-ticket")]
    [ApiController]
    public class EditTicketController : ControllerBase
    {
        private readonly EditTicketServices _service;

        public EditTicketController(EditTicketServices service)
        {
            _service = service;
        }


        [HttpPut("{bookedTicketId}")]
        public async Task<IActionResult> EditBookedTicket(string bookedTicketId, [FromBody] EditTicketRequest request)
        {
            try
            {
                var result = await _service.EditTicketBookedTicketAsync(bookedTicketId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    title: "Not Found",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status404NotFound,
                    type: "https://tools.ietf.org/html/rfc7807"
                );
            }
            catch (InvalidOperationException ex)
            {
                return Problem(
                    title: "Invalid Quantity",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status400BadRequest,
                    type: "https://tools.ietf.org/html/rfc7807"
                );
            }
            catch (ArgumentException ex)
            {
                return Problem(
                    title: "Bad Request",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status400BadRequest,
                    type: "https://tools.ietf.org/html/rfc7807"
                );
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Internal Server Error",
                    type: "https://tools.ietf.org/html/rfc7807"
                );
            }

        }
    }
}
