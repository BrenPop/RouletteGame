using DerivcoAssessment.Controllers.Version1;
using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Controllers
{
    public class SpinControllerTests
    {
        private Mock<ILogger<SpinController>> _mockLogger;

        private SpinController _spinController;
        private Mock<ISpinService> _mockSpinService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<SpinController>>();
            _mockSpinService = new Mock<ISpinService>();
            _spinController = new SpinController(_mockLogger.Object, _mockSpinService.Object);
        }

        [Test]
        public async Task GetSpinsHistory_ReturnsOkObjectResult_WithSpins()
        {
            // Arrange
            var spins = new List<Spin>
            {
                new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 12, Colour = BetColour.Red },
                new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 4, Colour = BetColour.Black },
                new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 23, Colour = BetColour.Green },
            };
            _mockSpinService.Setup(x => x.GetAllAsync()).ReturnsAsync(spins);

            // Act
            var actionResult = await _spinController.GetSpinsHistory();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(spins, Is.EqualTo(result?.Value));
        }

        [Test]
        public async Task GetSpinsHistory_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockSpinService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _spinController.GetSpinsHistory();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task SpinRouletteWheel_ReturnsCreatedAtActionResult_WithSpinResult()
        {
            // Arrange
            var spinResult = new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 12, Colour = BetColour.Red };
            _mockSpinService.Setup(x => x.SpinRouletteWheel()).ReturnsAsync(spinResult);

            // Act
            var result = await _spinController.SpinRouletteWheel();

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult?.ActionName, Is.EqualTo(nameof(SpinController.SpinRouletteWheel)));
            Assert.That(createdAtActionResult?.Value, Is.EqualTo(spinResult));
        }

        [Test]
        public async Task SpinRouletteWheel_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockSpinService.Setup(x => x.SpinRouletteWheel()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _spinController.SpinRouletteWheel();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
