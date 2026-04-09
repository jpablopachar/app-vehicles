using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;

namespace Application.Users.GetUserSession;

/// <summary>
/// Controlador para manejar la consulta de obtención de la sesión del usuario.
/// Recupera la información del usuario autenticado desde la base de datos.
/// </summary>
internal sealed class GetUserSessionQueryHandler(
    IUserContext userContext,
    ISqlConnectionFactory sqlConnectionFactory)
: IQueryHandler<GetUserSessionQuery, UserResponse>
{
    private readonly IUserContext _userContext = userContext;

    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Maneja la consulta para obtener la información de la sesión del usuario actual.
    /// </summary>
    /// <param name="request">La consulta de obtención de sesión del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Un resultado que contiene la información del usuario o un error.</returns>
    public async Task<Result<UserResponse>> Handle(
        GetUserSessionQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id as Id,
                name as Name,
                lastname as Lastname,
                email as Email
            FROM users
            WHERE email = @email
        """;

        var user = await connection.QuerySingleAsync<UserResponse>(
            sql,
            new
            {
                email = _userContext.UserEmail
            }
        );

        return user;
    }
}
