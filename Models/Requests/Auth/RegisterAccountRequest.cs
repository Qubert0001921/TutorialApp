using System.ComponentModel.DataAnnotations;

namespace EmptyTest.Models.Requests.Auth;
public class RegisterAccountRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MinLength(3, ErrorMessage = "Minimum lenght of email is 3")]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(8, ErrorMessage = "Minimun length of password is 8")]
    public string Password { get; set; } = string.Empty;
}
