using ExamProject.Models;
using MediatR;

namespace ExamProject.Commands
{
    public class GetBookedTicketCommand : IRequest<BookingResponseDto>
    {
        public BookedTicketsGet BookedTicketRequest { get; }

        public GetBookedTicketCommand(BookedTicketsGet bookedTicketRequest)
        {
            BookedTicketRequest = bookedTicketRequest;
        }
    }
}
