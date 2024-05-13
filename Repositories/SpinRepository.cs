using DerivcoAssessment.Data;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DerivcoAssessment.Repositories
{
    public class SpinRepository : BaseRepository<Spin>, ISpinRepository
    {
        public SpinRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<Spin?> GetLatestSpinResult()
        {
            return await _context.Set<Spin>()
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
