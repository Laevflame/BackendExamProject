using FluentValidation;
using ExamProject.Models;
using ExamProject.Services;

namespace ExamProject.Validation
{
    public class BookingListModelValidator : AbstractValidator<BookingListModelRequestBody>
    {
        public BookingListModelValidator(BookingTicketService bookingTicketService)
        {
            RuleFor(x => x.TicketCode)
                .NotEmpty().WithMessage("TicketCode is required.")
                .MustAsync(async (ticketCode, cancellation) =>
                    await bookingTicketService.TicketExistsAsync(ticketCode))
                .WithMessage("Ticket with the given code does not exist.");

            RuleFor(x => x.TicketQuantityToBook)
                .GreaterThan(0).WithMessage("TicketQuantityToBook must be greater than 0.");
            RuleFor(x => x)
                .MustAsync(async (request, cancellation) =>
                {
                    var ticket = await bookingTicketService.GetTicketByCodeAsync(request.TicketCode);
                    if (ticket == null)
                    {
                        return false;
                    }

                    if (ticket.TicketRemainingQuota <= 0)
                    {
                        return false;
                    }

                    if (request.TicketQuantityToBook > ticket.TicketRemainingQuota)
                    {
                        return false;
                    }

                    if (ticket.EventDate <= DateTime.UtcNow)
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage(request =>
                {
                    var ticket = bookingTicketService.GetTicketByCodeAsync(request.TicketCode).Result;
                    if (ticket == null)
                    {
                        return $"Ticket with code {request.TicketCode} does not exist.";
                    }

                    if (ticket.TicketRemainingQuota <= 0)
                    {
                        return $"Ticket with code {request.TicketCode} is sold out.";
                    }

                    if (request.TicketQuantityToBook > ticket.TicketRemainingQuota)
                    {
                        return $"Ticket with code {request.TicketCode} only has {ticket.TicketRemainingQuota} remaining.";
                    }

                    if (ticket.EventDate <= DateTime.UtcNow)
                    {
                        return $"Ticket with code {request.TicketCode} is expired.";
                    }

                    return "Invalid ticket booking request.";
                });
        }
    }
}
