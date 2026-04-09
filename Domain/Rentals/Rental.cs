using Domain.Abstractions;
using Domain.Rentals.Events;
using Domain.Shared;
using Domain.Vehicles;

namespace Domain.Rentals;

/// <summary>
/// Representa una entidad de alquiler de vehículos en el dominio de la aplicación.
/// Esta clase gestiona el ciclo de vida completo de un alquiler, desde la reserva inicial
/// hasta la confirmación, cancelación o finalización.
/// </summary>
public sealed class Rental : Entity<RentalId>
{
    /// <summary>
    /// Identificador del vehículo alquilado.
    /// </summary>
    public VehicleId? VehicleId { get; private set; }

    /// <summary>
    /// Identificador del usuario que realiza el alquiler.
    /// </summary>
    public Users.UserId? UserId { get; private set; }

    /// <summary>
    /// Precio por período de alquiler.
    /// </summary>
    public Currency? PricePerPeriod { get; private set; }

    /// <summary>
    /// Costo de mantenimiento del vehículo.
    /// </summary>
    public Currency? Maintenance { get; private set; }

    /// <summary>
    /// Costo de accesorios adicionales.
    /// </summary>
    public Currency? Accessories { get; private set; }

    /// <summary>
    /// Precio total del alquiler.
    /// </summary>
    public Currency? TotalPrice { get; private set; }

    /// <summary>
    /// Estado actual del alquiler.
    /// </summary>
    public RentalStatus Status { get; private set; }

    /// <summary>
    /// Rango de fechas del período de alquiler.
    /// </summary>
    public DateRange? Duration { get; private set; }

    /// <summary>
    /// Fecha y hora de creación de la reserva.
    /// </summary>
    public DateTime? CreationDate { get; private set; }

    /// <summary>
    /// Fecha y hora en que se confirmó el alquiler.
    /// </summary>
    public DateTime? ConfirmationDate { get; private set; }

    /// <summary>
    /// Fecha y hora en que se rechazó el alquiler.
    /// </summary>
    public DateTime? DenialDate { get; private set; }

    /// <summary>
    /// Fecha y hora en que se completó el alquiler.
    /// </summary>
    public DateTime? CompletionDate { get; private set; }

    /// <summary>
    /// Fecha y hora en que se canceló el alquiler.
    /// </summary>
    public DateTime? CancellationDate { get; private set; }

    /// <summary>
    /// Constructor privado paramétrico que inicializa una nueva instancia de la clase Rental.
    /// </summary>
    private Rental() { }

    /// <summary>
    /// Constructor privado que inicializa una nueva instancia de la clase Rental con todos los parámetros necesarios.
    /// </summary>
    /// <param name="id">Identificador único del alquiler.</param>
    /// <param name="vehicleId">Identificador del vehículo a alquilar.</param>
    /// <param name="userId">Identificador del usuario que alquila.</param>
    /// <param name="pricePerPeriod">Precio por período de alquiler.</param>
    /// <param name="maintenance">Costo de mantenimiento.</param>
    /// <param name="accessories">Costo de accesorios.</param>
    /// <param name="totalPrice">Precio total del alquiler.</param>
    /// <param name="status">Estado del alquiler.</param>
    /// <param name="duration">Período de alquiler.</param>
    /// <param name="creationDate">Fecha de creación de la reserva.</param>
    private Rental(RentalId id, VehicleId vehicleId, Users.UserId userId, Currency pricePerPeriod, Currency maintenance, Currency accessories, Currency totalPrice, RentalStatus status, DateRange duration, DateTime creationDate) : base(id)
    {
        VehicleId = vehicleId;
        UserId = userId;
        PricePerPeriod = pricePerPeriod;
        Maintenance = maintenance;
        Accessories = accessories;
        TotalPrice = totalPrice;
        Status = status;
        Duration = duration;
        CreationDate = creationDate;
    }

