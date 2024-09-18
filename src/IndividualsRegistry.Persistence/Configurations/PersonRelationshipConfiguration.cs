using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndividualsRegistry.Persistence.Configurations;

public class PersonRelationshipConfiguration : IEntityTypeConfiguration<PersonRelationship>
{
    public void Configure(EntityTypeBuilder<PersonRelationship> builder)
    {
        builder.HasQueryFilter(pr => pr.DeletedAt == null);

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.RelationType)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasOne(pr => pr.Person)
               .WithMany(p => p.Relationships)
               .HasForeignKey(pr => pr.PersonId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.RelatedPerson)
               .WithMany()
               .HasForeignKey(pr => pr.RelatedPersonId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pr => new { pr.PersonId, pr.RelatedPersonId, pr.RelationType }).IsUnique();
    }
}