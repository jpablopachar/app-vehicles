using Application.Abstractions.Messaging;
using QuestPDF.Fluent;

namespace Application.Vehicles.VehicleReportPdf;

/// <summary>
/// Consulta para generar un reporte PDF de vehículos.
/// </summary>
/// <param name="Model">El modelo del vehículo para filtrar el reporte.</param>
public sealed record VehicleReportPdfQuery(string Model) : IQuery<Document>;
