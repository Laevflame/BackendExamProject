using ExamProject.Commands;
using ExamProject.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class RevokeTicketCommandValidator : AbstractValidator<RevokeTicketCommand>
    {
        private readonly BackendExamProjectContext _db;

        public RevokeTicketCommandValidator(BackendExamProjectContext db)
        {
            _db = db;
            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("BookedTicketId is Required")
                .MustAsync(TicketExists).WithMessage("BookedTicket with id {PropertyValue} does not exist.");
            RuleFor(x => x.TicketCode)
                .NotEmpty().WithMessage("TicketCode is required.")
                .MustAsync(TicketCodeExists).WithMessage("TicketCode with id {PropertyValue} does not exist.");

            RuleFor(x => x.BookedTicketDetailsQuantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must above 0.");
        }
        private async Task<bool> TicketExists(string bookedTicketId, CancellationToken cancellationToken)
        {
            return await _db.BookedTickets.AnyAsync(t => t.BookedTicketId == bookedTicketId, cancellationToken);
        }
        private async Task<bool> TicketCodeExists(string ticketCode, CancellationToken cancellationToken)
        {
            return await _db.Tickets.AnyAsync(t => t.TicketCode == ticketCode, cancellationToken);
        }
        
    }
}
