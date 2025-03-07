using ExamProject.Models;
using MediatR;

namespace ExamProject.Commands
{
    public class EditTicketCommand : IRequest<EditTicketResponse>
    {
        public string BookedTicketId { get; set; } = string.Empty;
        public EditTicketRequest EditRequest { get; }

        public EditTicketCommand(string bookedTicketId, EditTicketRequest editRequest)
        {
            BookedTicketId = bookedTicketId;
            EditRequest = editRequest;
        }
    }
}
