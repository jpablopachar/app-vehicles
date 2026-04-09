using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

/// <summary>
/// Define un contrato para las consultas de la aplicación.
/// </summary>
/// <typeparam name="TResponse">El tipo de respuesta que retorna la consulta.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
