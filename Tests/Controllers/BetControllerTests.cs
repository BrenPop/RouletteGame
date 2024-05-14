using DerivcoAssessment.Controllers.Version1;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Controllers
{
    public class BetControllerTests
    {
        private Mock<ILogger<BetController>> _mockLogger;

        private BetController _betController;
        private Mock<IBetService> _mockBetService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<BetController>>();
            _mockBetService = new Mock<IBetService>();
            _betController = new BetController(_mockLogger.Object, _mockBetService.Object);
        }

        [Test]
        public async Task Post_CreatesNewBet()
        {
            // Arrange
            BetDto newBetDto = new BetDto { Amount = 50000, Colour = "Black" };
            Bet responseBet = new Bet { Id = Guid.NewGuid(), Amount = 5000, Colour = Enums.BetColour.Black, BetStatus = Enums.BetStatus.Placed };

            _mockBetService.Setup(service => service.PlaceBet(newBetDto)).ReturnsAsync(responseBet);

            // Act
            var result = await _betController.PlaceBet(newBetDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult?.ActionName, Is.EqualTo(nameof(BetController.PlaceBet)));
            Assert.That(createdAtActionResult?.Value, Is.EqualTo(responseBet));

        }

        [Test]
        public async Task GetBetsHistory_ReturnsOkObjectResult_WithBets()
        {
            // Arrange
            var bets = new List<Bet>
            {
                new Bet { Id = Guid.NewGuid(), Amount = 50000, Colour = Enums.BetColour.Red },
                new Bet { Id = Guid.NewGuid(), Amount = 100000, Colour = Enums.BetColour.Black },
                new Bet { Id = Guid.NewGuid(), Amount = 35000, Colour = Enums.BetColour.Green }
            };
            _mockBetService.Setup(x => x.GetAllAsync()).ReturnsAsync(bets);

            // Act
            var result = await _betController.GetBetsHistory();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(bets, Is.EqualTo(okResult?.Value));
        }

        [Test]
        public async Task GetBetsHistory_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockBetService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _betController.GetBetsHistory();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task PlaceBet_ReturnsCreatedAtActionResult_WithCreatedBet()
        {
            // Arrange
            var betDto = new BetDto { Amount = 50000, Colour = "Black" };
            var createdBet = new Bet { Id = Guid.NewGuid(), Amount = 5000, Colour = Enums.BetColour.Black, BetStatus = Enums.BetStatus.Placed };
            _mockBetService.Setup(x => x.PlaceBet(betDto)).ReturnsAsync(createdBet);

            // Act
            var result = await _betController.PlaceBet(betDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult?.ActionName, Is.EqualTo(nameof(BetController.PlaceBet)));
            Assert.That(createdAtActionResult?.Value, Is.EqualTo(createdBet));
        }

        [Test]
        public async Task PlaceBet_ReturnsBadRequest_WhenBetDtoIsNull()
        {
            // Act
            var result = await _betController.PlaceBet(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid bet data"));
        }

        [Test]
        public async Task PlaceBet_ReturnsBadRequest_WhenBetColourIsInvalid()
        {
            // Arrange
            var invalidBetDto = new BetDto { Amount = 100, Colour = "Blue" };

            // Act
            var result = await _betController.PlaceBet(invalidBetDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult?.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task PlaceBet_ReturnsBadRequest_WhenBetAmountIsInvalid()
        {
            // Arrange
            var invalidBetDto = new BetDto { Amount = -100, Colour = "Green" };

            // Act
            var result = await _betController.PlaceBet(invalidBetDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult?.Value, Is.TypeOf<string>());
        }
    }
}
