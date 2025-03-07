using ExamProject.Services;
using MediatR;
using ExamProject.Commands;
using ExamProject.Models;
using FluentValidation;

namespace ExamProject.Handler
{
    public class BookTicketCommandHandler : IRequestHandler<BookTicketCommand, BookTicketResponse>
    {
        private readonly BookingTicketService _service;
        private readonly IValidator<BookingListModelRequestBody> _validator;

        public BookTicketCommandHandler(BookingTicketService service, IValidator<BookingListModelRequestBody> validator)
        {
            _service = service;
            _validator = validator;
        }

        public async Task<BookTicketResponse> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            foreach (var ticketRequest in request.TicketRequests)
            {
                var validationResult = await _validator.ValidateAsync(ticketRequest, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            return await _service.BookTicketsAsync(request.TicketRequests);
        }
    }
}
