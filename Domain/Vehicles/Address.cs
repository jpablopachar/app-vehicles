namespace Domain.Vehicles;

/// <summary>
/// Representa la dirección de un vehículo con información geográfica.
/// </summary>
/// <param name="Country">País donde se ubica la dirección.</param>
/// <param name="Department">Departamento o región de la dirección.</param>
/// <param name="Province">Provincia de la dirección.</param>
/// <param name="City">Ciudad de la dirección.</param>
/// <param name="Street">Calle o vía de la dirección.</param>
public record Address(string Country, string Department, string Province, string City, string Street);
