using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;

namespace EmptyTest.ViewModels;
public class AddSectonViewModel
{
    public TutorialResponse Tutorial { get; set; }
    public AddSectionRequest AddSectionRequest { get; set; }
}
