using EmptyTest.Models.Requests.Queries;
using EmptyTest.Services;
using EmptyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
public class HomeController : Controller
{
    private readonly ITutorialService _tutorialService;
    private readonly Serilog.ILogger _logger;

    public HomeController(ITutorialService tutorialService, Serilog.ILogger logger)
    {
        _tutorialService = tutorialService;
        _logger = logger;
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
