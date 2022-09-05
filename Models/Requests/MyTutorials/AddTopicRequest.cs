using System.ComponentModel.DataAnnotations;

namespace EmptyTest.Models.Requests.MyTutorials;
public class AddTopicRequest
{
    [MinLength(2, ErrorMessage = "Title require at least 2 letters")]
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    [MaxLength(100, ErrorMessage = "Description require max 100 letters long")]
    [Required(ErrorMessage = "Description is required")]
    public string ShortDescription { get; set; }
}
