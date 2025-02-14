﻿using ExamProject.Entities;
using ExamProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Services
{
    public class BookingTicketService
    {
        private readonly BackendExamProjectContext _db;

        public BookingTicketService(BackendExamProjectContext db)
        {
            _db = db;
        }

        public async Task<BookTicketResponse> BookTicketAsync(List<BookingListModelRequestBody> ticketRequests)
        {
            var response = new BookTicketResponse
            {
                PriceSummary = 0,
                TicketsPerCategories = new List<TicketCategorySummary>()
            };

            var bookedTicketId = $"BT{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";

            var bookedTicket = new BookedTicket
            {
                BookedTicketId = bookedTicketId,
                BookedTicketTotalPrice = 0,
                BookedTicketDate = DateTime.UtcNow,
                PaymentMethodId = 1,
                BookedTicketCreatedAt = DateTime.UtcNow,
                UserAccountId = "User1",
            };

            foreach (var request in ticketRequests)
            {
                var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.TicketCode == request.TicketCode);
                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket with code {request.TicketCode} not found");
                }
                if (ticket.TicketRemainingQuota <= 0)
                {
                    throw new InvalidOperationException($"Ticket with code {request.TicketCode} is sold out");

                }
                if (request.TicketQuantityToBook > ticket.TicketRemainingQuota)
                {
                    throw new InvalidOperationException($"Ticket with code {request.TicketCode} only has {ticket.TicketRemainingQuota} remaining");
                }
                if (ticket.EventDate <= DateTime.UtcNow)
                {
                    throw new InvalidOperationException($"Ticket with code {request.TicketCode} is expired");
                }

                var ticketDetailsId = $"BTD{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";

                var subtotalPrice = ticket.TicketPrice;

                var ticketDetails = new BookedTicketsDetail
                {
                    BookedTicketDetailId = ticketDetailsId,
                    BookedTicketId = bookedTicketId,
                    TicketId = ticket.TicketId,
                    BookedTicketDetailsQuantity = request.TicketQuantityToBook,
                    BookedTicketDetailsSubtotalPrice = subtotalPrice,
                    BookedTicketDetailsCreatedAt = DateTime.UtcNow
                };

                ticket.TicketRemainingQuota -= request.TicketQuantityToBook;

                _db.Tickets.Update(ticket);

                await _db.BookedTicketsDetails.AddAsync(ticketDetails);

                bookedTicket.BookedTicketTotalPrice = subtotalPrice * request.TicketQuantityToBook;

                var category = response.TicketsPerCategories.FirstOrDefault(c => c.CategoryName == ticket.CategoryName);

                if (category == null)
                {
                    category = new TicketCategorySummary
                    {
                        CategoryName = ticket.CategoryName,
                        SummaryPrice = 0,
                        Tickets = new List<TicketDetailsResponse>()
                    };
                    response.TicketsPerCategories.Add(category);
                }

                category.SummaryPrice += bookedTicket.BookedTicketTotalPrice;

                category.Tickets.Add(new TicketDetailsResponse
                {
                    TicketCode = ticket.TicketCode,
                    TicketName = ticket.TicketName,
                    Price = ticket.TicketPrice
                });

                response.PriceSummary += bookedTicket.BookedTicketTotalPrice;
            }
            bookedTicket.BookedTicketPaidAmount = bookedTicket.BookedTicketTotalPrice;
            await _db.BookedTickets.AddAsync(bookedTicket);

            await _db.SaveChangesAsync();

            return response;
        }
    }
}
