using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/revoke-ticket")]
    [ApiController]
    public class RevokeController : ControllerBase
    {
        private readonly RevokeTicketServices _service;

        public RevokeController(RevokeTicketServices service)
        {
            _service = service;
        }

        [HttpDelete("{bookedTicketId}/{ticketCode}/{bookedTicketDetailsQuantity}")]
        public async Task<IActionResult> RevokeTicket(string bookedTicketId, string ticketCode, int bookedTicketDetailsQuantity)
        {
            try
            {
                var result = await _service.RevokeTicketAsync(bookedTicketId, ticketCode, bookedTicketDetailsQuantity);
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
