using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence.Options.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> AddAuditProperties<T>(this EntityTypeBuilder<T> builder) where T : class
    {
        builder.Property<DateTime>("CreatedAt");
        builder.Property<DateTime?>("ModifiedAt");
        builder.Property<DateTime?>("DeletedAt");
        builder.Property<bool>("IsDeleted");
        return builder;
    }
}
