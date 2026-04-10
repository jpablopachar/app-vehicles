using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;
using Domain.Rentals;

namespace Application.Vehicles.SearchVehicles;

/// <summary>
/// Manejador de consulta para la búsqueda de vehículos disponibles en un rango de fechas.
/// </summary>
/// <remarks>
/// Este manejador ejecuta una consulta SQL que obtiene los vehículos que no tienen alquileres
/// activos (reservados, confirmados o completados) dentro del período especificado.
/// </remarks>
internal sealed class SearchVehiclesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
: IQueryHandler<SearchVehiclesQuery, IReadOnlyList<VehicleResponse>>
{
    /// <summary>
    /// Matriz de estados de alquiler considerados como activos.
    /// </summary>
    private static readonly int[] ActiveAlquilerStatuses =
    [
        (int)RentalStatus.Reserved,
        (int)RentalStatus.Confirmed,
        (int)RentalStatus.Completed
    ];

    /// <summary>
    /// Fábrica para crear conexiones SQL.
    /// </summary>
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Procesa la consulta de búsqueda de vehículos disponibles.
    /// </summary>
    /// <param name="request">La consulta con las fechas de inicio y fin del alquiler.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Una lista de respuestas de vehículos disponibles en el período especificado.</returns>
    public async Task<Result<IReadOnlyList<VehicleResponse>>> Handle(
        SearchVehiclesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.StartDate > request.EndDate) return new List<VehicleResponse>();

        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
               SELECT
                a.id as Id,
                a.modelo as Modelo,
                a.vin as Vin,
                a.precio_monto as Precio,
                a.precio_tipo_moneda as TipoMoneda,
                a.direccion_pais as Pais,
                a.direccion_departamento as Departamento,
                a.direccion_provincia as Provincia,
                a.direccion_ciudad as Ciudad,
                a.direccion_calle as Calle
             FROM vehiculos AS a
             WHERE NOT EXISTS
             (
                    SELECT 1 
                    FROM alquileres AS b
                    WHERE 
                        b.vehiculo_id = a.id  AND
                        b.duracion_inicio <= @EndDate AND
                        b.duracion_fin  >= @StartDate AND
                        b.status = ANY(@ActiveAlquilerStatuses)
             )
        """;


        var vehicles = await connection
            .QueryAsync<VehicleResponse, AddressResponse, VehicleResponse>
            (
                sql,
                (vehicle, address) =>
                {
                    vehicle.Address = address;

                    return vehicle;
                },
                new
                {
                    request.StartDate,
                    request.EndDate,
                    ActiveAlquilerStatuses
                },
                splitOn: "Country"
            );

        return vehicles.ToList();
    }
}
