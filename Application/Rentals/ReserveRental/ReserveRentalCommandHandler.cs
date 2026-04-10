using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Rentals;
using Domain.Users;
using Domain.Vehicles;

namespace Application.Rentals.ReserveRental;

/// <summary>
/// Controlador de comandos para procesar la reserva de alquileres de vehículos.
/// </summary>
/// <remarks>
/// Maneja la lógica de negocio para reservar un vehículo por parte de un usuario,
/// validando la disponibilidad del vehículo y el usuario, calculando los precios
/// y persistiendo la reserva en la base de datos.
/// </remarks>
internal sealed class ReservarAlquilerCommandHandler(
    IUserRepository userRepository,
    IVehicleRepository vehicleRepository,
    IRentalRepository rentalRepository,
    PriceService priceService,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<ReserveRentalCommand, Guid>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IRentalRepository _rentalRepository = rentalRepository;
    private readonly PriceService _priceService = priceService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    /// <summary>
    /// Procesa la reserva de un alquiler de vehículo.
    /// </summary>
    /// <param name="request">Comando con los datos de la reserva a procesar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Un resultado que contiene el identificador del alquiler creado si la operación es exitosa,
    /// o un error si el usuario, el vehículo no existe o si hay solapamiento con otras reservas.
    /// </returns>
    public async Task<Result<Guid>> Handle(
        ReserveRentalCommand request,
        CancellationToken cancellationToken
    )
    {

        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetById(userId, cancellationToken);

        if (user is null) return Result.Failure<Guid>(UserErrors.NotFound);

        var vehicleId = new VehicleId(request.VehicleId);
        var vehicle = await _vehicleRepository.GetById(vehicleId, cancellationToken);

        if (vehicle is null) return Result.Failure<Guid>(VehicleErrors.NotFound);

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await _rentalRepository.IsOverlapping(vehicle, duration, cancellationToken)) return Result.Failure<Guid>(RentalErrors.Overlap);

        try
        {
            var rental = Rental.Reserve(
                vehicle,
                user.Id!,
                duration,
                _dateTimeProvider.CurrentTime,
                _priceService
            );

            _rentalRepository.Add(rental);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rental.Id!.Value;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }
    }
}
