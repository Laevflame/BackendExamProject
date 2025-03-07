using ExamProject.Models;
using MediatR;

namespace ExamProject.Commands
{
    public class RevokeTicketCommand : IRequest<RevokeTicketDto>
    {
        public string BookedTicketId { get; set;  } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public int BookedTicketDetailsQuantity { get; set; }

        public RevokeTicketCommand(string bookedTicketId, string ticketCode, int bookedTicketDetailsQuantity)
        {
            BookedTicketId = bookedTicketId;
            TicketCode = ticketCode;
            BookedTicketDetailsQuantity = bookedTicketDetailsQuantity;
        }
    }
}
