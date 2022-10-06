using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bootstrap.Infrastructure;

public class BootstrapDbContext : DbContext
{
    private readonly IOptions<DatabaseConfiguration> _configuration;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<BootstrapDbContext> _logger;

    public BootstrapDbContext(
        DbContextOptions<BootstrapDbContext> options,
        IOptions<DatabaseConfiguration> configuration,
        IHostEnvironment environment,
        ILogger<BootstrapDbContext> logger) : base(options)
    {
        _configuration = configuration;
        _environment = environment;
        _logger = logger;
    }

    public virtual DbSet<CustomerEntity> Customers { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = optionsBuilder.UseNpgsql(_configuration);
        if (!_environment.IsDevelopment())
        {
            builder.UseLogging(_logger);
        }
    }
}