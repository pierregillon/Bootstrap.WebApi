using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Infrastructure.Database;

public class DatabaseConfiguration
{
    public static string Section => "Database";
    
    [Required]
    public string? Host { get; set; }
    
    [Required]
    public int? Port { get; set; }
    
    [Required]
    public string? Database { get; set; }
    
    [Required]
    public string? User { get; set; }
    
    [Required]
    public string? Password { get; set; }

    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};User ID={User};Password={Password};Include Error Detail=true";
}
