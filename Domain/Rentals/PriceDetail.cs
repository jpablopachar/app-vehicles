using Domain.Shared;

namespace Domain.Rentals;

/// <summary>
/// Representa los detalles de precios de un alquiler.
/// </summary>
/// <param name="PricePerPeriod">Precio por período de alquiler.</param>
/// <param name="Maintenance">Costo de mantenimiento del vehículo.</param>
/// <param name="Accessories">Costo de accesorios adicionales.</param>
/// <param name="TotalPrice">Precio total del alquiler.</param>
public record PriceDetail(Currency PricePerPeriod, Currency Maintenance, Currency Accessories, Currency TotalPrice);
