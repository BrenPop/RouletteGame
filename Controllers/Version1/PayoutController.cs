using DerivcoAssessment.Models;
using DerivcoAssessment.Services;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DerivcoAssessment.Controllers.Version1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PayoutController(ILogger<PayoutController> logger, IPayoutService payoutService, ISpinService spinService, IBetService betService) : Controller
    {
        private readonly ILogger<PayoutController> _logger = logger;
        private readonly IPayoutService _payoutService = payoutService;
        private readonly ISpinService _spinService = spinService;
        private readonly IBetService _betService = betService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payout>>> GetPayoutsHistory()
        {
            try
            {
                var payouts = await _payoutService.GetAllAsync();

                return Ok(payouts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Payouts history");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Payout>>> Payout()
        {
            try
            {
                Spin? latestSpin = await _spinService.GetLatestSpinResult();

                if (latestSpin == null)
                {
                    return BadRequest("Invalid spin data");
                }

                List<Bet> placedBets = await _betService.GetPlacedBets();

                if (placedBets == null || placedBets.Count <= 0)
                {
                    return BadRequest("Invalid bet list data");
                }

                List<Payout> payoutsResult = await _payoutService.CalculateBetPayouts(latestSpin, placedBets);

                return CreatedAtAction(nameof(Payout), payoutsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the payouts");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
