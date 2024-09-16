namespace Api.Common.Model.Options;

public class AppSetting
{
    public const string SectionName = "Settings";

    public required AppSettingRedis Redis { get; set; }
    public required AppSettingElastic Elastic { get; set; }
    public required AppSettingJwt Jwt { get; set; }
}

public class AppSettingRedis
{
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Password { get; set; }
    public required int Database { get; set; }
}

public class AppSettingElastic
{
    public required string Index { get; set; }
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class AppSettingJwt
{
    public required string Key { get; set; }
    public required int ExpiresInMinutes { get; set; }
    public required int RefreshInMinutes { get; set; }
}