using System.Data;

namespace Application.Abstractions.Data;

/// <summary>
/// Fábrica para crear conexiones a la base de datos SQL.
/// </summary>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Crea una nueva conexión a la base de datos.
    /// </summary>
    /// <returns>Una nueva instancia de <see cref="IDbConnection"/> configurada y lista para usar.</returns>
    IDbConnection CreateConnection();
}
