using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

/// <summary>
/// Interfaz que define un comando que retorna un resultado.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand { }

/// <summary>
/// Interfaz genérica que define un comando que retorna un resultado con una respuesta específica.
/// </summary>
/// <typeparam name="TResponse">El tipo de respuesta que retorna el comando.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand { }

/// <summary>
/// Interfaz base que marca un tipo como comando.
/// </summary>
public interface IBaseCommand { }
