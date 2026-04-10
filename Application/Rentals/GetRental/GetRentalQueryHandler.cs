using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;

namespace Application.Rentals.GetRental;

/// <summary>
/// Controlador de consulta para obtener los detalles de un alquiler específico.
/// </summary>
internal sealed class GetRentalQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetRentalQuery, RentalResponse>
{
    /// <summary>
    /// Fábrica de conexiones SQL para acceder a la base de datos.
    /// </summary>
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Maneja la consulta para obtener los detalles de un alquiler.
    /// </summary>
    /// <param name="request">La consulta con el identificador del alquiler a recuperar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Un resultado que contiene la información del alquiler si se encuentra; de lo contrario, nulo.</returns>
    public async Task<Result<RentalResponse>> Handle(
        GetRentalQuery request,
        CancellationToken cancellationToken
    )
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT
                id AS Id,
                vehiculo_id AS VehiculoId,
                user_id AS UserId,
                status AS Status,
                precio_por_periodo_monto AS PrecioAlquiler,
                precio_por_periodo_tipo_moneda AS TipoMonedaAlquiler,
                mantenimiento_monto AS PrecioMantenimiento,
                mantenimiento_tipo_moneda AS TipoMonedaMantenimiento,
                accesorios_monto AS AccesoriosPrecio,
                accesorios_tipo_moneda AS TipoMonedaAccesorio,
                precio_total_monto AS PrecioTotal,
                precio_total_tipo_moneda AS PrecioTotalTipoMoneda,
                duracion_inicio AS DuracionInicio,
                duracion_fin AS DuracionFinal,
                fecha_creacion AS FechaCreacion
            FROM alquileres WHERE id=@AlquilerId  
        """;

        var rental = await connection.QueryFirstOrDefaultAsync<RentalResponse>(
            sql,
            new
            {
                request.RentalId
            }
        );

        return rental!;
    }
}
