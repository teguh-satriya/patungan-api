using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patungan.DataAccess.Entities;
using Patungan.Shared.Constants;

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
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(512)
                .IsRequired(false);

            builder.Property(t => t.Icon)
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("attach_money");
        }
    }
}
