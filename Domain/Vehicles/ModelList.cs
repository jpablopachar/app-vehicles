namespace Domain.Vehicles;

/// <summary>
/// Representa una lista de modelos de vehículos.
/// </summary>
public class ModelList : List<string>
{
    /// <summary>
    /// Convierte explícitamente una lista de modelos a una cadena de texto.
    /// </summary>
    /// <param name="models">La lista de modelos a convertir.</param>
    /// <returns>Una cadena de texto que representa los modelos.</returns>
    /// <exception cref="NotImplementedException">Este operador no está implementado.</exception>
    public static explicit operator string(ModelList models)
    {
        throw new NotImplementedException();
    }
}
