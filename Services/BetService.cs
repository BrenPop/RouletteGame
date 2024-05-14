using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;

namespace DerivcoAssessment.Services
{
    public class BetService : BaseService<Bet>, IBetService
    {
        private readonly IBetRepository _repository;

        public BetService(IBetRepository repository) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Bet> PlaceBetAsync(BetDto betDto)
        {
            if (betDto == null)
            {
                throw new ArgumentNullException(nameof(betDto), "BetDto cannot be null.");
            }

            try
            {
                var bet = new Bet
                {
                    Amount = betDto.Amount,
                    Colour = Enum.Parse<BetColour>(betDto.Colour),
                    BetStatus = BetStatus.Placed,
                    BetResult = BetResult.Pending
                };

                return await AddAsync(bet);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Invalid bet color specified.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to place the bet.", ex);
            }
        }

        public async Task<List<Bet>> GetPlacedBetsAsync()
        {
            return await _repository.GetPlacedBets();
        }
    }
}
