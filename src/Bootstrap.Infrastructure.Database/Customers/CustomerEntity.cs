using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bootstrap.Infrastructure.Database.Customers;

[Table("Customer")]
public class CustomerEntity
{
    [Key] public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
