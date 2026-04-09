using System.Text;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;

namespace Application.Users.GetUsersDapperPaginationQuery;

/// <summary>
/// Manejador para la consulta de obtención de usuarios con paginación usando Dapper.
/// </summary>
/// <remarks>
/// Este manejador procesa las solicitudes de obtención de usuarios con soporte para
/// búsqueda, ordenamiento y paginación mediante consultas SQL con Dapper.
/// </remarks>
internal sealed class GetUsersDapperPaginationQueryHandler(ISqlConnectionFactory sqlConnectionFactory) :
IQueryHandler<GetUsersDapperPaginationQuery, PagedDapperResults<UserPaginationData>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Maneja la solicitud de obtención de usuarios con paginación.
    /// </summary>
    /// <param name="request">La solicitud que contiene los parámetros de búsqueda, ordenamiento y paginación.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Un resultado que contiene los usuarios paginados o un error.</returns>
    public async Task<Result<PagedDapperResults<UserPaginationData>>> Handle(
        GetUsersDapperPaginationQuery request, CancellationToken cancellationToken
        )
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var builder = new StringBuilder("""
                SELECT 
                    usr.email, rl.name as role, p.nombre as permiso
                FROM users usr
                    LEFT JOIN users_roles usrl
                        ON usr.id=usrl.user_id
                    LEFT JOIN roles rl
                        ON rl.id=usrl.role_id
                    LEFT JOIN roles_permissions rp
                        ON rl.id=rp.role_id
                    LEFT JOIN permissions p
                        ON p.id=rp.permission_id
        """);

        var search = string.Empty;
        var whereStatement = string.Empty;

        if (!string.IsNullOrEmpty(request.Search))
        {
            search = "%" + EncodeForLike(request.Search) + "%";
            whereStatement = $" WHERE rl.name LIKE @Search ";

            builder.AppendLine(whereStatement);
        }

        var orderBy = request.OrderBy;

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderStatement = string.Empty;
            var orderAsc = request.OrderAsc ? "ASC" : "DESC";

            orderStatement = orderBy switch
            {
                "email" => $" ORDER BY usr.email {orderAsc}",
                "role" => $" ORDER BY rl.name {orderAsc}",
                _ => $" ORDER BY rl.name {orderAsc}",
            };

            builder.AppendLine(orderStatement);
        }

        builder.AppendLine(" LIMIT @PageSize OFFSET @Offset;");

        builder.AppendLine("""
                    SELECT 
                        COUNT(*)
                    FROM users usr
                        LEFT JOIN users_roles usrl
                            ON usr.id=usrl.user_id
                        LEFT JOIN roles rl
                            ON rl.id=usrl.role_id
                        LEFT JOIN roles_permissions rp
                            ON rl.id=rp.role_id
                        LEFT JOIN permissions p
                            ON p.id=rp.permission_id
            """);

        builder.AppendLine(whereStatement);
        builder.AppendLine(";");

        var offset = request.PageSize * (request.PageNumber - 1);
        var sql = builder.ToString();

        using var multi = await connection.QueryMultipleAsync(sql,
            new
            {
                PageSize = request.PageSize,
                Offset = offset,
                Search = search
            }
        );

        var items = await multi.ReadAsync<UserPaginationData>().ConfigureAwait(false);

        var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

        var result = new PagedDapperResults<UserPaginationData>(totalItems, request.PageNumber, request.PageSize)
        {
            Items = items
        };

        return result;
    }

    /// <summary>
    /// Codifica una cadena de búsqueda para su uso seguro en cláusulas LIKE de SQL.
    /// </summary>
    /// <remarks>
    /// Este método escapa los caracteres especiales [ y % para evitar problemas de sintaxis
    /// en consultas SQL con cláusulas LIKE. Los caracteres [ se escapan a [[] y % a [%].
    /// </remarks>
    /// <param name="search">La cadena de búsqueda a codificar.</param>
    /// <returns>La cadena de búsqueda codificada y lista para usarse en una cláusula LIKE.</returns>
    private static string EncodeForLike(string search)
    {
        return search.Replace("[", "[]]").Replace("%", "[%]");
    }
}