    /// <summary>
    /// Crea una nueva reserva de alquiler para un vehículo específico.
    /// </summary>
    /// <param name="vehicle">El vehículo a reservar.</param>
    /// <param name="userId">El identificador del usuario que realiza la reserva.</param>
    /// <param name="duration">El período de alquiler solicitado.</param>
    /// <param name="creationDate">La fecha y hora de creación de la reserva.</param>
    /// <param name="priceService">Servicio para calcular los precios del alquiler.</param>
    /// <returns>Una nueva instancia de Rental con estado Reservado.</returns>
    public static Rental Reserve(Vehicle vehicle, Users.UserId userId, DateRange duration, DateTime creationDate, PriceService priceService)
    {
        var priceDetail = priceService.CalculatePrice(vehicle, duration);

        var rental = new Rental(
            RentalId.New(),
            vehicle.Id!,
            userId,
            priceDetail.PricePerPeriod,
            priceDetail.Maintenance,
            priceDetail.Accessories,
            priceDetail.TotalPrice,
            RentalStatus.Reserved,
            duration,
            creationDate
        );

        rental.RaiseDomainEvent(new RentalReservedDomainEvent(rental.Id!));

        return rental;
    }

    /// <summary>
    /// Confirma una reserva de alquiler que se encuentra en estado Reservado.
    /// </summary>
    /// <param name="utcNow">La fecha y hora actual en formato UTC.</param>
    /// <returns>Un resultado indicando si la confirmación fue exitosa.</returns>
    public Result Confirm(DateTime utcNow)
    {
        if (Status != RentalStatus.Reserved) return Result.Failure(RentalErrors.NotReserved);

        Status = RentalStatus.Confirmed;
        ConfirmationDate = utcNow;

        RaiseDomainEvent(new RentalConfirmedDomainEvent(Id!));

        return Result.Success();
    }

    /// <summary>
    /// Rechaza una reserva de alquiler que se encuentra en estado Reservado.
    /// </summary>
    /// <param name="utcNow">La fecha y hora actual en formato UTC.</param>
    /// <returns>Un resultado indicando si el rechazo fue exitoso.</returns>
    public Result Reject(DateTime utcNow)
    {
        if (Status != RentalStatus.Reserved) return Result.Failure(RentalErrors.NotReserved);

        Status = RentalStatus.Rejected;
        DenialDate = utcNow;

        RaiseDomainEvent(new RentalRejectedDomainEvent(Id!));

        return Result.Success();
    }

    /// <summary>
    /// Cancela un alquiler confirmado, siempre que no haya iniciado.
    /// </summary>
    /// <param name="utcNow">La fecha y hora actual en formato UTC.</param>
    /// <returns>Un resultado indicando si la cancelación fue exitosa o si el alquiler ya ha iniciado.</returns>
    public Result Cancel(DateTime utcNow)
    {
        if (Status != RentalStatus.Confirmed) return Result.Failure(RentalErrors.NotConfirmed);

        var currentDate = DateOnly.FromDateTime(utcNow);

        if (currentDate > Duration!.Start) return Result.Failure(RentalErrors.AlreadyStarted);

        Status = RentalStatus.Cancelled;
        CancellationDate = utcNow;

        RaiseDomainEvent(new RentalCancelledDomainEvent(Id!));

        return Result.Success();
    }

    /// <summary>
    /// Completa un alquiler confirmado, marcándolo como finalizado.
    /// </summary>
    /// <param name="utcNow">La fecha y hora actual en formato UTC.</param>
    /// <returns>Un resultado indicando si la finalización fue exitosa.</returns>
    public Result Complete(DateTime utcNow)
    {
        if (Status != RentalStatus.Confirmed) return Result.Failure(RentalErrors.NotConfirmed);

        Status = RentalStatus.Completed;
        CompletionDate = utcNow;

        RaiseDomainEvent(new RentalCompletedDomainEvent(Id!));

        return Result.Success();
    }
}
