using FluentValidation;
using ExamProject.Commands;
using ExamProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class GetBookedCommandValidator : AbstractValidator<GetBookedTicketCommand>
    {
        private readonly BackendExamProjectContext _db;

        public GetBookedCommandValidator(BackendExamProjectContext db)
        {
            _db = db;

            RuleFor(x => x.BookedTicketRequest).ChildRules(ticketRequest =>
            {
                ticketRequest.RuleFor(x => x.BookedTicketId)
                    .NotEmpty().WithMessage("BookedTicketId is required.")
                    .MustAsync(TicketExists).WithMessage("BookedTicket with id {PropertyValue} does not exist.");
            });
        }

        private async Task<bool> TicketExists(string bookedTicketId, CancellationToken cancellationToken)
        {
            return await _db.BookedTickets.AnyAsync(t => t.BookedTicketId == bookedTicketId, cancellationToken);
        }
    }
}
