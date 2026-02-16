using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Configurations
{
    public class UserConfiguration
    {
        public static void Configure(EntityTypeBuilder<UserModel> entity)
        {
            entity.ToTable("User");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Email)
                .IsRequired() 
                .HasMaxLength(100);
            
            entity.Property(e => e.PasswordHash);
            
            entity.Property(e => e.GoogleId);
            
            entity.Property(e=> e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .ValueGeneratedOnAdd();
        }
    }
}
