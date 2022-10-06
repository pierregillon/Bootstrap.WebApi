using System.ComponentModel.DataAnnotations;

namespace Bootstrap.WebApi.Controllers;

public class RegisterNewCustomerBody
{
    [Required]
    public string FirstName { get; set; } = default!;
    
    [Required]
    public string LastName { get; set; } = default!;
}