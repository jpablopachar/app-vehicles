using Application.Abstractions.Messaging;
using Application.Paginations;
using Domain.Abstractions;
using Domain.Users;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetUsersPagination;

/// <summary>
/// Manejador de consultas para obtener usuarios con paginación.
/// </summary>
/// <remarks>
/// Este manejador procesa consultas de obtención de usuarios aplicando filtros de búsqueda
/// y retorna los resultados paginados con sus roles y permisos asociados.
/// </remarks>
internal sealed class GetUsersPaginationQueryHandler(IPaginationRepository paginationRepository)
: IQueryHandler<GetUsersPaginationQuery, PagedResults<User, UserId>>
{
    private readonly IPaginationRepository _paginationRepository = paginationRepository;

    /// <summary>
    /// Maneja la consulta de paginación de usuarios.
    /// </summary>
    /// <param name="request">La solicitud de paginación que contiene criterios de búsqueda y parámetros de paginación.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Un resultado que contiene los usuarios paginados con sus roles y permisos, o un error si la operación falla.</returns>
    public async Task<Result<PagedResults<User, UserId>>> Handle(
        GetUsersPaginationQuery request,
        CancellationToken cancellationToken
        )
    {
        var predicateBuilder = PredicateBuilder.New<User>(true);

        if (!string.IsNullOrEmpty(request.Search))
        {
            predicateBuilder = predicateBuilder.Or(p => p.Name == new Name(request.Search));
            predicateBuilder = predicateBuilder.Or(p => p.Email == new Email(request.Search));
        }

        var pagedResultUsuarios = await _paginationRepository.GetPagination(
            predicateBuilder,
            p => p.Include(x => x.Roles!).ThenInclude(y => y.Permissions!),
            request.PageNumber,
            request.PageSize,
            request.OrderBy!,
            request.OrderAsc
        );

        return pagedResultUsuarios;
    }
}
