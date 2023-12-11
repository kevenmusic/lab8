﻿using System;
using System.Text;

namespace ClassLibrary8
{
    /// <summary>
    /// Абстрактный класс, представляющий наземное транспортное средство.
    /// </summary>
    public abstract class GroundTransport : ITransport
    {
        /// <summary>
        /// Получает или устанавливает номер рейса наземного транспортного средства.
        /// </summary>
        public int FlightNumber { get; set; }

        /// <summary>
        /// Получает или устанавливает количество свободных мест в наземном транспортном средстве.
        /// </summary>
        public virtual int FreeSeats { get; set; }

        /// <summary>
        /// Получает или устанавливает точку отправления наземного транспортного средства.
        /// </summary>
        public string DeparturePoint { get; set; }

        /// <summary>
        /// Получает или устанавливает пункт назначения наземного транспортного средства.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Получает тип транспортного средства (в данном случае - "Наземный").
        /// </summary>
        public string TransportType
        {
            get { return "Наземный"; }
        }

        /// <summary>
        /// Массив, представляющий цены на билеты различного типа.
        /// </summary>
        protected double[] ticketPrices;

        /// <summary>
        /// Конструктор абстрактного класса GroundTransport.
        /// </summary>
        /// <param name="flightNumber">Номер рейса.</param>
        /// <param name="freeSeats">Количество свободных мест.</param>
        /// <param name="departurePoint">Пункт отправления.</param>
        /// <param name="destination">Пункт назначения.</param>
        /// <param name="ticketPrices">Массив цен на билеты различного типа.</param>
        protected GroundTransport(int flightNumber, int freeSeats, string departurePoint, string destination, double[] ticketPrices)
        {
            FlightNumber = flightNumber;
            FreeSeats = freeSeats;
            DeparturePoint = departurePoint;
            Destination = destination;
            this.ticketPrices = ticketPrices;
        }

        /// <summary>
        /// Отображает информацию о наземном транспортном средстве.
        /// </summary>
        public virtual string DisplayInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine($"Вид транспорта: {TransportType}");
            infoBuilder.AppendLine($"Номер рейса: {FlightNumber}");
            infoBuilder.AppendLine($"Пункт отправления: {DeparturePoint}");
            infoBuilder.AppendLine($"Пункт назначения: {Destination}");
            infoBuilder.AppendLine($"Количество свободных мест: {FreeSeats}");

            return infoBuilder.ToString();
        }

        /// <summary>
        /// Абстрактный индексатор для доступа к ценам на билеты различного типа.
        /// </summary>
        /// <param name="index">Тип билета (мягкий или жесткий).</param>
        /// <returns>Цена на билет указанного типа.</returns>
        public abstract double this[string index] { get; set; }
    }
}