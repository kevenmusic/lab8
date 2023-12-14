using System;
using System.Linq;
using System.Text;

namespace ClassLibrary8
{
    /// <summary>
    /// Класс, представляющий поезд в качестве наземного транспортного средства.
    /// </summary>
    public class Train : GroundTransport
    {
        /// <summary>
        /// Получает или устанавливает массив, представляющий количество мест в каждом вагоне поезда.
        /// </summary>
        public int[] WagonSeats { get; set; }

        /// <summary>
        /// Конструктор класса Train.
        /// </summary>
        /// <param name="flightNumber">Номер рейса.</param>
        /// <param name="wagonSeats">Массив, представляющий количество мест в каждом вагоне поезда.</param>
        /// <param name="departurePoint">Пункт отправления.</param>
        /// <param name="destination">Пункт назначения.</param>
        /// <param name="ticketPrices">Массив цен на билеты различного типа.</param>
        public Train(int flightNumber, int[] wagonSeats, string departurePoint, string destination, double[] ticketPrices)
            : base(flightNumber, CalculateTotalSeats(wagonSeats), departurePoint, destination, ticketPrices)
        {
            WagonSeats = wagonSeats;
        }

        /// <summary>
        /// Метод для вычисления общего количества мест в поезде на основе количества мест в каждом вагоне.
        /// </summary>
        /// <param name="wagonSeats">Массив, представляющий количество мест в каждом вагоне поезда.</param>
        /// <returns>Общее количество мест в поезде.</returns>
        public static int CalculateTotalSeats(int[] wagonSeats)
        {
            return wagonSeats.Sum();
        }

        /// <summary>
        /// Выводит информацию о транспорте, включая цены на билеты различных типов.
        /// </summary>
        public override string DisplayInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine(base.DisplayInfo());
            infoBuilder.AppendLine($"Цена для билета типа \"люкс\": {ticketPrices[0]}");
            infoBuilder.AppendLine($"Цена для билета типа \"купейный\": {ticketPrices[1]}");
            infoBuilder.AppendLine($"Цена для билета типа \"плацкартный\": {ticketPrices[2]}");
            infoBuilder.AppendLine($"Цена для билета типа \"общий\": {ticketPrices[3]}");

            return infoBuilder.ToString();
        }

        /// <summary>
        /// Индексатор для доступа к ценам на билеты различного типа. Переопределен для поезда.
        /// </summary>
        /// <param name="index">Тип билета (люкс, купейный, плацкартный, общий).</param>
        /// <returns>Цена на билет указанного типа.</returns>
        public override double this[string index]
        {
            get
            {
                switch (index.ToLower())
                {
                    case "люкс":
                        return ticketPrices[0];
                    case "купейный":
                        return ticketPrices[1];
                    case "плацкартный":
                        return ticketPrices[2];
                    case "общий":
                        return ticketPrices[3];
                    default:
                        throw new TransportTrainException($"Неверный индекс: ",index);
                }
            }
            set
            {
                switch (index.ToLower())
                {
                    case "люкс":
                        ticketPrices[0] = value;
                        break;
                    case "купейный":
                        ticketPrices[1] = value;
                        break;
                    case "плацкартный":
                        ticketPrices[2] = value;
                        break;
                    case "общий":
                        ticketPrices[3] = value;
                        break;
                    default:
                        throw new TransportTrainException($"Неверный индекс: ",index);
                }
            }
        }

        /// <summary>
        /// Получает количество свободных мест в поезде, суммируя количество мест в каждом вагоне.
        /// </summary>
        public override int FreeSeats
        {
            get { return WagonSeats.Sum(); }
        }
    }
}
