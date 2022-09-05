using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;

namespace EmptyTest.ViewModels;
public class AddSectonViewModel
{
    public string TutorialName { get; set; }
    public Guid TutorialId { get; set; }
    public AddSectionRequest AddSectionRequest { get; set; }
}
