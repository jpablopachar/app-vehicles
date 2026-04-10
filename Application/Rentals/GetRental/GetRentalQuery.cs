using Application.Abstractions.Messaging;

namespace Application.Rentals.GetRental;

/// <summary>
/// Consulta para obtener los detalles de un alquiler específico.
/// </summary>
/// <param name="RentalId">El identificador único del alquiler a recuperar.</param>
public sealed record GetRentalQuery(Guid RentalId) : IQuery<RentalResponse>;
