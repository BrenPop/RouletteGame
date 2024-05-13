using DerivcoAssessment.Data;
using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DerivcoAssessment.Repositories
{
    public class BetRepository : BaseRepository<Bet>, IBetRepository
    {
        public BetRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<List<Bet>> GetPlacedBets()
        {
            return await _context.Set<Bet>()
                .Where(b => b.BetStatus == BetStatus.Placed)
                .ToListAsync();
        }
    }
}
