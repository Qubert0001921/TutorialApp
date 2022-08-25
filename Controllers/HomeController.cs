using EmptyTest.Models.Requests.Queries;
using EmptyTest.Services;
using EmptyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
public class HomeController : Controller
{
    private readonly ITutorialService _tutorialService;

    public HomeController(ITutorialService tutorialService)
    {
        _tutorialService = tutorialService;
    }

    public async Task<IActionResult> Index([FromQuery] GetTutorialsQuery query)
    {
        var tutorials = await _tutorialService.GetPublicTutorialsAsync();
        var viewModel = new TutorialsViewModel
        {
            Query = query,
            Tutorials = tutorials.ToList(),
        };
        return View(viewModel);
    }
}
