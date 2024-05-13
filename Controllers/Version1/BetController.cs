using DerivcoAssessment.Models;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DerivcoAssessment.Controllers.Version1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BetController(ILogger<BetController> logger, IBetService betService) : Controller
    {
        private readonly ILogger<BetController> _logger = logger;
        private readonly IBetService _betService = betService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetBetsHistory()
        {
            try
            {
                var bets = await _betService.GetAllAsync();

                return Ok(bets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Bets history");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Bet>> PlaceBet([FromBody] BetDto betDto)
        {
            if (betDto == null)
            {
                return BadRequest("Invalid bet data");
            }

            string colour = betDto.Colour.ToString();
            if (colour != "Red" && colour != "Black" && colour != "Green")
            {
                return BadRequest($"Invalid bet colour. Must be Red, Black or Green. Please ensure is is capitalised. Colour given: {colour}");
            }

            double amount = betDto.Amount;
            if (amount <= 0)
            {
                return BadRequest($"Invalid bet amount. Amount must be greater than zero. Amount given: {amount}");
            }

            try
            {
                Bet createdBet = await _betService.PlaceBet(betDto);

                return CreatedAtAction(nameof(PlaceBet), new { id = createdBet.Id }, createdBet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while placing the bet");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
