using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;

namespace Application.Users.GetUsersDapperPaginationQuery;

/// <summary>
/// Consulta para obtener un listado paginado de usuarios utilizando Dapper.
/// </summary>
/// <remarks>
/// Este query hereda de <see cref="PaginationParams"/> para recibir parámetros de paginación
/// y devuelve resultados paginados de usuarios obtenidos mediante Dapper.
/// </remarks>
public sealed record GetUsersDapperPaginationQuery 
: PaginationParams, IQuery<PagedDapperResults<UserPaginationData>>;
