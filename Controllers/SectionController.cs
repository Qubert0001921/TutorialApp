using EmptyTest.Helpers;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Services;
using EmptyTest.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;

[Controller]
[Route("Tutorials/{tutorialId:guid}/[controller]")]
[Authorize]
public class SectionController : Controller
{
    private readonly ITutorialService _tutorialService;
    private readonly ISectionService _sectionService;

    public SectionController(ITutorialService tutorialService, ISectionService sectionService)
    {
        _tutorialService = tutorialService;
        _sectionService = sectionService;
    }

    [HttpGet]
    public async Task<IActionResult> AddSectionPage([FromRoute] Guid tutorialId)
    {
        var tutorial = await _tutorialService.GetTutorialById(tutorialId);
        if (tutorial is null)
        {
            return Redirect(Routes.TutorialsPage);
        }

        var model = new AddSectonViewModel
        {
            TutorialName = tutorial.Name,
            TutorialId = tutorial.Id,
            AddSectionRequest = new AddSectionRequest()
        };

        return View("Views/Sections/AddSection.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> AddSectionForm(AddSectonViewModel viewModel, [FromRoute] Guid tutorialId)
    {
        var tutorial = await _tutorialService.GetTutorialById(tutorialId);
        if (tutorial is null)
        {
            ViewData["ValidationError"] = "Tutorial doesn't exist!";
            return View(viewModel);
        }

        viewModel.TutorialId = tutorial.Id;
        viewModel.TutorialName = tutorial.Name;

        var addingResult = await _sectionService.CreateSection(tutorialId, viewModel.AddSectionRequest);
        if (addingResult == ServiceResult.ValidationError)
        {
            ViewData["ValidationError"] = "Section at this name already exists!";
            viewModel.AddSectionRequest = new AddSectionRequest();
            return View("Views/Sections/AddSection.cshtml", viewModel);
        }

        viewModel.AddSectionRequest = new AddSectionRequest();

        ViewData["Success"] = "Successfully added section";

        return View("Views/Sections/AddSection.cshtml", viewModel);
    }
}
