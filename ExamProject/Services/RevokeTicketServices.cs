using ExamProject.Entities;
using ExamProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Services
{
    public class RevokeTicketServices
    {
        private readonly BackendExamProjectContext _db;

        public RevokeTicketServices(BackendExamProjectContext db)
        {
            _db = db;
        }
        public async Task<BookedTicket?> BookedTicketIdExistAsync(string bookedTicketId)
        {
            return await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);
        }

        public async Task<bool> TicketCodeExistsAsync(string ticketCode)
        {
            return await _db.Tickets.AnyAsync(t => t.TicketCode == ticketCode);
        }

        public async Task<BookedTicketsDetail?> GetBookedTicketDetailsAsync(string bookedTicketId, string ticketCode)
        {
            var bookedTicket = await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);

            return bookedTicket?.BookedTicketsDetails
                .FirstOrDefault(t => t.Ticket.TicketCode == ticketCode);
        }
        public async Task<RevokeTicketDto> RevokeTicketAsync(string bookedTicketId, string ticketCode, int quantity)
        {
            var bookedTicket = await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);

            if (bookedTicket == null)
            {
                throw new Exception("BookedTicket not found.");
            }

            var ticketDetail = bookedTicket.BookedTicketsDetails.FirstOrDefault(t => t.Ticket.TicketCode == ticketCode);

            if (ticketDetail == null)
            {
                throw new Exception("Ticket detail not found.");
            }

            if (quantity > ticketDetail.BookedTicketDetailsQuantity)
            {
                throw new Exception("The quantity to revoke is higher than the available ticket quantity.");
            }

            ticketDetail.BookedTicketDetailsQuantity -= quantity;
            await _db.SaveChangesAsync();

            return new RevokeTicketDto
            {
                TicketCode = ticketDetail.Ticket.TicketCode,
                TicketName = ticketDetail.Ticket.TicketName,
                CategoryName = ticketDetail.Ticket.CategoryName,
                RemainingQuantity = ticketDetail.BookedTicketDetailsQuantity
            };
        }
    }
}
