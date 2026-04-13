using Application.Rentals.GetRental;
using Application.Rentals.ReserveRental;
using Domain.Permissions;
using MediatR;

namespace Api.Controllers.Rentals;

/// <summary>
/// Clase que contiene los puntos finales (endpoints) para la gestión de alquileres.
/// Proporciona operaciones para obtener y reservar alquileres de vehículos.
/// </summary>
public static class RentalsEndpoints
{
    /// <summary>
    /// Mapea los endpoints de alquileres en el constructor de rutas de la aplicación.
    /// </summary>
    /// <param name="builder">Constructor de rutas de puntos finales.</param>
    /// <returns>El constructor de rutas configurado con los endpoints de alquileres.</returns>
    public static IEndpointRouteBuilder MapRentalsEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        builder
        .MapGet("rentals/{id}", GetRental)
        .RequireAuthorization(PermissionEnum.ReadUser.ToString())
        .WithName(nameof(GetRental));

        builder
        .MapPost("rentals", ReserveRental)
        .RequireAuthorization(PermissionEnum.WriteUser.ToString());

        return builder;
    }

    /// <summary>
    /// Obtiene los detalles de un alquiler específico por identificador.
    /// </summary>
    /// <param name="id">Identificador único del alquiler.</param>
    /// <param name="sender">Mediador para enviar consultas.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Resultado con los datos del alquiler si existe, o NotFound si no se encuentra.</returns>
    public static async Task<IResult> GetRental(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken
    )
    {
        var query = new GetRentalQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
    }

    /// <summary>
    /// Reserva un nuevo alquiler de vehículo para un usuario específico.
    /// </summary>
    /// <param name="sender">Mediador para enviar comandos.</param>
    /// <param name="request">Solicitud de reserva con los datos del alquiler.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Resultado con los datos del alquiler creado, o error si la operación falla.</returns>
    public static async Task<IResult> ReserveRental(
        ISender sender,
        RentalReserveRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new ReserveRentalCommand
        (
            request.VehicleId,
            request.UserId,
            request.StartDate,
            request.EndDate
        );

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute(nameof(GetRental), new { id = result.Value }, result.Value);
    }
}
