using Application.Abstractions.Messaging;

namespace Application.Vehicles.SearchVehicles;

/// <summary>
/// Consulta para buscar vehículos dentro de un rango de fechas específico.
/// </summary>
/// <param name="StartDate">Fecha de inicio del rango de búsqueda.</param>
/// <param name="EndDate">Fecha de finalización del rango de búsqueda.</param>
public sealed record SearchVehiclesQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<IReadOnlyList<VehicleResponse>>;
