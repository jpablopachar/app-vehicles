using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad OutboxMessage para Entity Framework Core.
/// </summary>
/// <remarks>
/// Esta clase configura la tabla y propiedades de la entidad OutboxMessage,
/// incluyendo la clave primaria y el tipo de columna para el contenido.
/// </remarks>
internal sealed class OutboxMessageConfiguration
: IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    /// Configura la entidad OutboxMessage en la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para OutboxMessage.</param>
    /// <remarks>
    /// Define el nombre de la tabla como "outbox_messages", establece la clave primaria
    /// y configura la propiedad Content como tipo jsonb.
    /// </remarks>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outboxMessage => outboxMessage.Content)
        .HasColumnType("jsonb");
    }
}
