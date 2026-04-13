using System.Net;
using Api.Utils;
using Application.Users.GetUsersDapperPaginationQuery;
using Application.Users.GetUserSession;
using Application.Users.GetUsersPagination;
using Application.Users.LoginUser;
using Application.Users.RegisterUser;
using Asp.Versioning;
using Domain.Abstractions;
using Domain.Permissions;
using Domain.Users;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

/// <summary>
/// Controlador de usuarios que gestiona las operaciones relacionadas con usuarios.
/// </summary>
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Obtiene la información del usuario actual autenticado.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Datos del usuario autenticado.</returns>
    [HttpGet("me")]
    [HasPermission(PermissionEnum.ReadUser)]
    public async Task<IActionResult> GetUserMe(CancellationToken cancellationToken)
    {
        var query = new GetUserSessionQuery();
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    /// <summary>
    /// Autentica un usuario con sus credenciales de email y contraseña.
    /// </summary>
    /// <param name="request">Solicitud de login que contiene email y contraseña.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Token de autenticación si el login es exitoso, de lo contrario error 401.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [MapToApiVersion(ApiVersions.V1)]
    public async Task<IActionResult> LoginV1(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) return Unauthorized(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request">Solicitud de registro que contiene email, nombre, apellido y contraseña.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Datos del usuario registrado si el registro es exitoso, de lo contrario error 401.</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Name,
            request.Lastname,
            request.Password
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) return Unauthorized(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una lista paginada de usuarios.
    /// </summary>
    /// <param name="paginationQuery">Parámetros de consulta para la paginación (página, tamaño, etc).</param>
    /// <returns>Resultados paginados de usuarios con información de paginación.</returns>
    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationUsers")]
    [ProducesResponseType(typeof(PagedResults<User, UserId>), (int)HttpStatusCode.OK
    )]
    public async Task<ActionResult<PagedResults<User, UserId>>> GetPagination(
        [FromQuery] GetUsersPaginationQuery paginationQuery
    )
    {
        var result = await _sender.Send(paginationQuery);

        return Ok(result);
    }

    /// <summary>
    /// Obtiene una lista paginada de usuarios utilizando Dapper como acceso a datos.
    /// </summary>
    /// <param name="paginationQuery">Parámetros de consulta para la paginación (página, tamaño, etc).</param>
    /// <returns>Resultados paginados de usuarios obtenidos mediante Dapper.</returns>
    [AllowAnonymous]
    [HttpGet("getPaginationDapper", Name = "GetPaginationDapper")]
    [ProducesResponseType(typeof(PagedDapperResults<UserPaginationData>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedDapperResults<UserPaginationData>>> GetPaginationDapper
    (
        [FromQuery] GetUsersDapperPaginationQuery paginationQuery
    )
    {
        var result = await _sender.Send(paginationQuery);

        return Ok(result);
    }
}
