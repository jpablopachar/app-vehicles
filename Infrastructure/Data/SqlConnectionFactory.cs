using System.Data;
using Application.Abstractions.Data;
using Npgsql;

namespace Infrastructure.Data;

/// <summary>
/// Factory sellado para crear conexiones SQL a una base de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase implementa el patrón Factory para proporcionar instancias de <see cref="IDbConnection"/>
/// configuradas con la cadena de conexión especificada.
/// </remarks>
internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    /// <summary>
    /// Cadena de conexión a la base de datos PostgreSQL.
    /// </summary>
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// Crea y abre una nueva conexión a la base de datos PostgreSQL.
    /// </summary>
    /// <returns>Una instancia de <see cref="IDbConnection"/> abierta y lista para usar.</returns>
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);

        connection.Open();

        return connection;
    }
}
