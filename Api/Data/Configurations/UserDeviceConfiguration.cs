using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable(EntityConstant.TableName.UserDevice);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();
        
        builder.Property(x => x.DeviceId)
            .HasColumnName("DeviceId")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.DeviceName)
            .HasColumnName("DeviceName")
            .IsRequired(false);
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserDevices)
            .HasForeignKey(x => x.UserId);
    }
}