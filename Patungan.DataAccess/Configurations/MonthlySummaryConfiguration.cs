using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Configurations
{
    public class MonthlySummaryConfiguration
    {
        public static void Configure(EntityTypeBuilder<MonthlySummaryModel> builder)
        {
            builder.ToTable("MonthlySummary");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.UserId)
                .IsRequired();

            builder.Property(m => m.Year)
                .IsRequired();

            builder.Property(m => m.Month)
                .IsRequired();

            builder.Property(m => m.StartingBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(m => m.EndingBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(m => m.CarriedOver)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                .IsRequired();

            builder.HasIndex(m => new { m.UserId, m.Year, m.Month })
                .IsUnique();

            builder.HasOne(m => m.User)
                .WithMany(u => u.MonthlySummaries)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Transactions)
                .WithOne(t => t.MonthlySummary)
                .HasForeignKey(t => t.MonthlySummaryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}