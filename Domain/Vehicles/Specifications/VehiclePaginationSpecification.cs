using Domain.Abstractions;

namespace Domain.Vehicles.Specifications;

/// <summary>
/// Especificación para la paginación y búsqueda de vehículos.
/// Permite filtrar, ordenar y paginar vehículos basándose en criterios de búsqueda y ordenamiento.
/// </summary>
public class VehiclePaginationSpecification : BaseSpecification<Vehicle, VehicleId>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="VehiclePaginationSpecification"/>.
    /// </summary>
    /// <param name="sort">Criterio de ordenamiento: "modelAsc" (modelo ascendente), "modelDesc" (modelo descendente), o predeterminado (fecha de último alquiler).</param>
    /// <param name="pageIndex">Número de página (comienza en 1).</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <param name="search">Término de búsqueda por modelo del vehículo. Si está vacío, se retornan todos los vehículos.</param>
    public VehiclePaginationSpecification(
        string sort,
        int pageIndex,
        int pageSize,
        string search
        ) : base(
            x => string.IsNullOrEmpty(search) || x.Model!.Value.Contains(search)
        )
    {
        ApplyPaging(pageSize * (pageIndex - 1), pageSize);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "modelAsc": AddOrderBy(p => p.Model!.Value); break;
                case "modelDesc": AddOrderByDescending(p => p.Model!.Value); break;
                default: AddOrderBy(p => p.LastRentalDate!); break;
            }
        }
        else
        {
            AddOrderBy(p => p.LastRentalDate!);
        }
    }
}
