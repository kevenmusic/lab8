using ClassLibrary8;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject8
{
    [TestClass]
    public class TransportAirplaneTests
    {
        [TestMethod]
        public void Airplane_DisplayInfo_ShouldNotThrowException()
        {
            // Arrange
            Airplane airplane = new Airplane(1, 200, "Город M", "Город N", new double[] { 100, 200, 300 });

            // Act & Assert
            try
            {
                airplane.DisplayInfo();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Ошибка: {ex.Message}");
            }
        }

        [TestMethod]
        public void Airplane_Indexer_ShouldReturnCorrectTicketPrice()
        {
            // Arrange
            Airplane airplane = new Airplane(1, 200, "Город M", "Город N", new double[] { 100, 200, 300 });

            // Act
            double economyPrice = airplane["эконом"];

            // Assert
            Assert.AreEqual(100, economyPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Airplane_Indexer_InvalidIndex_ShouldThrowArgumentException()
        {
            // Arrange
            Airplane airplane = new Airplane(1, 200, "Город M", "Город N", new double[] { 100, 200, 300 });

            // Act & Assert
            double price = airplane["неверный"];
        }
    }
}
