using Application.Abstractions.Messaging;

namespace Application.Users.GetUserSession;

/// <summary>
/// Consulta para obtener la sesión actual del usuario.
/// </summary>
public sealed record GetUserSessionQuery : IQuery<UserResponse>;
