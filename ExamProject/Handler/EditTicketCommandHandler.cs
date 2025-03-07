using ExamProject.Commands;
using ExamProject.Models;
using ExamProject.Services;
using FluentValidation;
using MediatR;

namespace ExamProject.Handler
{
    public class EditTicketCommandHandler : IRequestHandler<EditTicketCommand, EditTicketResponse>
    {
        private readonly EditTicketServices _service;
        private readonly IValidator<(string, EditTicketRequest)> _validator;

        public EditTicketCommandHandler(EditTicketServices service, IValidator<(string, EditTicketRequest)> validator)
        {
            _service = service;
            _validator = validator;
        }

        public async Task<EditTicketResponse> Handle(EditTicketCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync((request.BookedTicketId, request.EditRequest), cancellationToken);
            if (!validationResult.IsValid)
            {
                var distinctErrors = validationResult.Errors
                    .GroupBy(e => new { e.PropertyName, e.ErrorMessage })
                    .Select(g => g.First())
                    .ToList();

                throw new ValidationException(distinctErrors);
            }


            return await _service.EditTicketBookedTicketAsync(request.BookedTicketId, request.EditRequest);
        }
    }
}
