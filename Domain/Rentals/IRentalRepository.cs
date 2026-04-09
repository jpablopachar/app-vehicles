using Domain.Vehicles;

namespace Domain.Rentals;

/// <summary>
/// Repositorio para la gestión de alquileres de vehículos.
/// </summary>
public interface IRentalRepository
{
    /// <summary>
    /// Obtiene un alquiler por su identificador.
    /// </summary>
    /// <param name="id">El identificador del alquiler a obtener.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>El alquiler encontrado, o nulo si no existe.</returns>
    Task<Rental?> GetById(RentalId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existen alquileres que se solapan con el período de duración especificado para un vehículo.
    /// </summary>
    /// <param name="vehicle">El vehículo para el cual se verifica el solapamiento.</param>
    /// <param name="duration">El rango de fechas para el cual se verifica el solapamiento.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Verdadero si existe solapamiento, falso en caso contrario.</returns>
    Task<bool> IsOverlapping(
        Vehicle vehicle,
        DateRange duration,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Añade un nuevo alquiler al repositorio.
    /// </summary>
    /// <param name="rental">El alquiler a añadir.</param>
    void Add(Rental rental);
}
