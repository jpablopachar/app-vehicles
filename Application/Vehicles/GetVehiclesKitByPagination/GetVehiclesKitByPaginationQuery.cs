using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Vehicles;

namespace Application.Vehicles.GetVehiclesKitByPagination;

public record GetVehiclesKitByPaginationQuery : PaginationParams, IQuery<PagedResults<Vehicle, VehicleId>>;
