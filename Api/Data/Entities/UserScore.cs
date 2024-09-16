namespace Api.Data.Entities;

public class UserScore
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relationship Models
    public User? User { get; set; }
}