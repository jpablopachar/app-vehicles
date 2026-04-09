using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

/// <summary>
/// Define un manejador de comandos que procesa comandos sin valor de retorno.
/// </summary>
/// <typeparam name="TCommand">El tipo de comando a manejar.</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand { }

/// <summary>
/// Define un manejador de comandos que procesa comandos con valor de retorno específico.
/// </summary>
/// <typeparam name="TCommand">El tipo de comando a manejar.</typeparam>
/// <typeparam name="TResponse">El tipo de respuesta devuelto por el manejador.</typeparam>
public interface ICommandHandler<TCommand, TResponse>
: IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{ }
