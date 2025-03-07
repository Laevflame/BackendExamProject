using FluentValidation;
using ExamProject.Models;
using ExamProject.Services;

namespace ExamProject.Validation
{
    public class GetBookedTicketValidator : AbstractValidator<BookedTicketsGet>
    {
        public GetBookedTicketValidator(BookedTicketsServices bookedTicketsServices)
        {
            RuleFor(x => x.BookedTicketId)
                .NotEmpty()
                .WithMessage("BookedTicketId is required")
                .MustAsync(async (bookedTicketId, cancellation) =>
                {
                    return await bookedTicketsServices.BookedTicketIdExists(bookedTicketId);
                })
                .WithMessage("BookedTicketId does not exist");
        }
    }
}
