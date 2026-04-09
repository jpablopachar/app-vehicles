using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;

namespace Application.Users.GetUsersPagination;

/// <summary>
/// Consulta para obtener usuarios con paginación.
/// </summary>
/// <remarks>
/// Este record encapsula los parámetros de paginación y devuelve un conjunto de resultados paginados de usuarios.
/// </remarks>
public record GetUsersPaginationQuery : PaginationParams, IQuery<PagedResults<User, UserId>> { }
