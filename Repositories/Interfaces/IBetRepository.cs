using DerivcoAssessment.Models;

namespace DerivcoAssessment.Repositories.Interfaces
{
    public interface IBetRepository : IBaseRepository<Bet>
    {
        Task<List<Bet>> GetPlacedBets();
    }
}
