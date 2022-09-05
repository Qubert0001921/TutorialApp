using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Exceptions;
using EmptyTest.Helpers;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;
using EmptyTest.Repositories;

namespace EmptyTest.Services;
public interface ISectionService
{
    Task<ServiceResult> CreateSection(Guid tutorialId, AddSectionRequest requestModel);
    Task<SectionResponse> GetSectionById(Guid sectionId);
}

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public SectionService(ISectionRepository sectionRepository, IMapper mapper)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult> CreateSection(Guid tutorialId, AddSectionRequest requestModel)
    {
        var existingSection = await _sectionRepository.FindSectionByNameAndTutorialId(requestModel.Name, tutorialId);
        if (existingSection is not null)
        {
            return ServiceResult.ValidationError;
        }

        var section = _mapper.Map<Section>(requestModel);
        section.TutorialId = tutorialId;
        await _sectionRepository.CreateAsync(section);

        return ServiceResult.Success;
    }

    public async Task<SectionResponse> GetSectionById(Guid sectionId)
    {
        var section = await _sectionRepository.FindByIdWithValuesAsync(sectionId);
        if (section is null)
        {
            throw new NotFoundException("Section not found");
        }

        var topicResponses = _mapper.Map<List<TopicResponse>>(section.Topics);
        var model = _mapper.Map<SectionResponse>(section);

        model.TopicsResponses = topicResponses;
        return model;
    }
}
