using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services.Interfaces;
using DerivcoAssessment.Services;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Services
{
    [TestFixture]
    public class PayoutServiceTests
    {
        private PayoutService _payoutService;
        private Mock<IPayoutRepository> _payoutRepositoryMock;
        private Mock<IBetService> _betServiceMock;

        [SetUp]
        public void Setup()
        {
            _payoutRepositoryMock = new Mock<IPayoutRepository>();
            _betServiceMock = new Mock<IBetService>();
            _payoutService = new PayoutService(_payoutRepositoryMock.Object, _betServiceMock.Object);
        }

        [Test]
        public async Task CalculateBetPayouts_ReturnsListOfPayouts()
        {
            // Arrange
            Guid payoutId = Guid.NewGuid();
            DateTime timestamp = DateTime.Now;

            var spin = new Spin { Id = Guid.NewGuid(), CreatedAt = timestamp, UpdatedAt = timestamp, Number = 4, Colour = BetColour.Black };
            var placedBets = new List<Bet> {
                new Bet { Id = Guid.NewGuid(), Amount = 100.0, Colour = Enums.BetColour.Black, BetStatus = Enums.BetStatus.Placed, BetResult = BetResult.Pending, CreatedAt = timestamp, UpdatedAt = timestamp },
            };
            var expectedPayouts = new List<Payout> {
                new Payout { Id = payoutId, Amount = 200.0, BetResult = BetResult.Win, Spin = spin, CreatedAt = timestamp, UpdatedAt = timestamp }
            };
            var newPayout = new Payout { Id = payoutId, Amount = 200.0, Spin = spin, BetResult = BetResult.Win };
            var savedPayout = new Payout { Id = payoutId, Amount = 200.0, Spin = spin, BetResult = BetResult.Win, CreatedAt = timestamp, UpdatedAt = timestamp };

            _betServiceMock.SetupSequence(x => x.UpdateAsync(It.IsAny<Bet>()))
                .Returns(Task.CompletedTask)
                .Returns(Task.CompletedTask);

            _payoutRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Payout>())).ReturnsAsync(savedPayout);

            // Act
            var result = await _payoutService.CalculateBetPayouts(spin, placedBets);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0].Amount, Is.EqualTo(expectedPayouts[0].Amount));
            Assert.That(result[0].Spin, Is.EqualTo(expectedPayouts[0].Spin));
            Assert.That(result[0].BetResult, Is.EqualTo(expectedPayouts[0].BetResult));
        }

        [TestCase(BetColour.Red, BetColour.Red, 10.0, ExpectedResult = 20.0)]
        [TestCase(BetColour.Red, BetColour.Black, 10.0, ExpectedResult = -10.0)]
        [TestCase(BetColour.Green, BetColour.Green, 10.0, ExpectedResult = 140.0)]
        public double CalculateBetResult_ReturnsCorrectResult(BetColour spinColour, BetColour betColour, double amount)
        {
            // Arrange & Act
            var result = _payoutService.CalculateBetResult(spinColour, betColour, amount);

            // Assert
            return result;
        }
    }
}
