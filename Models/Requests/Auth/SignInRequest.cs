using System.ComponentModel.DataAnnotations;

namespace EmptyTest.Models.Requests.Auth;
public class SignInRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
