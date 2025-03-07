using ExamProject.Commands;
using ExamProject.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class EditTicketCommandValidator : AbstractValidator<EditTicketCommand>
    {
        private readonly BackendExamProjectContext _db;

        public EditTicketCommandValidator(BackendExamProjectContext db)
        {
            _db = db;

            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("BookedTicketId is required.")
                .MustAsync(TicketExists).WithMessage("BookedTicket with id {PropertyValue} does not exist.");

            RuleFor(x => x.EditRequest).ChildRules(editRequest =>
            {
                editRequest.RuleFor(x => x.TicketCode)
                    .NotEmpty().WithMessage("TicketCode is required.")
                    .MustAsync(TicketCodeExists).WithMessage("TicketCode with id {PropertyValue} does not exist.");

                editRequest.RuleFor(x => x.BookedTicketDetailsQuantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            });
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
