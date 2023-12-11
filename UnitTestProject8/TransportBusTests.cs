using ClassLibrary8;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject8
{
    [TestClass]
    public class TransportBusTests
    {
        [TestMethod]
        public void Bus_DisplayInfo_ShouldNotThrowException()
        {
            // Arrange
            Bus bus = new Bus(1, 40, "Город P", "Город Q", new double[] { 30, 25 });

            // Act & Assert
            try
            {
                bus.DisplayInfo();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Ошибка: {ex.Message}");
            }
        }

        [TestMethod]
        public void Bus_Indexer_ShouldReturnCorrectTicketPrice()
        {
            // Arrange
            Bus bus = new Bus(1, 40, "Город P", "Город Q", new double[] { 30, 25 });

            // Act
            double softPrice = bus["мягкий"];

            // Assert
            Assert.AreEqual(30, softPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Bus_Indexer_InvalidIndex_ShouldThrowArgumentException()
        {
            // Arrange
            Bus bus = new Bus(1, 40, "Город P", "Город Q", new double[] { 30, 25 });

            // Act & Assert
            double price = bus["неверный"];
        }
    }
}
