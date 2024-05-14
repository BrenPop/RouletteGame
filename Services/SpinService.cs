using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class SpinService : BaseService<Spin>, ISpinService
    {
        protected new readonly ISpinRepository _repository;

        public SpinService(ISpinRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Spin> SpinRouletteWheelAsync()
        {
            Random random = new Random();
            int rouletteNumber = random.Next(0, 37);

            BetColour rouletteColor = GetRouletteColor(rouletteNumber);

            return await AddAsync(new Spin
            {
                Number = rouletteNumber,
                Colour = rouletteColor
            });
        }

        public async Task<Spin?> GetLatestSpinResultAsync()
        {
            return await _repository.GetLatestSpinResult();
        }

        #region Private Functions
        private BetColour GetRouletteColor(int number)
        {
            if (number == 0)
            {
                return BetColour.Green;
            }
            else if ((number >= 1 && number <= 10) || (number >= 19 && number <= 28))
            {
                return (number % 2 == 0) ? BetColour.Black : BetColour.Red;
            }
            else
            {
                return (number % 2 == 0) ? BetColour.Red : BetColour.Black;
            }
        }
        #endregion
    }
}
