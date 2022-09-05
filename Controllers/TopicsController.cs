using EmptyTest.Exceptions;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Services;
using EmptyTest.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
[Controller]
[Route("Tutorials/{tutorialId:guid}/Section/{sectionId:guid}/[controller]")]
[Authorize]
public class TopicsController : Controller
{
    private readonly ITopicService _topicService;
    private readonly ISectionService _sectionService;

    public TopicsController(ITopicService topicService, ISectionService sectionService)
    {
        _topicService = topicService;
        _sectionService = sectionService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTopic([FromForm] AddTopicRequest modelRequest, [FromRoute] Guid sectionId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var topic = await _topicService.CreateTopic(modelRequest, sectionId);
            return Created(topic.Id.ToString(), topic);
        }
        catch (BadRequestException exception)
        {
            ModelState.AddModelError(exception.Property, exception.Message);
            return BadRequest(ModelState);
        }

    }

    [HttpGet]
    [Route("{topicId:guid}")]
    public async Task<IActionResult> GetTopicById([FromRoute] Guid topicId, [FromRoute] Guid sectionId, [FromRoute] Guid tutorialId)
    {
        try
        {
            var section = await _sectionService.GetSectionById(sectionId);
            var topic = section.TopicsResponses.FirstOrDefault(x => x.Id == topicId);

            if (topic is null)
            {
                return Redirect($"/Tutorials/{tutorialId}");
            }

            TopicViewModel viewModel = new()
            {
                CurrentSection = section,
                CurrentTopic = topic
            };

            return View("Views/Topics/TopicById.cshtml", viewModel);
        }
        catch (NotFoundException exception)
        {
            return Redirect($"/Tutorials/{tutorialId}");
        }
    }
}
