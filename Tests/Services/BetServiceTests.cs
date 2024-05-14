using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services;
using DerivcoAssessment.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Services
{
    [TestFixture]
    public class BetServiceTests
    {
        private BetService _betService;
        private Mock<IBetRepository> _betRepositoryMock;
        private Mock<IBetService> _betServiceMock;

        [SetUp]
        public void Setup()
        {
            _betRepositoryMock = new Mock<IBetRepository>();
            _betServiceMock = new Mock<IBetService>();
            _betService = new BetService(_betRepositoryMock.Object);
        }

        [Test]
        public async Task PlaceBetAsync_ReturnsBet_WhenValidBetDtoIsProvided()
        {
            // Arrange
            Guid Id = Guid.NewGuid();
            BetDto betDto = new BetDto { 
                Amount = 10.0, 
                Colour = "Red" 
            };
            Bet newBet = new Bet { 
                Id = Id, Amount = 10.0, 
                Colour = Enums.BetColour.Red, 
                BetStatus = Enums.BetStatus.Placed, 
                BetResult = BetResult.Pending 
            };
            Bet responseBet = new Bet { 
                Id = Id, 
                Amount = 10.0, 
                Colour = Enums.BetColour.Red, 
                BetStatus = Enums.BetStatus.Placed, 
                BetResult = BetResult.Pending, 
                CreatedAt = DateTime.Now, 
                UpdatedAt = DateTime.Now 
            };

            _betServiceMock.Setup(service => service.AddAsync(newBet)).ReturnsAsync(responseBet);
            _betRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Bet>())).ReturnsAsync(responseBet);

            // Act
            var result = await _betService.PlaceBetAsync(betDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Amount, Is.EqualTo(betDto.Amount));
            Assert.That(result.Colour, Is.EqualTo(BetColour.Red));
            Assert.That(result.BetStatus, Is.EqualTo(BetStatus.Placed));
            Assert.That(result.BetResult, Is.EqualTo(BetResult.Pending));
            _betRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Bet>()), Times.Once);
        }

        [Test]
        public void PlaceBetAsync_ThrowsException_WhenInvalidColourIsProvided()
        {
            // Arrange
            var invalidBetDto = new BetDto { Amount = 10.0, Colour = "InvalidColour" };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _betService.PlaceBetAsync(invalidBetDto));
            Assert.That(exception.Message, Is.EqualTo("Invalid bet color specified."));
            _betRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Bet>()), Times.Never);
        }

        [Test]
        public async Task GetPlacedBetsAsync_ReturnsListOfBets()
        {
            // Arrange
            var expectedBets = new List<Bet> {
                new Bet { Id = Guid.NewGuid(), Amount = 50000, Colour = Enums.BetColour.Red },
                new Bet { Id = Guid.NewGuid(), Amount = 100000, Colour = Enums.BetColour.Black },
                new Bet { Id = Guid.NewGuid(), Amount = 35000, Colour = Enums.BetColour.Green }
            };
            _betRepositoryMock.Setup(x => x.GetPlacedBets()).ReturnsAsync(expectedBets);

            // Act
            var result = await _betService.GetPlacedBetsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedBets));
        }

        [Test]
        public async Task GetPlacedBetsAsync_ReturnsEmptyList_WhenNoBetsArePlaced()
        {
            // Arrange
            _betRepositoryMock.Setup(x => x.GetPlacedBets()).ReturnsAsync(new List<Bet>());

            // Act
            var result = await _betService.GetPlacedBetsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }
    }
}
