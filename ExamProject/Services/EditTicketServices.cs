using ExamProject.Entities;
using ExamProject.Models;
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

        public async Task<bool> TicketCodeExistsAsync(string bookedTicketId, string ticketCode)
        {
            var bookedTicket = await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);

            if (bookedTicket == null)
            {
                return false;
            }

            return bookedTicket.BookedTicketsDetails.Any(td => td.Ticket.TicketCode == ticketCode);
        }

        public async Task<BookedTicket?> BookedTicketIdExistAsync(string bookedTicketId)
        {
            return await _db.BookedTickets.FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);
        }

        public async Task<EditTicketResponse> EditTicketBookedTicketAsync(string bookedTicketId, EditTicketRequest request)
        {
            var bookedTicket = await _db.BookedTickets
                .Include(t => t.BookedTicketsDetails)
                .ThenInclude(td => td.Ticket)
                .FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);

            var ticketDetail = bookedTicket.BookedTicketsDetails.FirstOrDefault(t => t.Ticket.TicketCode == request.TicketCode);

            ticketDetail.BookedTicketDetailsQuantity = request.BookedTicketDetailsQuantity;
            await _db.SaveChangesAsync();

            var response =  new EditTicketResponse
            {
                TicketCode = ticketDetail.Ticket.TicketCode,
                TicketName = ticketDetail.Ticket.TicketName,
                BookedTicketDetailsQuantity = ticketDetail.BookedTicketDetailsQuantity,
                categoryName = ticketDetail.Ticket.CategoryName
            };
            return response;
        }
    }
}