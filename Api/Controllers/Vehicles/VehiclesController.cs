using System.Net;
using Api.Utils;
using Application.Vehicles.GetVehiclesByPagination;
using Application.Vehicles.GetVehiclesKitByPagination;
using Application.Vehicles.SearchVehicles;
using Application.Vehicles.VehicleReportPdf;
using Asp.Versioning;
using Domain.Abstractions;
using Domain.Permissions;
using Domain.Vehicles;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace Api.Controllers.Vehicles;

/// <summary>
/// Controlador para gestionar las operaciones relacionadas con vehículos.
/// Proporciona endpoints para reportes, búsquedas, paginación y gestión de kits de vehículos.
/// </summary>
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/vehicles")]
public class VehiclesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Genera un reporte en PDF de vehículos filtrados por modelo.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <param name="model">Modelo del vehículo a filtrar en el reporte. Opcional.</param>
    /// <returns>Un archivo PDF con el reporte de vehículos.</returns>
    [AllowAnonymous]
    [HttpGet("report")]
    public async Task<IActionResult> ReportVehicles(
        CancellationToken cancellationToken,
        string model = ""
    )
    {
        var query = new VehicleReportPdfQuery(model);
        var result = await _sender.Send(query, cancellationToken);

        byte[] pdfBytes = result.Value.GeneratePdf();

        return File(pdfBytes, "application/pdf");
    }

    /// <summary>
    /// Busca vehículos dentro de un rango de fechas especificado.
    /// Requiere permiso de lectura de usuario.
    /// </summary>
    /// <param name="startDate">Fecha de inicio del rango de búsqueda.</param>
    /// <param name="endDate">Fecha de fin del rango de búsqueda.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una colección de vehículos que coinciden con los criterios de búsqueda.</returns>
    [HasPermission(PermissionEnum.ReadUser)]
    [HttpGet("search")]
    public async Task<IActionResult> SearchVehicles(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken
    )
    {
        var query = new SearchVehiclesQuery(startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    /// <summary>
    /// Obtiene una lista paginada de vehículos.
    /// </summary>
    /// <param name="request">Parámetros de paginación y filtrado de vehículos.</param>
    /// <returns>Un resultado paginado con la lista de vehículos solicitados.</returns>
    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationVehicles")]
    [ProducesResponseType(typeof(PaginationResult<Vehicle, VehicleId>),
    (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationResult<Vehicle, VehicleId>>> GetPaginationVehicle(
        [FromQuery] GetVehiclesByPaginationQuery request
    )
    {
        var result = await _sender.Send(request);

        return Ok(result);
    }

    /// <summary>
    /// Obtiene una lista paginada de kits de vehículos.
    /// </summary>
    /// <param name="paginationQuery">Parámetros de paginación y filtrado de kits de vehículos.</param>
    /// <returns>Un resultado paginado con la lista de kits de vehículos solicitados.</returns>
    [AllowAnonymous]
    [HttpGet("getPaginationKit", Name = "PaginationVehicleKit")]
    [ProducesResponseType(typeof(PaginationResult<Vehicle, VehicleId>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResults<Vehicle, VehicleId>>> GetPaginationVehicleKit(
        [FromQuery] GetVehiclesKitByPaginationQuery paginationQuery
    )
    {
        var result = await _sender.Send(paginationQuery);

        return Ok(result);
    }
}
