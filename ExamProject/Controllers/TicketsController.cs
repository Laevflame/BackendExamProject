using ExamProject.Models;
using ExamProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamProject.Controllers
{
    [Route("api/v1/get-available-ticket")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketsServices _service;
        public TicketsController(TicketsServices service)
        {
            _service = service;
        }
        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<IActionResult> GetAvailableTickets([FromQuery] TicketGetFilter filter)
        {
            try
            {
                var (tickets, totalTickets) = await _service.GetAvailableTicketsAsync(filter);
                return Ok(new
                {
                    tickets,
                    totalTickets
                });
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
