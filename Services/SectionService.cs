using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Helpers;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Repositories;

namespace EmptyTest.Services;
public interface ISectionService
{
    Task<ServiceResult> CreateSection(Guid tutorialId, AddSectionRequest requestModel);
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
}
