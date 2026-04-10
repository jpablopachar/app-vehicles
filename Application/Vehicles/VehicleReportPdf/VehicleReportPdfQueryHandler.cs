using System.Text;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Vehicles.SearchVehicles;
using Dapper;
using Domain.Abstractions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Vehicles.VehicleReportPdf;

/// <summary>
/// Manejador de consulta para generar reportes de vehículos en formato PDF.
/// </summary>
/// <remarks>
/// Esta clase es responsable de procesar una consulta de reporte de vehículos,
/// consultar los datos de la base de datos y generar un documento PDF con formato.
/// </remarks>
internal sealed class VehicleReportPdfQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
: IQueryHandler<VehicleReportPdfQuery, Document>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Procesa la solicitud de reporte PDF de vehículos.
    /// </summary>
    /// <param name="request">Solicitud que contiene los parámetros de filtrado para el reporte.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Un resultado que contiene el documento PDF generado con la lista de vehículos.</returns>
    /// <remarks>
    /// El método construye una consulta SQL dinámica basada en el modelo de vehículo proporcionado,
    /// recupera los datos de la base de datos y genera un documento PDF con una tabla formateada
    /// que incluye el modelo, VIN y precio de cada vehículo.
    /// </remarks>
    public async Task<Result<Document>> Handle(VehicleReportPdfQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var builder = new StringBuilder("""
            SELECT
                v.id as Id,
                v.model as Model,
                v.vin as Vin,
                v.price_amount as Price
            FROM vehicles AS v
        """);


        var search = string.Empty;
        var where = string.Empty;

        if (!string.IsNullOrEmpty(request.Model))
        {
            search = "%" + request.Model + "%";
            where = $" WHERE v.model LIKE @Search";
            builder.AppendLine(where);
        }

        builder.AppendLine(" ORDER BY v.model ");


        var vehicles = await connection.QueryAsync<VehicleResponse>(
            builder.ToString(),
            new
            {
                Search = search
            }
        );

        var report = Document.Create(container =>
        {

            container.Page(page =>
            {
                page.Margin(50);
                page.Size(PageSizes.A4.Landscape());
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .AlignCenter()
                    .Text("Vehículos: Modernos de Alta Gama")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                page.Content().Padding(25)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Modelo");
                            header.Cell().Element(CellStyle).Text("Vin");
                            header.Cell().Element(CellStyle).AlignRight().Text("Precio");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .DefaultTextStyle(
                                            x => x.SemiBold())
                                            .PaddingVertical(5)
                                            .BorderBottom(1)
                                            .BorderColor(Colors.Black);
                            }

                        });

                        foreach (var vehicle in vehicles)
                        {
                            table.Cell().Element(CellStyle).Text(vehicle.Model);
                            table.Cell().Element(CellStyle).Text(vehicle.Vin);
                            table.Cell().Element(CellStyle).AlignRight().Text($"${vehicle.Price}");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(5);
                            }
                        }
                    });
            });
        });

        return report;
    }
}
