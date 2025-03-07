using MediatR;
using ExamProject.Models;

namespace ExamProject.Commands
{
    public class BookTicketCommand : IRequest<BookTicketResponse>
    {
        public List<BookingListModelRequestBody> TicketRequests { get; }

        public BookTicketCommand(List<BookingListModelRequestBody> ticketRequests)
        {
            TicketRequests = ticketRequests;
        }
    }
}
