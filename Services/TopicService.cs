using AutoMapper;
using EmptyTest.AppSettings;
using EmptyTest.Entities;
using EmptyTest.Exceptions;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;
using EmptyTest.Repositories;

namespace EmptyTest.Services;
public interface ITopicService
{
    Task<TopicResponse> CreateTopic(AddTopicRequest modelRequest, Guid sectionId);
}

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;
    private readonly ISectionRepository _sectionRepository;
    private readonly IFileSaveSettingsService _fileSaveSettingsService;
    private readonly IConfiguration _configuration;

    public TopicService(ITopicRepository topicRepository, IMapper mapper, ISectionRepository sectionRepository, IFileSaveSettingsService fileSaveSettingsService, IConfiguration configuration)
    {
        _topicRepository = topicRepository;
        _mapper = mapper;
        _sectionRepository = sectionRepository;
        _fileSaveSettingsService = fileSaveSettingsService;
        _configuration = configuration;
    }

    public async Task<TopicResponse> CreateTopic(AddTopicRequest modelRequest, Guid sectionId)
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

        if (modelRequest.VideoFile is not null && modelRequest.VideoFile.ContentType != "video/mp4")
        {
            throw new BadRequestException("VideoFile", "Wrong video file extension");
        }

        if (modelRequest.DocumentFile is not null && modelRequest.DocumentFile.ContentType != "application/pdf")
        {
            throw new BadRequestException("DocumentFile", "Wrong document file extension");
        }

        var topic = _mapper.Map<Topic>(modelRequest);
        topic.LastModifiedDate = DateTime.UtcNow;
        topic.SectionId = sectionId;

        var fileSaveSettings = new FileSaveSettings();
        _configuration.GetSection("FileSave").Bind(fileSaveSettings);

        if (modelRequest.DocumentFile is not null)
        {
            var documentFileName = await SaveFile(_fileSaveSettingsService.DocumentsPath, modelRequest.DocumentFile);
            topic.DocumentFilePath = $"/{fileSaveSettings.UserDataFolder}/{fileSaveSettings.DocumentsFolder}/{documentFileName}";
        }

        if (modelRequest.VideoFile is not null)
        {
            var videoFileName = await SaveFile(_fileSaveSettingsService.VideosPath, modelRequest.VideoFile);
            topic.VideFilePath = $"/{fileSaveSettings.UserDataFolder}/{fileSaveSettings.VideosFolder}/{videoFileName}";
        }

        await _topicRepository.CreateAsync(topic);

        var model = _mapper.Map<TopicResponse>(topic);

        return model;
    }

    private async Task<string> SaveFile(string basePath, IFormFile file)
    {
        var fileExtension = new FileInfo(file.FileName).Extension;
        var fileName = DateTime.Now.Ticks.ToString() + fileExtension;

        using var fs = new FileStream(Path.Join(basePath, fileName), FileMode.Create);
        await file.CopyToAsync(fs);

        return fileName;
    }
}
