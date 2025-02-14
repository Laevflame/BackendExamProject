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

        public async Task<RevokeTicketDto> RevokeTicketAsync(string bookedTicketId, string ticketCode, int bookedTicketDetailsQuantity)
        {
            if (bookedTicketDetailsQuantity <= 0)
            {
                throw new ArgumentException("The quantity to revoke must be above 0");
            }

            var bookedTicketDetails = await _db.BookedTicketsDetails
                .Include(td => td.Ticket)
                .Where(td => td.BookedTicketId == bookedTicketId && td.Ticket.TicketCode == ticketCode)
                .FirstOrDefaultAsync();

            if (bookedTicketDetails == null)
            {
                throw new KeyNotFoundException("The specified BookedTicketId or TicketCode does not exist.");
            }

            if (bookedTicketDetailsQuantity > bookedTicketDetails.BookedTicketDetailsQuantity)
            {
                throw new InvalidOperationException("The quantity to revoke is higher than the available ticket quantity.");
            }

            bookedTicketDetails.BookedTicketDetailsQuantity -= bookedTicketDetailsQuantity;

            if (bookedTicketDetails.BookedTicketDetailsQuantity <= 0)
            {
                _db.BookedTicketsDetails.Remove(bookedTicketDetails);
            }

            bool hasRemainingTickets = await _db.BookedTicketsDetails
                .AnyAsync(td => td.BookedTicketId == bookedTicketId && td.BookedTicketDetailsQuantity > 0);
            if (!hasRemainingTickets)
            {
                var ticketsBooked = await _db.BookedTickets.FindAsync(bookedTicketId);
                if (ticketsBooked != null)
                {
                    _db.BookedTickets.Remove(ticketsBooked);
                }
            }
            await _db.SaveChangesAsync();

            return new RevokeTicketDto
            {
                TicketCode = bookedTicketDetails.Ticket.TicketCode,
                TicketName = bookedTicketDetails.Ticket.TicketName,
                CategoryName = bookedTicketDetails.Ticket.CategoryName,
                RemainingQuantity = bookedTicketDetails.BookedTicketDetailsQuantity
            };
        }
    }
}
