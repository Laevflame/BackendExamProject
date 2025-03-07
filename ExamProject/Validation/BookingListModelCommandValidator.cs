using FluentValidation;
using ExamProject.Commands;
using ExamProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class BookingListModelCommandValidator : AbstractValidator<BookTicketCommand>
    {
        private readonly BackendExamProjectContext _db;

        public BookingListModelCommandValidator(BackendExamProjectContext db)
        {
            _db = db;

            RuleForEach(x => x.TicketRequests).ChildRules(ticketRequest =>
            {
                ticketRequest.RuleFor(x => x.TicketCode)
                    .NotEmpty().WithMessage("TicketCode is required.")
                    .MustAsync(TicketExists).WithMessage("Ticket with code {PropertyValue} does not exist.");
                ticketRequest.RuleFor(x => x.TicketQuantityToBook)
                    .GreaterThan(0).WithMessage("TicketQuantityToBook must be greater than 0.");
            });
        }

        private async Task<bool> TicketExists(string ticketCode, CancellationToken cancellationToken)
        {
            return await _db.Tickets.AnyAsync(t => t.TicketCode == ticketCode, cancellationToken);
        }
    }
}
