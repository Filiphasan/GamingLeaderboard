using System.Reflection;
using System.Text;
using Api.Common.Model.Options;
using Api.Common.Model.Validator;
using Api.Data;
using Api.FluentValidation;
using Api.Helper;
using Api.Middleware;
using Api.Service.Implementation;
using Api.Service.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnvModels(configuration)
            .AddServices()
            .AddJwtAuthentication()
            .AddFluentValidation()
            .AddDataContext(configuration)
            .AddMediator();

        services.AddCors(options => { options.AddPolicy("AllowOrigin", policy => policy.AllowAnyOrigin()); });
        return services;
    }

    private static IServiceCollection AddEnvModels(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettingModel = configuration.GetSection(AppSetting.SectionName).Get<AppSetting>();
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
        services.AddValidatorsFromAssemblyContaining<AddUserRequestValidator>();
        services.AddFluentValidationAutoValidation(config => { config.OverrideDefaultResultFactoryWith<FvResultFactory>(); });
        return services;
    }

    private static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<LeaderboardContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("Leaderboard"), sqlOpt => { sqlOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); });
            });
        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(opt => { opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var appSettingModel = services.BuildServiceProvider().GetRequiredService<AppSetting>();
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1), //Skew default 5 minute
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingModel.Jwt.Key)),
                };
            });
        services.AddAuthorization();

        return services;
    }
}