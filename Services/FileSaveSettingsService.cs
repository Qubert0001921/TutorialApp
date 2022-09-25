using EmptyTest.AppSettings;

namespace EmptyTest.Services;
public interface IFileSaveSettingsService
{
	string ThumbNailsPath { get; }
	string DocumentsPath { get; }
	string VideosPath { get; }
	string BasePath { get; }
}

public class FileSaveSettingsService : IFileSaveSettingsService
{
	private readonly FileSaveSettings _fileSaveSettings;
	private readonly IWebHostEnvironment _environment;

	public FileSaveSettingsService(IConfiguration configuration, IWebHostEnvironment environment)
	{
		_fileSaveSettings = new FileSaveSettings();
		configuration.GetSection("FileSave").Bind(_fileSaveSettings);
		_environment = environment;
		BasePath = Path.Join(_environment.WebRootPath, _fileSaveSettings.UserDataFolder);
	}
	public string BasePath { get; }

	public string ThumbNailsPath => Path.Join(BasePath, _fileSaveSettings.ThumbNailsFolder);
	public string DocumentsPath => Path.Join(BasePath, _fileSaveSettings.DocumentsFolder);
	public string VideosPath => Path.Join(BasePath, _fileSaveSettings.VideosFolder);
}
