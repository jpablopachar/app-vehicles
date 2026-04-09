using Application.Abstractions.Messaging;
using Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Abstractions.Behaviors;

/// <summary>
/// Comportamiento de validación que intercepta las solicitudes en la canalización de MediatR
/// y ejecuta la validación antes de procesar la solicitud.
/// </summary>
/// <typeparam name="TRequest">El tipo de solicitud que debe ser validado.</typeparam>
/// <typeparam name="TResponse">El tipo de respuesta retornada por el manejador.</typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    /// <summary>
    /// Maneja la solicitud, validándola antes de procesarla con el siguiente manejador.
    /// </summary>
    /// <param name="request">La solicitud a validar y procesar.</param>
    /// <param name="next">El delegado que representa el siguiente paso en la canalización.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>La respuesta del siguiente manejador si la validación es exitosa.</returns>
    /// <exception cref="Exceptions.ValidationException">Se lanza cuando la validación falla.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        if (!_validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
        .Select(validators => validators.Validate(context))
        .Where(validationResult => validationResult.Errors.Any())
        .SelectMany(validationResult => validationResult.Errors)
        .Select(validationFailure => new ValidationError(
            validationFailure.PropertyName,
            validationFailure.ErrorMessage
        )).ToList();

        if (validationErrors.Count != 0) throw new Exceptions.ValidationException(validationErrors);

        return await next(cancellationToken);
    }
}
