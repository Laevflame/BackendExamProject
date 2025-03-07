using ExamProject.Entities;
using ExamProject.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Validation
{
    public class RevokeTicketValidator : AbstractValidator<(string, string, int)>
    {
        private readonly RevokeTicketServices _services;

        public RevokeTicketValidator(RevokeTicketServices services)
        {
            _services = services;

            RuleFor(x => x.Item1)
                .NotEmpty().WithMessage("BookedTicketId is required.")
                .MustAsync(async (bookedTicketId, cancellation) => await _services.BookedTicketIdExistAsync(bookedTicketId) != null)
                .WithMessage("BookedTicketId does not exist.");

            RuleFor(x => x.Item2)
                .NotEmpty().WithMessage("TicketCode is required.")
                .MustAsync(async (ticketCode, cancellation) => await _services.TicketCodeExistsAsync(ticketCode))
                .WithMessage("TicketCode does not exist.");

            RuleFor(x => x.Item3)
                .GreaterThan(0).WithMessage("The quantity to revoke must be above 0.");

            RuleFor(x => x)
                .MustAsync(async (request, cancellation) =>
                {
                    var bookedTicketDetails = await _services.GetBookedTicketDetailsAsync(request.Item1, request.Item2);
                    return bookedTicketDetails != null;
                })
                .WithMessage(request => $"BookedTicket with id {request.Item1} and Ticket Code {request.Item2} does not exist.");

            RuleFor(x => x)
                .MustAsync(async (request, cancellation) =>
                {
                    var bookedTicketDetails = await _services.GetBookedTicketDetailsAsync(request.Item1, request.Item2);
                    if (bookedTicketDetails == null)
                    {
                        return false;
                    }
                    return request.Item3 <= bookedTicketDetails.BookedTicketDetailsQuantity;
                })
                .WithMessage(request =>
                {
                    var bookedTicketDetails = _services.GetBookedTicketDetailsAsync(request.Item1, request.Item2).Result;
                    if (bookedTicketDetails == null)
                    {
                        return $"BookedTicket with id {request.Item1} or Ticket Code {request.Item2} does not exist.";
                    }
                    return $"The quantity to revoke is higher than the available ticket quantity.";
                });
        }
    }
}
