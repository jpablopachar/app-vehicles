namespace Domain.Shared;

/// <summary>
/// Clase base para enumeraciones fuertemente tipadas.
/// </summary>
/// <typeparam name="TEnum">Tipo de la enumeración.</typeparam>
public abstract class Enumeration<TEnum>(int id, string name) : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    /// <summary>
    /// Identificador de la enumeración.
    /// </summary>
    public int Id { get; protected init; } = id;

    /// <summary>
    /// Nombre de la enumeración.
    /// </summary>
    public string Name { get; protected init; } = name;

    /// <summary>
    /// Obtiene una instancia de la enumeración a partir de su identificador.
    /// </summary>
    /// <param name="id">Identificador de la enumeración.</param>
    /// <returns>Instancia de la enumeración o null si no existe.</returns>
    public static TEnum? FromValue(int id)
    {
        return Enumerations.TryGetValue(id, out TEnum? enumeration)
        ? enumeration
        : default;
    }

    /// <summary>
    /// Obtiene una instancia de la enumeración a partir de su nombre.
    /// </summary>
    /// <param name="name">Nombre de la enumeración.</param>
    /// <returns>Instancia de la enumeración o null si no existe.</returns>
    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(x => x.Name == name);
    }

    /// <summary>
    /// Obtiene todas las instancias de la enumeración.
    /// </summary>
    /// <returns>Lista de todas las enumeraciones.</returns>
    public static List<TEnum> GetValues()
    {
        return Enumerations.Values.ToList();
    }

    /// <inheritdoc/>
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;

        return GetType() == other.GetType() && Id == other.Id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Crea el diccionario de enumeraciones a partir de los campos estáticos definidos.
    /// </summary>
    /// <returns>Diccionario de enumeraciones.</returns>
    public static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType.GetFields(
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.FlattenHierarchy
        ).Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
        .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
}
