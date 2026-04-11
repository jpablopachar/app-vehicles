using Domain.Rentals;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de alquileres de vehículos.
/// Proporciona operaciones de acceso a datos para la entidad Rental.
/// </summary>
internal sealed class RentalRepository(AppVehiclesDbContext dbContext) : Repository<Rental, RentalId>(dbContext), IRentalRepository
{
    /// <summary>
    /// Estados de alquiler que se consideran activos.
    /// Incluye: Reservado, Confirmado y Completado.
    /// </summary>
    private static readonly RentalStatus[] ActiveRentalStatuses = {
        RentalStatus.Reserved,
        RentalStatus.Confirmed,
        RentalStatus.Completed
    };

    /// <summary>
    /// Determina si existe un conflicto de disponibilidad para un vehículo en el rango de fechas especificado.
    /// Verifica si el vehículo tiene alquileres activos que se superponen con el período indicado.
    /// </summary>
    /// <param name="vehicle">El vehículo a verificar.</param>
    /// <param name="duration">El rango de fechas a validar.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>
    /// Devuelve true si existe un conflicto de disponibilidad (alquileres superpuestos), false en caso contrario.
    /// </returns>
    public async Task<bool> IsOverlapping(
        Vehicle vehicle,
        DateRange duration,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Rental>()
        .AnyAsync(
            rental =>
                rental.VehicleId == vehicle.Id &&
                rental.Duration!.Start <= duration.End &&
                rental.Duration.End >= duration.Start &&
                ActiveRentalStatuses.Contains(rental.Status),
                cancellationToken
        );
    }
}
