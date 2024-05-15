using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class PayoutService : BaseService<Payout>, IPayoutService
    {
        private readonly IBetService _betService;

        public PayoutService(IPayoutRepository repository, IBetService betService) : base(repository)
        {
            _betService = betService ?? throw new ArgumentNullException(nameof(betService));
        }

        public async Task<List<Payout>> CalculateBetPayoutsAsync(Spin spin, List<Bet> placedBets)
        {
            var payouts = new List<Payout>();

            foreach (var bet in placedBets)
            {
                await UpdateBetStatusAsync(bet, spin, BetStatus.Active);

                var betPayoutAmount = CalculateBetResult(spin.Colour, bet.Colour, bet.Amount);
                var payout = CreatePayout(spin, betPayoutAmount);
                var savedPayout = await AddAsync(payout);

                payouts.Add(savedPayout);

                await FinalizeBetAsync(bet, savedPayout);
            }

            return payouts;
        }

        private async Task UpdateBetStatusAsync(Bet bet, Spin spin, BetStatus status)
        {
            bet.Spin = spin;
            bet.BetStatus = status;

            await _betService.UpdateAsync(bet);
        }

        private Payout CreatePayout(Spin spin, double betPayoutAmount)
        {
            return new Payout
            {
                Amount = betPayoutAmount,
                Spin = spin,
                BetResult = betPayoutAmount >= 0 ? BetResult.Win : BetResult.Loss
            };
        }

        private async Task FinalizeBetAsync(Bet bet, Payout payout)
        {
            bet.Payout = payout;
            bet.BetStatus = BetStatus.Completed;

            await _betService.UpdateAsync(bet);
        }

        public double CalculateBetResult(BetColour spinColour, BetColour betColour, double amount)
        {
            return spinColour == betColour
                ? (betColour == BetColour.Green ? amount * 14 : amount * 2)
                : -amount;
        }
    }
}
