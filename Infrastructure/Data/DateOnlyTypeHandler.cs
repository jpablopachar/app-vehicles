using System.Data;
using Dapper;

namespace Infrastructure.Data;

/// <summary>
/// Controlador de tipos personalizado para manejar la serialización y deserialización de valores <see cref="DateOnly"/>
/// con Dapper en operaciones de base de datos.
/// </summary>
internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    /// <summary>
    /// Convierte un valor de base de datos en un objeto <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="value">El valor de la base de datos a convertir, generalmente un <see cref="DateTime"/>.</param>
    /// <returns>Un objeto <see cref="DateOnly"/> que representa la fecha sin la hora.</returns>
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

    /// <summary>
    /// Establece el valor de un parámetro de base de datos a partir de un objeto <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="parameter">El parámetro de base de datos a configurar.</param>
    /// <param name="value">El valor <see cref="DateOnly"/> a asignar al parámetro.</param>
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}
