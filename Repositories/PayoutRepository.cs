using DerivcoAssessment.Data;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;

namespace DerivcoAssessment.Repositories
{
    public class PayoutRepository : BaseRepository<Payout>, IPayoutRepository
    {
        public PayoutRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
