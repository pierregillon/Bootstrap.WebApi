using Bootstrap.Infrastructure.Database.Customers;
using Bootstrap.Infrastructure.Database.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bootstrap.Infrastructure.Database;

public class BootstrapDbContext : DbContext
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<BootstrapDbContext> _logger;

    public BootstrapDbContext(
        DbContextOptions<BootstrapDbContext> options,
        IHostEnvironment environment,
        ILogger<BootstrapDbContext> logger) : base(options)
    {
        _environment = environment;
        _logger = logger;
    }

    public virtual DbSet<CustomerEntity> Customers { get; set; } = default!;
    public virtual DbSet<UserEntity> Users { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!_environment.IsDevelopment())
        {
            optionsBuilder.UseLogging(_logger);
        }
    }
}
