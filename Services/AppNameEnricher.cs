using Serilog.Core;
using Serilog.Events;

namespace EmptyTest.Services;
public class AppNameEnricher : ILogEventEnricher
{
    private readonly IConfiguration _configuration;

    public AppNameEnricher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var appNameProperty = propertyFactory.CreateProperty("Application", _configuration["AppName"]);
        logEvent.AddPropertyIfAbsent(appNameProperty);
    }
}
