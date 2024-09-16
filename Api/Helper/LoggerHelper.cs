using Api.Common.Model.Options;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Elasticsearch;

namespace Api.Helper;

public static class LoggerHelper
{
    public static void ConfigureSeriLog(AppSetting? appSetting)
    {
        ArgumentNullException.ThrowIfNull(appSetting);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "development";
        SelfLog.Enable(msg => Console.WriteLine(msg));
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(appSetting.Elastic.Host))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true, 
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                DetectElasticsearchVersion = true,
                IndexFormat = $"{appSetting.Elastic.Index}-{environment}-logs-" + "{0:yyyy.MM.dd}",
                ModifyConnectionSettings = s => s.BasicAuthentication(appSetting.Elastic.Username, appSetting.Elastic.Password),
            })
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environment)
            .CreateLogger();
    }
}