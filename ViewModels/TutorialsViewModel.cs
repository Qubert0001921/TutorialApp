using EmptyTest.Models.Requests.Queries;
using EmptyTest.Models.Responses;

namespace EmptyTest.ViewModels;
public class TutorialsViewModel
{
    public GetTutorialsQuery Query { get; set; }
    public List<TutorialResponse> Tutorials { get; set; }
}
