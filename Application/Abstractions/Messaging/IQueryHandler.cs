using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

/// <summary>
/// Define un manejador para las consultas del dominio.
/// </summary>
/// <typeparam name="TQuery">El tipo de consulta que implementa <see cref="IQuery{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">El tipo de respuesta que retorna la consulta.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse> { }
