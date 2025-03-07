using ExamProject.Commands;
using ExamProject.Models;
using ExamProject.Services;
using FluentValidation;
using MediatR;

namespace ExamProject.Handler
{
    public class GetBookedTicketCommandHandler : IRequestHandler<GetBookedTicketCommand, BookingResponseDto>
    {
        private readonly BookedTicketsServices _service;
        private readonly IValidator<BookedTicketsGet> _validator;

        public GetBookedTicketCommandHandler(BookedTicketsServices service, IValidator<BookedTicketsGet> validator)
        {
            _service = service;
            _validator = validator;
        }

        public async Task<BookingResponseDto> Handle(GetBookedTicketCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.BookedTicketRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Ensure that validation errors are not duplicated
                var distinctErrors = validationResult.Errors
                    .GroupBy(e => new { e.PropertyName, e.ErrorMessage })
                    .Select(g => g.First())
                    .ToList();

                throw new ValidationException(distinctErrors);
            }

            var bookedTickets = await _service.GetBookedTicketsAsync(request.BookedTicketRequest);
            return new BookingResponseDto { Categories = bookedTickets.SelectMany(bt => bt.Categories).ToList() };
        }
    }
}
