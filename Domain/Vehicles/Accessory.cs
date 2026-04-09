namespace Domain.Vehicles;

/// <summary>
/// Enumeración que representa los accesorios disponibles en un vehículo.
/// </summary>
public enum Accessory
{
    /// <summary>
    /// Conectividad WiFi del vehículo.
    /// </summary>
    Wifi = 1,

    /// <summary>
    /// Sistema de aire acondicionado.
    /// </summary>
    AirConditioning = 2,

    /// <summary>
    /// Integración con Apple CarPlay.
    /// </summary>
    AppleCarPlay = 3,

    /// <summary>
    /// Integración con Android Auto.
    /// </summary>
    AndroidAuto = 4,

    /// <summary>
    /// Sistema de navegación GPS integrado.
    /// </summary>
    Maps = 5,
}
