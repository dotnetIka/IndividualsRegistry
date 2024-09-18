using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;

namespace IndividualsRegistry.Persistence.Repositories;
public class PhoneNumberRepository : GenericRepository<PhoneNumber>, IPhoneNumberRepository
{
    public PhoneNumberRepository(IndividualsRegistryDbContext context) : base(context)
    {
    }
}
