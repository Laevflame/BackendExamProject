using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProject.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators.Distinct().ToList(); // Ensure distinct validators
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = new HashSet<string>(); // Use HashSet to prevent duplicate error messages

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                foreach (var error in result.Errors)
                {
                    failures.Add($"{error.PropertyName}: {error.ErrorMessage}"); // Store only unique errors
                }
            }

            if (failures.Count > 0)
            {
                throw new ValidationException(failures.Select(e => new FluentValidation.Results.ValidationFailure(
                    e.Split(": ")[0], e.Split(": ")[1])).ToList());
            }

            return await next();
        }
    }
}
