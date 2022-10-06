using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Infrastructure;

public class DatabaseConfiguration
{
    public const string SectionName = "Database";

    [Required] public string ConnectionString { get; set; } = default!;
}