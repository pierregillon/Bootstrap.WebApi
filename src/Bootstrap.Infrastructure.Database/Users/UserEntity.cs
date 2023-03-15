using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bootstrap.Infrastructure.Database.Users;

[Table("User")]
public class UserEntity
{
    [Key] public Guid Id { get; set; }

    public string EmailAddress { get; set; }
    public string Password { get; set; }
}
