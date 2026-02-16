using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Configurations
{
    public class TransactionTypeTemplateConfiguration
    {
        public static void Configure(EntityTypeBuilder<TransactionTypeTemplateModel> builder)
        {
            builder.ToTable("TransactionTypeTemplate");

            builder.HasKey(t => t.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Nature)
                .HasColumnType("public.transaction_nature")
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(512)
                .IsRequired(false);
        }
    }
}
