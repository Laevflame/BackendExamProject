using ExamProject.Models;
using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/book-ticket")]
    [ApiController]
    public class TicketBookingController : ControllerBase
    {
        private readonly BookingTicketService _service;

        public TicketBookingController(BookingTicketService service)
        {
            _service = service;
        }

        // POST api/<TicketBookingController>
        [HttpPost]
        public async Task<IActionResult> BookTicket([FromBody] List<BookingListModelRequestBody> ticketRequests)
        {
            try
            {
                var response = await _service.BookTicketAsync(ticketRequests);
                return Ok(response);
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
