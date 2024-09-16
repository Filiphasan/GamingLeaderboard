using Api;
using Api.Common.Model.Options;
using Api.HealthCheck;
using Api.Helper;
using Api.Middleware;
using Carter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
LoggerHelper.ConfigureSeriLog(builder.Configuration.GetSection(AppSetting.SectionName).Get<AppSetting>());

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddApi(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddCheck<LeaderboardHealthCheck>(LeaderboardHealthCheck.Name);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.MapCarter();
app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    ResponseWriter = LeaderboardHealthCheckResponseWriter.WriteResponse
});

await app.RunAsync();