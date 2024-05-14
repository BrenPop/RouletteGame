using DerivcoAssessment.Controllers.Version1;
using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Controllers
{
    public class PayoutControllerTests
    {
        private Mock<ILogger<PayoutController>> _mockLogger;
        private PayoutController _payoutController;
        private Mock<IPayoutService> _mockPayoutService;
        private Mock<ISpinService> _mockSpinService;
        private Mock<IBetService> _mockBetService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<PayoutController>>();
            _mockPayoutService = new Mock<IPayoutService>();
            _mockSpinService = new Mock<ISpinService>();
            _mockBetService = new Mock<IBetService>();
            _payoutController = new PayoutController(_mockLogger.Object, _mockPayoutService.Object, _mockSpinService.Object, _mockBetService.Object);
        }

        [Test]
        public async Task GetPayoutsHistory_ReturnsOkObjectResult_WithPayouts()
        {
            // Arrange
            var payouts = new List<Payout> {
                new Payout { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 100.0, Spin = new Spin { /* Add spin data */ }, BetResult = BetResult.Win },
                new Payout { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 50.0, Spin = new Spin { /* Add spin data */ }, BetResult = BetResult.Loss },
            };
            _mockPayoutService.Setup(x => x.GetAllAsync()).ReturnsAsync(payouts);

            // Act
            var result = await _payoutController.GetPayoutsHistory();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(payouts, Is.EqualTo(okResult?.Value));
        }

        [Test]
        public async Task GetPayoutsHistory_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockPayoutService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _payoutController.GetPayoutsHistory();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task Payout_ReturnsCreatedAtActionResult_WithPayoutsResult()
        {
            // Arrange
            var spinResult = new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 12, Colour = BetColour.Red };
            var placedBets = new List<Bet> {
                new Bet { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 10.0, Colour = BetColour.Red, BetStatus = BetStatus.Placed, BetResult = BetResult.Pending },
                new Bet { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 20.0, Colour = BetColour.Black, BetStatus = BetStatus.Active, BetResult = BetResult.Pending },
            };
            var payoutsResult = new List<Payout> {
                new Payout { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 100.0, Spin = new Spin { /* Add spin data */ }, BetResult = BetResult.Win },
                new Payout { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Amount = 50.0, Spin = new Spin { /* Add spin data */ }, BetResult = BetResult.Loss },
            };
            _mockSpinService.Setup(x => x.GetLatestSpinResultAsync()).ReturnsAsync(spinResult);
            _mockBetService.Setup(x => x.GetPlacedBetsAsync()).ReturnsAsync(placedBets);
            _mockPayoutService.Setup(x => x.CalculateBetPayoutsAsync(spinResult, placedBets)).ReturnsAsync(payoutsResult);

            // Act
            var result = await _payoutController.Payout();

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult?.ActionName, Is.EqualTo(nameof(PayoutController.Payout)));
            Assert.That(createdAtActionResult?.Value, Is.EqualTo(payoutsResult));
        }

        [Test]
        public async Task Payout_ReturnsBadRequest_WhenSpinResultIsNull()
        {
            // Arrange
            _mockSpinService.Setup(x => x.GetLatestSpinResultAsync()).ReturnsAsync((Spin)null);

            // Act
            var result = await _payoutController.Payout();

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid spin data"));
        }

        [Test]
        public async Task Payout_ReturnsBadRequest_WhenPlacedBetsAreNullOrEmpty()
        {
            // Arrange
            _mockSpinService.Setup(x => x.GetLatestSpinResultAsync()).ReturnsAsync(new Spin());
            _mockBetService.Setup(x => x.GetPlacedBetsAsync()).ReturnsAsync(new List<Bet>());

            // Act
            var result = await _payoutController.Payout();

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.Not.Null);
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid bet list data"));
        }

        [Test]
        public async Task Payout_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockSpinService.Setup(x => x.GetLatestSpinResultAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _payoutController.Payout();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
