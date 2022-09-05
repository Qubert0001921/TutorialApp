﻿using EmptyTest.Exceptions;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
[Controller]
[Route("Tutorials/{tutorialId:guid}/Section/{sectionId:guid}/[controller]")]
[Authorize]
public class TopicsController : Controller
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTopic([FromForm] AddTopicRequest modelRequest, [FromRoute] Guid sectionId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Guid topicId;

        try
        {
            topicId = await _topicService.CreateTopic(modelRequest, sectionId);
        }
        catch (BadRequestException exception)
        {
            ModelState.AddModelError(exception.Property, exception.Message);
            return BadRequest(ModelState);
        }

        return Created(topicId.ToString(), null);
    }

    [HttpGet]
    [Route("{topicId:guid}")]
    public async Task<IActionResult> GetTopicById([FromRoute] Guid topicId)
    {
        return View("Views/Topics/TopicById.cshtml");
    }
}
