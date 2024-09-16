namespace Api.Data.Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relationship Models
    public ICollection<UserDevice> UserDevices { get; set; } = [];
    public ICollection<UserScore> UserScores { get; set; } = [];
}