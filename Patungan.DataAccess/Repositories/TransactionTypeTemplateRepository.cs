using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Patungan.DataAccess.Repositories
{
    public class TransactionTypeTemplateRepository:ITransactionTypeTemplateRepository
    {
        private readonly PatunganDbContext _context;
        public TransactionTypeTemplateRepository(PatunganDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<TransactionTypeTemplateModel>> GetDefaultTemplatesAsync()
        {
            var templates = await _context.TransactionTypeTemplates
                .Select(t => new TransactionTypeTemplateModel
                {
                    Name = t.Name,
                    Nature = t.Nature,
                    Description = t.Description
                })
                .AsNoTracking()
                .ToListAsync();

            return templates;
        }
    }
}
