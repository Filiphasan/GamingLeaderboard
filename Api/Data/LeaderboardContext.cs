using System.Reflection;
using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class LeaderboardContext(DbContextOptions<LeaderboardContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserDevice> UserDevices { get; set; }
    public DbSet<UserScore> UserScores { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}