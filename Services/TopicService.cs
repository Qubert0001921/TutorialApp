using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Exceptions;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Repositories;

namespace EmptyTest.Services;
public interface ITopicService
{
    Task<Guid> CreateTopic(AddTopicRequest modelRequest, Guid sectionId);
}

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;
    private readonly ISectionRepository _sectionRepository;

    public TopicService(ITopicRepository topicRepository, IMapper mapper, ISectionRepository sectionRepository)
    {
        _topicRepository = topicRepository;
        _mapper = mapper;
        _sectionRepository = sectionRepository;
    }

    public async Task<Guid> CreateTopic(AddTopicRequest modelRequest, Guid sectionId)
    {
        var section = await _sectionRepository.FindByIdAsync(sectionId);
        if (section is null)
        {
            throw new BadRequestException("Section not found!");
        }

        var existingTopic = await _topicRepository.FindByNameAndSectionIdAsync(modelRequest.Title, sectionId);
        if (existingTopic is not null)
        {
            throw new BadRequestException("Title", "Topic at this name is already taken");
        }

        var topic = _mapper.Map<Topic>(modelRequest);
        topic.LastModifiedDate = DateTime.UtcNow;
        topic.SectionId = sectionId;

        await _topicRepository.CreateAsync(topic);

        return topic.Id;
    }
}
