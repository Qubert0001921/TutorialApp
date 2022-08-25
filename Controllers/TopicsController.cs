using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
[Controller]
[Route("Tutorials/{tutorialId:guid}/Section/{sectionId:guid}/[controller]")]
public class TopicsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddTopic()
    {
        return View("/Views/Topics/AddTopic.cshtml");
    }
}
