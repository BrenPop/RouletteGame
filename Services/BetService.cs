using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class BetService : BaseService<Bet>, IBetService
    {
        protected new readonly IBetRepository _repository;

        public BetService(IBetRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Bet> PlaceBet(BetDto betDto)
        {
            try
            {
                Bet bet = new Bet
                {
                    Amount = betDto.Amount,
                    Colour = (BetColour)Enum.Parse(typeof(BetColour), betDto.Colour),
                    BetStatus = BetStatus.Placed,
                    BetResult = BetResult.Pending
                };

                var test = await AddAsync(bet);

                return await AddAsync(bet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Bet>> GetPlacedBets()
        {
            return await _repository.GetPlacedBets();
        }
    }
}
