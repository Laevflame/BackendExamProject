using ExamProject.Entities;
using ExamProject.Models;
using ExamProject.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class EditTicketValidator : AbstractValidator<(string, EditTicketRequest)>
    {
        public EditTicketValidator(EditTicketServices services)
        {
            RuleFor(x => x.Item1)
                .NotEmpty().WithMessage("BookedTicketId is required.")
                .MustAsync(async (bookedTicketId, cancellation) => await services.BookedTicketIdExistAsync(bookedTicketId) != null)
                .WithMessage("BookedTicketId does not exist.");

            RuleFor(x => x.Item2.TicketCode)
                .MustAsync(async (request, ticketCode, cancellation) => await services.TicketCodeExistsAsync(request.Item1, ticketCode))
                .WithMessage("TicketCode does not exist.");

            RuleFor(x => x.Item2.BookedTicketDetailsQuantity)
                .GreaterThan(0).WithMessage("BookedTicketDetailsQuantity must be greater than 0.");

            RuleFor(x => x).CustomAsync(async (request, context, cancellationToken) =>
            {
                var bookedTicket = await services.BookedTicketIdExistAsync(request.Item1);

                if (bookedTicket == null)
                {
                    context.AddFailure("BookedTicketId", "The specified BookedTicketId does not exist.");
                    return;
                }

                var ticketDetail = bookedTicket.BookedTicketsDetails
                    .FirstOrDefault(t => t.Ticket.TicketCode == request.Item2.TicketCode);

                if (ticketDetail == null)
                {
                    context.AddFailure("TicketCode", "The specified TicketCode does not exist.");
                    return;
                }

                if (request.Item2.BookedTicketDetailsQuantity > ticketDetail.BookedTicketDetailsQuantity)
                {
                    context.AddFailure("BookedTicketDetailsQuantity", "The quantity to edit is higher than the available ticket quantity.");
                }
            });
        }
    }
}
