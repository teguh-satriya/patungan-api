using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patungan.DataAccess.Entities;
using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Configurations
{
    public class TransactionTypeConfiguration
    {
        public static void Configure(EntityTypeBuilder<TransactionTypeModel> builder)
        {
            builder.ToTable("TransactionType");

            builder.HasKey(t => t.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Nature)
                .HasColumnType("public.transaction_nature")
                .HasConversion(
                    v => v.ToString().ToLowerInvariant(),
                    v => Enum.Parse<TransactionNature>(v, true))
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(512)
                .IsRequired(false);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(u => u.TransactionTypes)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Transactions)
                .WithOne(t => t.TransactionType)
                .HasForeignKey(t => t.TransactionTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}