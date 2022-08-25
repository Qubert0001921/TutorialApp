using EmptyTest.Models.Responses;

namespace EmptyTest.ViewModels;
public class TutorialByIdViewModel
{
    public TutorialResponse Tutorial { get; set; }
    public List<SectionResponse> Sections { get; set; }
}
