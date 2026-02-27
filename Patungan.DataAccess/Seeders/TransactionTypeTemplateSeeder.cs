using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Seeders
{
    public static class TransactionTypeTemplateSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionTypeTemplateModel>().HasData(
                new TransactionTypeTemplateModel { Id = 1, Name = "Salary", Nature = TransactionNature.Income, Description = "Primary monthly income" },
                new TransactionTypeTemplateModel { Id = 2, Name = "Side Hustle", Nature = TransactionNature.Income, Description = "Secondary income streams" },
                new TransactionTypeTemplateModel { Id = 3, Name = "Investasi", Nature = TransactionNature.Income, Description = "" },
                new TransactionTypeTemplateModel { Id = 4, Name = "Belanja Kebutuhan", Nature = TransactionNature.Outcome, Description = "" },
                new TransactionTypeTemplateModel { Id = 5, Name = "Utilitas", Nature = TransactionNature.Outcome, Description = "" },
                new TransactionTypeTemplateModel { Id = 6, Name = "Hiburan", Nature = TransactionNature.Outcome, Description = "" }
            );
        }
    }
}