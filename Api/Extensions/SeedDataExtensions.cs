using Application.Abstractions.Data;
using Bogus;
using Dapper;
using Domain.Users;
using Domain.Vehicles;
using Infrastructure;

namespace Api.Extensions;

/// <summary>
/// Clase de extensión que proporciona métodos para inicializar (seed) datos en la base de datos.
/// </summary>
public static class SeedDataExtensions
{
    /// <summary>
    /// Inicializa los datos de autenticación en la base de datos, crear dos usuarios de prueba si no existen.
    /// </summary>
    /// <param name="app">Interfaz del constructor de aplicación.</param>
    public static void SeedDataAuthentication(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();

        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<AppVehiclesDbContext>();

            if (!context.Set<User>().Any())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test123$");

                var user = User.Create(
                    new Name("Juan Pablo"),
                    new Lastname("Pachar"),
                    new Email("jppachar@yopmail.com"),
                    new PasswordHash(passwordHash)
                );

                context.Add(user);

                passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123$");

                user = User.Create(
                    new Name("Admin"),
                    new Lastname("Admin"),
                    new Email("admin@yopmail.com"),
                    new PasswordHash(passwordHash)
                );

                context.Add(user);
                context.SaveChangesAsync().Wait();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<AppVehiclesDbContext>();

            logger.LogError(ex.Message);
        }
    }

    /// <summary>
    /// Inicializa la base de datos con 100 vehículos generados de forma aleatoria.
    /// </summary>
    /// <param name="app">Interfaz del constructor de aplicación.</param>
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> vehicles = [];

        for (var i = 0; i < 100; i++)
        {
            vehicles.Add(new
            {
                Id = Guid.NewGuid(),
                Vin = faker.Vehicle.Vin(),
                Model = faker.Vehicle.Model(),
                Country = faker.Address.Country(),
                Department = faker.Address.State(),
                Province = faker.Address.County(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                AmountPrice = faker.Random.Decimal(1000, 20000),
                CurrencyTypePrice = "USD",
                MaintenancePrice = faker.Random.Decimal(100, 200),
                CurrencyTypeMaintenance = "USD",
                Accessories = new List<int> { (int)Accessory.Wifi, (int)Accessory.AppleCarPlay },
                LastRentalDate = DateTime.MinValue
            });
        }

        const string sql = """
            INSERT INTO public.vehiculos
                (id, vin, modelo, direccion_pais, direccion_departamento, direccion_provincia, direccion_ciudad, direccion_calle, precio_monto, precio_tipo_moneda, mantenimiento_monto, mantenimiento_tipo_moneda, accesorios, fecha_ultima_alquiler)
                values(@id, @Vin,@Modelo,@Pais, @Departamento, @Provincia, @Ciudad, @Calle, @PrecioMonto, @PrecioTipoMoneda, @PrecioMantenimiento, @PrecioMantenimientoTipoMoneda, @Accesorios, @FechaUltima)
        """;

        connection.Execute(sql, vehicles);
    }
}
