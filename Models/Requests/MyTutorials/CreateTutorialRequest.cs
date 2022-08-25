using System.ComponentModel.DataAnnotations;

namespace EmptyTest.Models.Requests.MyTutorials;
public class CreateTutorialRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(5, ErrorMessage = "Min lenght of Name is 5")]
    public string Name { get; set; } = String.Empty;
    [MaxLength(80, ErrorMessage = "Max length of Description is 80")]
    public string Description { get; set; } = String.Empty;
    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Access is required")]
    public bool IsPublic { get; set; }
    [Required(ErrorMessage = "Image is required")]
    public IFormFile ImageFile { get; set; }
}
