using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IndividualsRegistry.Infrastructure.Persistence;

public class IndividualsRegistryDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public IndividualsRegistryDbContext()
    {

    }

    public IndividualsRegistryDbContext(DbContextOptions<IndividualsRegistryDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonRelationship> PersonRelationships { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration?.GetConnectionString("k")
                ?? "Data Source=localhost,53313;Initial Catalog=IndividualsRegistry;Integrated Security=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IndividualsRegistryDbContext).Assembly);
    }
}