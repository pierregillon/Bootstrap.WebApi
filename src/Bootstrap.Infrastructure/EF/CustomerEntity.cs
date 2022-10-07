using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Infrastructure.EF;

public class CustomerEntity
{
    [Key] public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
