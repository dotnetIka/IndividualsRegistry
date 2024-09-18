using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsRegistry.Persistence.Configurations;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.HasQueryFilter(pn => pn.DeletedAt == null);

        builder.HasKey(pn => pn.Id);

        builder.Property(pn => pn.Type)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(pn => pn.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(pn => pn.Person)
            .WithMany(p => p.PhoneNumbers)
            .HasForeignKey(pn => pn.PersonId);
    }
}
