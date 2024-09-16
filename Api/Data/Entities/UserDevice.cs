namespace Api.Data.Entities;

public class UserDevice
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Guid DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relationship Models
    public User? User { get; set; }
}