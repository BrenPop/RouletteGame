using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class SpinService : BaseService<Spin>, ISpinService
    {
        private readonly ISpinRepository _repository;
        private static readonly Random _random = new Random();

        public SpinService(ISpinRepository repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Spin> SpinRouletteWheelAsync()
        {
            int rouletteNumber = _random.Next(0, 37);
            BetColour rouletteColor = GetRouletteColor(rouletteNumber);

            var spin = new Spin
            {
                Number = rouletteNumber,
                Colour = rouletteColor
            };

            return await AddAsync(spin);
        }

        public async Task<Spin?> GetLatestSpinResultAsync()
        {
            return await _repository.GetLatestSpinResult();
        }

        private BetColour GetRouletteColor(int number)
        {
            return number switch
            {
                0 => BetColour.Green,
                _ when (number >= 1 && number <= 10) || (number >= 19 && number <= 28) => number % 2 == 0 ? BetColour.Black : BetColour.Red,
                _ => number % 2 == 0 ? BetColour.Red : BetColour.Black,
            };
        }
    }
}
