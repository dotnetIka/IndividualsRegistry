using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsRegistry.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasQueryFilter(p => p.DeletedAt == null);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(p => p.PersonalNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(p => p.DateOfBirth)
            .IsRequired();

        builder.HasMany(p => p.PhoneNumbers)
            .WithOne(pn => pn.Person)
            .HasForeignKey(pn => pn.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Relationships)
            .WithOne(pr => pr.Person)
            .HasForeignKey(pr => pr.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
