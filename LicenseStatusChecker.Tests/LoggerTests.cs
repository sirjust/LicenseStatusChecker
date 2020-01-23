using LicenseStatusChecker_Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        [Ignore] // This is a manual test
        public void WriteToConsole_ShouldWriteMessageToConsole()
        {
            // Arrange
            var logger = new Logger();
            var message = "This is a test.";

            // Act
            logger.WriteToConsole(message);

            // Assert
            // See that the message is in the output window
        }
    }
}
