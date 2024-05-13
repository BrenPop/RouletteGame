using DerivcoAssessment.Models;

namespace DerivcoAssessment.Services.Interfaces
{
    public interface IBetService : IBaseService<Bet>
    {
        Task<Bet> PlaceBet(BetDto betDto);

        Task<List<Bet>> GetPlacedBets();
    }
}