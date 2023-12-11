using System;
using System.Text;

namespace ClassLibrary8
{
    /// <summary>
    /// Класс, представляющий самолет в качестве воздушного транспортного средства.
    /// </summary>
    public class Airplane : ITransport
    {
        /// <summary>
        /// Получает или устанавливает номер рейса самолета.
        /// </summary>
        public int FlightNumber { get; set; }

        /// <summary>
        /// Получает или устанавливает количество свободных мест в самолете.
        /// </summary>
        public int FreeSeats { get; set; }

        /// <summary>
        /// Получает или устанавливает точку отправления самолета.
        /// </summary>
        public string DeparturePoint { get; set; }

        /// <summary>
        /// Получает или устанавливает пункт назначения самолета.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Массив, представляющий цены на билеты различного класса в самолете.
        /// </summary>
        private double[] ticketPrices;

        /// <summary>
        /// Получает тип транспортного средства (в данном случае - "Воздушный").
        /// </summary>
        public string TransportType
        {
            get { return "Воздушный"; }
        }

        /// <summary>
        /// Конструктор класса Airplane.
        /// </summary>
        /// <param name="flightNumber">Номер рейса.</param>
        /// <param name="freeSeats">Количество свободных мест в самолете.</param>
        /// <param name="departurePoint">Пункт отправления.</param>
        /// <param name="destination">Пункт назначения.</param>
        /// <param name="ticketPrices">Массив цен на билеты различного класса.</param>
        public Airplane(int flightNumber, int freeSeats, string departurePoint, string destination, double[] ticketPrices)
        {
            FlightNumber = flightNumber;
            FreeSeats = freeSeats;
            DeparturePoint = departurePoint;
            Destination = destination;
            this.ticketPrices = ticketPrices;
        }

        /// <summary>
        /// Отображает информацию о самолете.
        /// </summary>
        public string DisplayInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine($"Вид транспорта: {TransportType}");
            infoBuilder.AppendLine($"Номер рейса: {FlightNumber}");
            infoBuilder.AppendLine($"Пункт отправления: {DeparturePoint}");
            infoBuilder.AppendLine($"Пункт назначения: {Destination}");
            infoBuilder.AppendLine($"Количество свободных мест: {FreeSeats}");
            infoBuilder.AppendLine($"Цена для билета типа \"эконом\": {ticketPrices[0]}");
            infoBuilder.AppendLine($"Цена для билета типа \"бизнес\": {ticketPrices[1]}");
            infoBuilder.AppendLine($"Цена для билета типа \"первый\": {ticketPrices[2]}");

            return infoBuilder.ToString();
        }

        /// <summary>
        /// Индексатор для доступа к ценам на билеты различного класса. Переопределен для самолета.
        /// </summary>
        /// <param name="index">Класс билета (эконом, бизнес, первый).</param>
        /// <returns>Цена на билет указанного класса.</returns>
        public double this[string index]
        {
            get
            {
                switch (index.ToLower())
                {
                    case "эконом":
                        return ticketPrices[0];
                    case "бизнес":
                        return ticketPrices[1];
                    case "первый":
                        return ticketPrices[2];
                    default:
                        throw new TransportAirplaneException($"Неверный индекс: {index}");
                }
            }
        }
    }
}
