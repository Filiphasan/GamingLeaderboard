using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class UserScoreConfiguration : IEntityTypeConfiguration<UserScore>
{
    public void Configure(EntityTypeBuilder<UserScore> builder)
    {
        builder.ToTable(EntityConstant.TableName.UserScore);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.Score)
            .HasColumnName("Score")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserScores)
            .HasForeignKey(x => x.UserId);

        builder.HasIndex(x => new { x.UserId, x.Score }, "IX_UserScore_UserId_Score")
            .IsUnique(false);
    }
}