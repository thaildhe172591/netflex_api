﻿using Netflex.Shared.CQRS;
using FluentValidation;
using MediatR;

namespace Netflex.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures =
            validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .GroupBy(
                r => r.PropertyName,
                r => r.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(r => r.Key, r => r.Values);

        if (failures.Count != 0)
            throw new Exceptions.ValidationException(failures);

        return await next();
    }
}
