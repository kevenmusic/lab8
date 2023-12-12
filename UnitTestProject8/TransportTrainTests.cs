using ClassLibrary8;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject8
{
    [TestClass]
    public class TransportTrainTests
    {
        [TestMethod]
        public void Train_CalculateTotalSeats_ShouldReturnCorrectTotalSeats()
        {
            // Arrange
            int[] wagonSeats = { 20, 30, 40 };

            // Act
            int totalSeats = Train.CalculateTotalSeats(wagonSeats);

            // Assert
            Assert.AreEqual(90, totalSeats);
        }

        [TestMethod]
        public void Train_DisplayInfo_ShouldNotThrowException()
        {
            // Arrange
            Train train = new Train(1, new int[] { 20, 30, 40 }, "Город X", "Город Y", new double[] { 50, 75, 40, 20 });

            // Act & Assert
            try
            {
                train.DisplayInfo();
            }
            catch (TransportException ex)
            {
                Assert.Fail($"Ошибка: {ex.Message}");
            }
        }

        [TestMethod]
        public void Train_Indexer_ShouldReturnCorrectTicketPrice()
        {
            // Arrange
            Train train = new Train(1, new int[] { 20, 30, 40 }, "Город X", "Город Y", new double[] { 50, 75, 40, 20 });

            // Act
            double luxuryPrice = train["люкс"];

            // Assert
            Assert.AreEqual(50, luxuryPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(TransportTrainException))]
        public void Train_Indexer_InvalidIndex_ShouldThrowArgumentException()
        {
            // Arrange
            Train train = new Train(1, new int[] { 20, 30, 40 }, "Город X", "Город Y", new double[] { 50, 75, 40, 20 });

            // Act & Assert
            double price = train["неверный"];
        }
    }

}
