using ExamProject.Entities;
using ExamProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Services
{
    public class BookedTicketsServices
    {
        private readonly BackendExamProjectContext _db;

        public BookedTicketsServices(BackendExamProjectContext db)
        {
            _db = db;
        }

        public async Task<bool> BookedTicketIdExists(string bookedTicketId)
        {
            return await _db.BookedTickets.AnyAsync(t => t.BookedTicketId == bookedTicketId);
        }

        public async Task<BookedTicket?> GetBookedTickedIdAsync(string bookedTicketId)
        {
            return await _db.BookedTickets.FirstOrDefaultAsync(t => t.BookedTicketId == bookedTicketId);
        }

        public async Task<List<BookingResponseDto>> GetBookedTicketsAsync(BookedTicketsGet get)
        {
            var query = _db.BookedTickets
                    .Include(b => b.BookedTicketsDetails)
                    .ThenInclude(d => d.Ticket)
                    .AsQueryable();

            // Filter by BookedTicketId if provided
            query = query.Where(t => t.BookedTicketId.Contains(get.BookedTicketId));

            // Execute query and transform data
            var result = await query
                .Select(b => new BookingResponseDto
                {
                    Categories = b.BookedTicketsDetails
                        .GroupBy(d => d.Ticket.CategoryName) // Group tickets by category
                        .Select(g => new CategoryDetailsDto
                        {
                            CategoryName = g.Key,
                            QuantityPerCategory = g.Sum(d => d.BookedTicketDetailsQuantity),
                            Tickets = g.Select(d => new TicketDetailsDto
                            {
                                TicketName = d.Ticket.TicketName,
                                TicketCode = d.Ticket.TicketCode,
                                EventDate = d.Ticket.EventDate
                            }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return result;
        }
    }
}
