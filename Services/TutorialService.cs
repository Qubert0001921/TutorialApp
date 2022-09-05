using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Helpers;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;
using EmptyTest.Repositories;

namespace EmptyTest.Services;
public interface ITutorialService
{
	Task<ServiceResult> CreateTutorialAsync(CreateTutorialRequest requestModel);
	Task<IEnumerable<TutorialResponse>> GetPublicTutorialsAsync();
	Task<IEnumerable<TutorialResponse>> GetAccountTutorialsAsync();
	Task<TutorialResponse?> GetTutorialById(Guid tutorialId);
}

public class TutorialService : ITutorialService
{
	private readonly ITutorialRepository _tutorialRepository;
	private readonly IUserContextService _userContextService;
	private readonly IMapper _mapper;
	private readonly IWebHostEnvironment _environment;
	private readonly ISectionRepository _sectionRepository;

	public TutorialService(ITutorialRepository tutorialRepository, IUserContextService userContextService, IMapper mapper, IWebHostEnvironment environment,
		ISectionRepository sectionRepository)
	{
		_tutorialRepository = tutorialRepository;
		_userContextService = userContextService;
		_mapper = mapper;
		_environment = environment;
		_sectionRepository = sectionRepository;
	}

	public async Task<ServiceResult> CreateTutorialAsync(CreateTutorialRequest requestModel)
	{
		var accountIdClaim = _userContextService.AccountId;
		Guid.TryParse(accountIdClaim?.Value, out Guid accountId);

		if (accountIdClaim is null)
		{
			return ServiceResult.UnAuthorized;
		}

		var existingTutorial = await _tutorialRepository.FindByNameAndAccountIdAsync(accountId, requestModel.Name);

		if (existingTutorial is not null)
		{
			return ServiceResult.ValidationError;
		}

		var tutorial = _mapper.Map<Tutorial>(requestModel);
		tutorial.AccountId = accountId;

		var fileInfo = new FileInfo(requestModel.ImageFile.FileName);
		string imageFileName = $"{DateTime.Now.Ticks}{fileInfo.Extension}";
		string tutorialImagesPath = Path.Combine(_environment.WebRootPath, "usrdata", "timgs", imageFileName);

		using (var fs = new FileStream(tutorialImagesPath, FileMode.Create))
			await requestModel.ImageFile.CopyToAsync(fs);

		tutorial.ImagePath = $"/usrdata/timgs/{imageFileName}";

		await _tutorialRepository.CreateAsync(tutorial);
		return ServiceResult.Created;
	}

	public async Task<IEnumerable<TutorialResponse>> GetAccountTutorialsAsync()
	{
		var accountIdClaim = _userContextService.AccountId;
		Guid.TryParse(accountIdClaim?.Value, out Guid accountId);

		var tutorials = await _tutorialRepository.FindAllByAccountIdWithValues(accountId);
		var models = _mapper.Map<IEnumerable<TutorialResponse>>(tutorials);
		return models;
	}

	public async Task<IEnumerable<TutorialResponse>> GetPublicTutorialsAsync()
	{
		var tutorials = await _tutorialRepository.FindPublicWithValuesAsync();
		var models = _mapper.Map<IEnumerable<TutorialResponse>>(tutorials);
		return models;
	}

	public async Task<TutorialResponse?> GetTutorialById(Guid tutorialId)
	{
		var tutorial = await _tutorialRepository.FindByIdAsync(tutorialId);
		if (tutorial is null)
		{
			return null;
		}

		var sections = await _sectionRepository.FindSectionsByTutorialIdWithValuesAsync(tutorialId);

		var tutorialModel = _mapper.Map<TutorialResponse>(tutorial);
		var sectionModels = _mapper.Map<List<SectionResponse>>(sections);

		for (int i = 0; i < sectionModels.Count(); i++)
		{
			sectionModels[i].TopicsResponses = _mapper.Map<List<TopicResponse>>(sections.ElementAt(i).Topics);
		}

		tutorialModel.SectionResponses = sectionModels;

		return tutorialModel;
	}
}
