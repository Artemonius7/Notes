using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using FluentValidation;
namespace Notes.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest,TResponse> where TRequest: IRequest <TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator) => _validator = validator;
        

        public Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            // сбор ошибок
            var failures = _validator
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();
            if (failures.Count !=0)
            {
                throw new ValidationException(failures);
            }
            return next();
        }
    }
}
