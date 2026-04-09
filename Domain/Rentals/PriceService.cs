using Domain.Shared;
using Domain.Vehicles;

namespace Domain.Rentals;

/// <summary>
/// Servicio encargado de calcular el precio de alquiler de vehículos.
/// </summary>
public class PriceService
{
    /// <summary>
    /// Calcula el precio total de alquiler de un vehículo considerando el período, accesorios y mantenimiento.
    /// </summary>
    /// <param name="vehicle">El vehículo para el cual se calcula el precio.</param>
    /// <param name="period">El período de alquiler expresado en rango de fechas.</param>
    /// <returns>Un objeto PriceDetail con el desglose del precio calculado.</returns>
    public PriceDetail CalculatePrice(Vehicle vehicle, DateRange period)
    {
        var currencyType = vehicle.Price!.CurrencyType;
        var pricePerPeriod = new Currency(period.DaysQuantity * vehicle.Price.Amount, currencyType);

        decimal percentageChange = 0;

        foreach (var accessory in vehicle.Accessories)
        {
            percentageChange += accessory switch
            {
                Accessory.AppleCarPlay => 0.05m,
                Accessory.AirConditioning => 0.01m,
                Accessory.Maps => 0.01m,
                _ => 0
            };
        }

        var accessoriesCharges = Currency.Zero(currencyType);

        if (percentageChange > 0)
        {
            accessoriesCharges = new Currency(pricePerPeriod.Amount * percentageChange, currencyType);
        }

        var totalPrice = Currency.Zero(currencyType);

        totalPrice += pricePerPeriod;

        if (!vehicle.Maintenance!.IsZero())
        {
            totalPrice += vehicle.Maintenance;
        }

        totalPrice += accessoriesCharges;

        return new PriceDetail(pricePerPeriod, vehicle.Maintenance, accessoriesCharges, totalPrice);
    }
}
