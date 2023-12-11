namespace ClassLibrary8
{
    /// <summary>
    /// Интерфейс, представляющий транспортное средство.
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// Получает или устанавливает номер рейса транспортного средства.
        /// </summary>
        int FlightNumber { get; set; }

        /// <summary>
        /// Получает количество свободных мест в транспортном средстве.
        /// </summary>
        int FreeSeats { get; }

        /// <summary>
        /// Получает точку отправления транспортного средства.
        /// </summary>
        string DeparturePoint { get; }

        /// <summary>
        /// Получает пункт назначения транспортного средства.
        /// </summary>
        string Destination { get; }

        /// <summary>
        /// Получает тип транспортного средства.
        /// </summary>
        string TransportType { get; }

        /// <summary>
        /// Отображает информацию о транспортном средстве.
        /// </summary>
        string DisplayInfo();

        /// <summary>
        /// Получает значение по указанному индексу. Индексы могут использоваться для дополнительных свойств транспортного средства.
        /// </summary>
        /// <param name="index">Индекс для доступа к свойству.</param>
        /// <returns>Значение свойства по указанному индексу.</returns>
        double this[string index] { get; set; }

    }
}
