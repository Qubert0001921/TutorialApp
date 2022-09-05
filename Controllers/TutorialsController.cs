using EmptyTest.Helpers;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;

[Authorize]
[Route("[controller]")]
[Controller]
public class TutorialsController : Controller
{
    private readonly ITutorialService _tutorialService;
    private readonly IUserContextService _userContextService;

    public TutorialsController(ITutorialService tutorialService, IUserContextService userContextService)
    {
        _tutorialService = tutorialService;
        _userContextService = userContextService;
    }

    [Route("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTutorialById([FromRoute] Guid id)
    {
        var tutorial = await _tutorialService.GetTutorialById(id);
        if (tutorial is null)
        {
            return Redirect(Routes.HomePage);
        }

        ViewData["IsAuthor"] = false;

        var accountIdClaim = _userContextService.AccountId;
        Guid.TryParse(accountIdClaim?.Value, out Guid accountId);
        if (accountId == tutorial?.AccountId)
        {
            ViewData["IsAuthor"] = true;
        }

        return View("TutorialById", tutorial);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tutorials = await _tutorialService.GetAccountTutorialsAsync();
        return View("Index", tutorials);
    }

    [Route("[action]")]
    public IActionResult CreateTutorial()
    {
        return View();
    }

    [HttpPost]
    [Route("CreateTutorial")]
    public async Task<IActionResult> CreateTutorial([FromForm] CreateTutorialRequest requestModel)
    {
        if (!ModelState.IsValid) return View(requestModel);

        var result = await _tutorialService.CreateTutorialAsync(requestModel);
        switch (result)
        {
            case ServiceResult.UnAuthorized:
                return Redirect(Routes.SignInPage);
            case ServiceResult.ValidationError:
                ViewData["NameError"] = "You've already created tutorial at this name";
                return View();
        }

        return View();
    }
}
