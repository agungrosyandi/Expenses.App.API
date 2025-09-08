using System.ComponentModel.DataAnnotations;

namespace Expenses.App.API.Dtos.auth;

public class PostLoginUserDto
{
    [Required]
    [EmailAddress] // ensures valid email format
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
