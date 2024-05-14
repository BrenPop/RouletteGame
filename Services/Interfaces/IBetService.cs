using DerivcoAssessment.Models;

namespace DerivcoAssessment.Services.Interfaces
{
    public interface IBetService : IBaseService<Bet>
    {
        Task<Bet> PlaceBetAsync(BetDto betDto);

        Task<List<Bet>> GetPlacedBetsAsync();
    }
}