using ExamProject.Entities;
using ExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Services
{
    public class EditTicketServices
    {
        private readonly BackendExamProjectContext _db;

        public EditTicketServices(BackendExamProjectContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> EditTicketBookedTicketAsync(string bookedTicketId, EditTicketRequest request)
        {
            if(request.BookedTicketDetailsQuantity < 1)
            {
               throw new ArgumentException("BookedTicketDetailsQuantity", "The quantity to edit must be above 0");
            }

            var bookedTicket = await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);

            if (bookedTicket == null)
            {
                throw new KeyNotFoundException("The specified BookedTicketId does not exist.");
            }

            var ticketDetail = bookedTicket.BookedTicketsDetails.FirstOrDefault(t => t.Ticket.TicketCode == request.TicketCode);
            if (ticketDetail == null)
            {
                throw new KeyNotFoundException("The specified TicketCode does not exist.");
            }
            if(request.BookedTicketDetailsQuantity > ticketDetail.BookedTicketDetailsQuantity)
            {
                throw new InvalidOperationException("The quantity to edit is higher than the available ticket quantity.");
            }

            ticketDetail.BookedTicketDetailsQuantity = request.BookedTicketDetailsQuantity;
            await _db.SaveChangesAsync();

            var result = new
            {
                TicketCode = ticketDetail.Ticket.TicketCode,
                TicketName = ticketDetail.Ticket.TicketName,
                BookedTicketDetailsQuantity = ticketDetail.BookedTicketDetailsQuantity
            };

            return new OkObjectResult(result);

        }
    }
}
