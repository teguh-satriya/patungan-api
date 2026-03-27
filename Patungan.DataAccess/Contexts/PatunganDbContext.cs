using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Configurations;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Seeders;
using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Contexts
{
    public class PatunganDbContext:DbContext
    {
        public PatunganDbContext(DbContextOptions<PatunganDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<TransactionNature>("public","transaction_nature");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatunganDbContext).Assembly);

            TransactionTypeTemplateSeeder.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);

            UserConfiguration.Configure(modelBuilder.Entity<UserModel>());
            TransactionTypeTemplateConfiguration.Configure(modelBuilder.Entity<TransactionTypeTemplateModel>());
            TransactionTypeConfiguration.Configure(modelBuilder.Entity<TransactionTypeModel>());
            TransactionConfiguration.Configure(modelBuilder.Entity<TransactionModel>());
            MonthlySummaryConfiguration.Configure(modelBuilder.Entity<MonthlySummaryModel>());
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TransactionTypeTemplateModel> TransactionTypeTemplates { get; set; }
        public DbSet<TransactionTypeModel> TransactionTypes { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<MonthlySummaryModel> MonthlySummaries { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
    }
}
