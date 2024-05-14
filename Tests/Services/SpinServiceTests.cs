using DerivcoAssessment.Enums;
using DerivcoAssessment.Models;
using DerivcoAssessment.Repositories.Interfaces;
using DerivcoAssessment.Services;
using Moq;
using NUnit.Framework;

namespace DerivcoAssessment.Tests.Services
{
    [TestFixture]
    public class SpinServiceTests
    {
        private SpinService _spinService;
        private Mock<ISpinRepository> _spinRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _spinRepositoryMock = new Mock<ISpinRepository>();
            _spinService = new SpinService(_spinRepositoryMock.Object);
        }

        [Test]
        public async Task SpinRouletteWheel_ReturnsSpinObject()
        {
            // Arrange
            var expectedSpin = new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 12, Colour = BetColour.Red };
            _spinRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Spin>())).ReturnsAsync(expectedSpin);

            // Act
            var result = await _spinService.SpinRouletteWheel();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSpin));
        }

        [Test]
        public async Task GetLatestSpinResult_ReturnsLatestSpin()
        {
            // Arrange
            var expectedSpin = new Spin { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Number = 4, Colour = BetColour.Black };
            _spinRepositoryMock.Setup(x => x.GetLatestSpinResult()).ReturnsAsync(expectedSpin);

            // Act
            var result = await _spinService.GetLatestSpinResult();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedSpin));
        }

        [Test]
        public async Task GetLatestSpinResult_ReturnsNull_WhenNoSpinResultExists()
        {
            // Arrange
            _spinRepositoryMock.Setup(x => x.GetLatestSpinResult()).ReturnsAsync((Spin)null);

            // Act
            var result = await _spinService.GetLatestSpinResult();

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
