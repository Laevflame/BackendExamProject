using ExamProject.Commands;
using ExamProject.Models;
using ExamProject.Services;
using FluentValidation;
using MediatR;

namespace ExamProject.Handler
{
    public class RevokeTicketCommandHandler : IRequestHandler<RevokeTicketCommand, RevokeTicketDto>
    {
        private readonly RevokeTicketServices _service;
        private readonly IValidator<(string, string, int)> _validator;

        public RevokeTicketCommandHandler(RevokeTicketServices service, IValidator<(string, string, int)> validator)
        {
            _service = service;
            _validator = validator;
        }

        public async Task<RevokeTicketDto> Handle(RevokeTicketCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync((request.BookedTicketId, request.TicketCode, request.BookedTicketDetailsQuantity), cancellationToken);
            if (!validationResult.IsValid)
            {
                var distinctErrors = validationResult.Errors
                    .GroupBy(e => new { e.PropertyName, e.ErrorMessage })
                    .Select(g => g.First())
                    .ToList();

                throw new ValidationException(distinctErrors);
            }

            return await _service.RevokeTicketAsync(request.BookedTicketId, request.TicketCode, request.BookedTicketDetailsQuantity);
        }
    }
}
