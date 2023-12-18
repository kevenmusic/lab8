using System;
using System.Text;

namespace ClassLibrary8
{
    /// <summary>
    /// Класс, представляющий автобус в качестве наземного транспортного средства.
    /// </summary>
    public class Bus : GroundTransport
    {
        /// <summary>
        /// Получает или устанавливает количество мест в автобусе.
        /// </summary>
        public int BusSeats;

        private TransportKind _type = TransportKind.Автобус;
        /// <summary>
        /// Получает вид транспорта.
        /// </summary>
        public override TransportKind Kind
        {
            get { return _type; }
        }

        /// <summary>
        /// Конструктор класса Bus.
        /// </summary>
        /// <param name="flightNumber">Номер рейса.</param>
        /// <param name="busSeats">Количество свободных мест в автобусе.</param>
        /// <param name="departurePoint">Пункт отправления.</param>
        /// <param name="destination">Пункт назначения.</param>
        /// <param name="ticketPrices">Массив цен на билеты различного типа.</param>
        public Bus(int flightNumber, int busSeats, string departurePoint, string destination, double[] ticketPrices)
            : base(flightNumber, busSeats, departurePoint, destination, ticketPrices)
        {
            BusSeats = busSeats;
        }

        /// <summary>
        /// Выводит информацию о транспорте, включая цены на билеты различных типов.
        /// </summary>
        public override string DisplayInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine(base.DisplayInfo());
            infoBuilder.AppendLine($"Цена для билета типа \"мягкий\": {ticketPrices[0]}");
            infoBuilder.AppendLine($"Цена для билета типа \"жесткий\": {ticketPrices[1]}");

            return infoBuilder.ToString();
        }

        /// <summary>
        /// Индексатор для доступа к ценам на билеты различного типа. Переопределен для автобуса.
        /// </summary>
        /// <param name="index">Тип билета (мягкий или жесткий).</param>
        /// <returns>Цена на билет указанного типа.</returns>
        public override double this[string index]
        {
            get
            {
                switch (index.ToLower())
                {
                    case "мягкий":
                        return ticketPrices[0];
                    case "жесткий":
                        return ticketPrices[1];
                    default:
                        throw new TransportBusException($"Неверный индекс: ",index);
                }
            }
            set
            {
                switch (index.ToLower())
                {
                    case "мягкий":
                        ticketPrices[0] = value;
                        break;
                    case "жесткий":
                        ticketPrices[1] = value;
                        break;
                    default:
                        throw new TransportBusException($"Неверный индекс: ",index);
                }
            }
        }
    }
}
