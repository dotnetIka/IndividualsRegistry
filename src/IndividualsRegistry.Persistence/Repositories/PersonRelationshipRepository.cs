using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;

namespace IndividualsRegistry.Persistence.Repositories;
public class PersonRelationshipRepository : GenericRepository<PersonRelationship>, IPersonRelationshipRepository
{
    public PersonRelationshipRepository(IndividualsRegistryDbContext context) : base(context)
    {
    }
}