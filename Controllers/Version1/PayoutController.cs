using DerivcoAssessment.Models;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DerivcoAssessment.Controllers.Version1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PayoutController : ControllerBase
    {
        private readonly ILogger<PayoutController> _logger;
        private readonly IPayoutService _payoutService;
        private readonly ISpinService _spinService;
        private readonly IBetService _betService;

        public PayoutController(ILogger<PayoutController> logger, IPayoutService payoutService, ISpinService spinService, IBetService betService)
        {
            _logger = logger;
            _payoutService = payoutService;
            _spinService = spinService;
            _betService = betService;
        }

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
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Payout>>> Payout()
        {
            try
            {
                var latestSpin = await _spinService.GetLatestSpinResultAsync();

                if (latestSpin == null)
                {
                    return BadRequest("Invalid spin data");
                }

                var placedBets = await _betService.GetPlacedBetsAsync();

                if (placedBets == null || placedBets.Count == 0)
                {
                    return BadRequest("Invalid bet list data");
                }

                var payoutsResult = await _payoutService.CalculateBetPayoutsAsync(latestSpin, placedBets);
                return CreatedAtAction(nameof(Payout), payoutsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the payouts");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing the request.");
            }
        }
    }
}
