using DerivcoAssessment.Models;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DerivcoAssessment.Controllers.Version1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SpinController(ILogger<SpinController> logger, ISpinService spinService) : Controller
    {
        private readonly ILogger<SpinController> _logger = logger;
        private readonly ISpinService _spinService = spinService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetSpinsHistory()
        {
            try
            {
                var spins = await _spinService.GetAllAsync();

                return Ok(spins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Spins history");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Spin>> SpinRouletteWheel()
        {
            try
            {
                Spin spinResult = await _spinService.SpinRouletteWheel();

                return CreatedAtAction(nameof(Spin), new { id = spinResult.Id }, spinResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while spinning the Roulette wheel");

                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
