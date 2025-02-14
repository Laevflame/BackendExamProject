using ExamProject.Models;
using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/get-booked-ticket/{BookedTicketId}")]
    [ApiController]
    public class BookedTicketController : ControllerBase
    {
        // GET: api/<BookedTicketController>
        private readonly BookedTicketsServices _service;

        public BookedTicketController(BookedTicketsServices service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookedTicketsAsync([FromRoute] string BookedTicketId)
        {
            try
            {
                var getRequest = new BookedTicketsGet { BookedTicketId = BookedTicketId };
                var result = await _service.GetBookedTicketsAsync(getRequest);
                if (result.Result is NotFoundObjectResult notFoundResult)
                {
                    return NotFound(notFoundResult.Value);
                }

                if (result.Result is BadRequestObjectResult badRequestResult)
                {
                    return BadRequest(badRequestResult.Value);
                }

                return Ok(result);
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
