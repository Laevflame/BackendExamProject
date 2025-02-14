using System.Threading.Tasks;
using ExamProject.Entities;
using ExamProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Services
{
    public class TicketsServices
    {
        private readonly BackendExamProjectContext _db;

        public TicketsServices(BackendExamProjectContext db)
        {
            _db = db;
        }


        public async Task<(List<TicketDto> TicketGetFilter, int TotalTickets)> GetAvailableTicketsAsync(TicketGetFilter filter)
        {
            var query = _db.Tickets.Where(t => t.TicketRemainingQuota > 0).AsQueryable();

            // **FILTER**
            if (!string.IsNullOrEmpty(filter.CategoryName))
            {
                query = query.Where(t => t.CategoryName.Contains(filter.CategoryName));
            }

            if (!string.IsNullOrEmpty(filter.TicketCode))
            {
                query = query.Where(t => t.TicketCode.Contains(filter.TicketCode));
            }

            if (!string.IsNullOrEmpty(filter.TicketName))
            {
                query = query.Where(t => t.TicketName.Contains(filter.TicketName));
            }

            if (filter.TicketPrice.HasValue)
            {
                query = query.Where(t => t.TicketPrice <= filter.TicketPrice);
            }

            if (filter.EventDate.HasValue)
            {
                query = query.Where(t => t.EventDate >= filter.EventDate);
            }

            int totalTickets = await query.CountAsync();
            
            query = filter.OrderBy?.ToLower() switch
            {
                "categoryname" => filter.OrderState == "desc" ? query.OrderByDescending(t => t.CategoryName) : query.OrderBy(t => t.CategoryName),
                "ticketname" => filter.OrderState == "desc" ? query.OrderByDescending(t => t.TicketName) : query.OrderBy(t => t.TicketName),
                "ticketprice" => filter.OrderState == "desc" ? query.OrderByDescending(t => t.TicketPrice) : query.OrderBy(t => t.TicketPrice),
                "eventdate" => filter.OrderState == "desc" ? query.OrderByDescending(t => t.EventDate) : query.OrderBy(t => t.EventDate),
                _ => filter.OrderState == "desc" ? query.OrderByDescending(t => t.TicketCode) : query.OrderBy(t => t.TicketCode)
            };

            var tickets = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(t => new TicketDto
                {
                    CategoryName = t.CategoryName,
                    TicketRemainingQuota = t.TicketRemainingQuota,
                    TicketCode = t.TicketCode,
                    TicketName = t.TicketName,
                    TicketPrice = t.TicketPrice,
                    EventDate = t.EventDate
                })
                .ToListAsync();

            return (tickets, totalTickets);
        }
    }
}
