namespace ClassLibrary8
{
    /// <summary>
    /// Представляет вид транспорта.
    /// </summary>
    public enum TransportKind
    {
        /// <summary>
        /// Автобус как вид транспорта.
        /// </summary>
        Автобус,

        /// <summary>
        /// Поезд как вид транспорта.
        /// </summary>
        Поезд,

        /// <summary>
        /// Самолёт как вид транспорта.
        /// </summary>
        Самолёт
    }

    /// <summary>
    /// Интерфейс, представляющий транспортное средство.
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// Получает вид транспорта.
        /// </summary>
        TransportKind Kind { get; }

        /// <summary>
        /// Получает или устанавливает количество свободных мест в транспортном средстве.
        /// </summary>
        int FreeSeats { get; set; }

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
        /// Получает или устанавливает значение по указанному индексу. Индексы могут использоваться для дополнительных свойств транспортного средства.
        /// </summary>
        /// <param name="index">Индекс для доступа к свойству.</param>
        /// <returns>Значение свойства по указанному индексу.</returns>
        double this[string index] { get; set; }
    }
}
