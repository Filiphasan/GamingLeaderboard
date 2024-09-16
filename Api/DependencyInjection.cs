using System.Reflection;
using Api.Common.Model.Options;
using Api.Data;
using Api.FluentValidation;
using Api.Helper;
using Api.Middleware;
using Api.Service.Implementation;
using Api.Service.Interface;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnvModels(configuration)
            .AddServices()
            .AddFluentValidation()
            .AddDataContext(configuration)
            .AddMediator();
        return services;
    }

    private static IServiceCollection AddEnvModels(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettingModel = configuration.GetValue<AppSetting>(AppSetting.SectionName);
        ArgumentNullException.ThrowIfNull(appSettingModel);
        services.AddSingleton(appSettingModel);
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var appSettingModel = services.BuildServiceProvider().GetRequiredService<AppSetting>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        var redisConnection = RedisHelper.GetConnection(appSettingModel.Redis);
        services.AddSingleton(redisConnection);
        services.AddSingleton<IRedisCacheService, RedisCacheService>();

        services.AddTransient<LoggingMiddleware>();
        services.AddTransient<ExceptionMiddleware>();

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config => { config.OverrideDefaultResultFactoryWith<FvResultFactory>(); });
        return services;
    }

    private static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<LeaderboardContext>(opt => { opt.UseNpgsql(configuration.GetConnectionString(""), sqlOpt => { sqlOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); }); });
        return services;
    }
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }

    public static void MigratePendingMigrations(this IApplicationBuilder app)
    {
        var context = app.ApplicationServices.GetRequiredService<LeaderboardContext>();
        var hasPendingMigrations = context.Database.GetPendingMigrations().Any();
        if (hasPendingMigrations)
        {
            context.Database.Migrate();
        }
    }
}