using Domain.Abstractions;
using Domain.Shared;

namespace Domain.Vehicles;

/// <summary>
/// Representa un vehículo en el sistema.
/// Entidad que encapsula la información completa de un vehículo incluyendo modelo, VIN, dirección, precio y mantenimiento.
/// </summary>
public sealed class Vehicle : Entity<VehicleId>
{
    /// <summary>
    /// Obtiene o establece el modelo del vehículo.
    /// </summary>
    public Model? Model { get; set; }

    /// <summary>
    /// Obtiene el número de identificación del vehículo (VIN).
    /// </summary>
    public Vin? Vin { get; private set; }

    /// <summary>
    /// Obtiene la dirección actual del vehículo.
    /// </summary>
    public Address? Address { get; private set; }

    /// <summary>
    /// Obtiene el precio del vehículo.
    /// </summary>
    public Currency? Price { get; private set; }

    /// <summary>
    /// Obtiene el costo de mantenimiento del vehículo.
    /// </summary>
    public Currency? Maintenance { get; private set; }

    /// <summary>
    /// Obtiene o establece la fecha del último alquiler del vehículo.
    /// </summary>
    public DateTime LastRentalDate { get; internal set; }

    /// <summary>
    /// Obtiene la lista de accesorios del vehículo.
    /// </summary>
    public List<Accessory> Accessories { get; private set; } = new();

    /// <summary>
    /// Retorna la representación en texto del vehículo usando su modelo.
    /// </summary>
    /// <returns>El valor del modelo del vehículo.</returns>
    public override string ToString()
    {
        return Model!.Value;
    }

    /// <summary>
    /// Constructor privado para la deserialización.
    /// </summary>
    private Vehicle() { }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Vehicle"/>.
    /// </summary>
    /// <param name="id">El identificador único del vehículo.</param>
    /// <param name="model">El modelo del vehículo.</param>
    /// <param name="vin">El número de identificación del vehículo (VIN).</param>
    /// <param name="address">La dirección del vehículo.</param>
    /// <param name="price">El precio del vehículo.</param>
    /// <param name="maintenance">El costo de mantenimiento del vehículo.</param>
    /// <param name="lastRentalDate">La fecha del último alquiler del vehículo.</param>
    /// <param name="accessories">La lista de accesorios del vehículo.</param>
    public Vehicle(VehicleId id, Model model, Vin vin, Address address, Currency price, Currency maintenance, DateTime lastRentalDate, List<Accessory> accessories)
        : base(id)
    {
        Model = model;
        Vin = vin;
        Address = address;
        Price = price;
        Maintenance = maintenance;
        LastRentalDate = lastRentalDate;
        Accessories = accessories;
    }
}
