using IndividualsRegistry.Domain.Interfaces;
using IndividualsRegistry.Infrastructure.Persistence;
using IndividualsRegistry.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace IndividualsRegistry.Persistence;
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IndividualsRegistryDbContext>(options =>
     options.UseSqlServer(configuration.GetConnectionString("k")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPersonRelationshipRepository, PersonRelationshipRepository>();
        services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();


        return services;
    }
}
