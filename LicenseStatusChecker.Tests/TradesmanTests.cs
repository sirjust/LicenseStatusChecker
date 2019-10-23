using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LicenseStatusChecker.Tests
{
    [TestClass]
    public class TradesmanTests
    {
        [TestMethod]
        public void GetHoursNeeded_ShouldReturnCorrectHours()
        {
            // Arrange
            var tradesman = TestHelper.GetMockTradesman();
            tradesman.Trade = "Plumber";
            var expected = 16;

            // Act
            var actual = tradesman.GetHoursNeeded(tradesman.Trade);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
