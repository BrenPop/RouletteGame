using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class PayoutService : BaseService<Payout>, IPayoutService
    {
        protected new readonly IPayoutRepository _repository;

        protected readonly IBetService _betService;

        public PayoutService(IPayoutRepository repository, IBetService betService) : base(repository)
        {
            _repository = repository;
            _betService = betService;
        }

        public async Task<List<Payout>> CalculateBetPayouts(Spin spin, List<Bet> placedBets)
        {
            var payouts = new List<Payout>();

            foreach (var bet in placedBets)
            {
                bet.Spin = spin;
                bet.BetStatus = BetStatus.Active;
                await _betService.UpdateAsync(bet);

                var betResult = CalculateBetResult(spin.Colour, bet.Colour, bet.Amount);
                var payout = new Payout
                {
                    Amount = betResult,
                    Spin = spin,
                    BetResult = betResult >= 0 ? BetResult.Win : BetResult.Loss
                };

                Payout savedPayout = await AddAsync(payout);

                payouts.Add(savedPayout);

                bet.Payout = savedPayout;
                bet.BetStatus = BetStatus.Completed;
                await _betService.UpdateAsync(bet);
            }

            return payouts;
        }

        public double CalculateBetResult(BetColour spinColour, BetColour betColour, double amount)
        {
            if (spinColour == betColour)
            {
                if (betColour == BetColour.Green)
                {
                    return amount * 14; // 14:1 payout for green
                }
                else
                {
                    return amount * 2; // 1:1 payout for red or black
                }
            }
            else
            {
                return -amount; // Player loses the amount they bet
            }
        }
    }
}
