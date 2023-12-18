using ClassLibrary8;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private ITransport[] transports;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DisplayTransportInfo(ITransport[] transports)
        {
            // Создаем новый документ для RichTextBox
            FlowDocument flowDocument = new FlowDocument();

            foreach (var transport in transports)
            {
                string transportInfo = transport.DisplayInfo();

                // Разделяем строку на отдельные строки
                string[] lines = transportInfo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Добавляем пустой параграф для разделения блоков
                flowDocument.Blocks.Add(new Paragraph(new Run(new string('-', 55))));
                foreach (var line in lines)
                {
                    // Создаем новый параграф с текстом транспорта
                    Paragraph paragraph = new Paragraph(new Run(line));
                    paragraph.LineHeight = 5;

                    // Если свободных мест 0, то цвет текста становится красным
                    if (line.Contains("Количество свободных мест:") && transport.FreeSeats == 0)
                    {
                        paragraph.Foreground = Brushes.Red;
                    }
                    // Добавляем параграф в документ
                    flowDocument.Blocks.Add(paragraph);
                }
            }
            flowDocument.Blocks.Add(new Paragraph(new Run(new string('-', 55))));

            // Применяем созданный документ к RichTextBox
            transportInfoTextBox.Document = flowDocument;
        }

        private void ReadTransportInfo_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "transport_info.txt";
            transports = ReadTransportsFromFile(filePath);
            DisplayTransportInfo(transports);
        }

        private ITransport[] ReadTransportsFromFile(string filePath)
        {
            ITransport[] transportArray = null;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                transportArray = new ITransport[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(' ');
                    string transportKind = parts[0].ToLower();
                    int flightNumber = int.Parse(parts[1]);
                    string departurePoint = parts[2];
                    string destination = parts[3];

                    double[] ticketPrices;
                    switch (transportKind)
                    {
                        case "самолет":
                            ticketPrices = [double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6])];
                            int freeSeats = int.Parse(parts[8]);
                            ITransport airplane = new Airplane(flightNumber, freeSeats, departurePoint, destination, ticketPrices);
                            transportArray[i] = airplane;
                            break;
                        case "поезд":
                            ticketPrices = [double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6]), double.Parse(parts[7])];
                            int[] wagonSeats = parts.Skip(9).Select(int.Parse).ToArray();
                            ITransport train = new Train(flightNumber, wagonSeats, departurePoint, destination, ticketPrices);
                            transportArray[i] = train;
                            break;
                        case "автобус":
                            ticketPrices = [double.Parse(parts[4]), double.Parse(parts[5])];
                            int busSeats = int.Parse(parts[7]);
                            ITransport bus = new Bus(flightNumber, busSeats, departurePoint, destination, ticketPrices);
                            transportArray[i] = bus;
                            break;
                        default:
                            throw new TransportException($"Неверный вид транспорта", transportKind);
                    }
                }
            }
            catch (TransportException ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}. Дополнительная информация: {ex.Value}.");
            }

            return transportArray;
        }

        private void FindMinPriceFlight_Click(object sender, RoutedEventArgs e)
        {
            if (transports == null)
            {
                MessageBox.Show("Пожалуйста, сначала прочитайте информацию о транспорте.");
                return;
            }

            string transportType = transportTypeComboBox.Text.ToLower();
            string transportKind = transportTypeComboBox.Text.ToLower();
            string ticketClass = ticketClassTextBox.Text.ToLower();

            ITransport minPriceFlight = FindMinPriceFlight(transports, transportType, transportKind, ticketClass);

            if (minPriceFlight != null)
            {
                DisplayTransportInfo([minPriceFlight]);
            }
            else
            {
                MessageBox.Show($"Для данного типа или вида транспорта не найдено минимальной цены на рейс. {transportType} и класс билета {ticketClass}");
            }
        }

        protected ITransport FindMinPriceFlight(ITransport[] transports, string transportType, string transportKind, string ticketClass)
        {
            ITransport minPriceFlight = null;
            double minPrice = double.MaxValue;
            foreach (var transport in transports)
            {
                try
                {
                    double ticketPrice = 0;

                    if (transport.TransportType.ToLower() == transportType || transport.Kind.ToString().ToLower() == transportKind)
                    {
                        switch (transport.Kind)
                        {
                            case TransportKind.Самолёт:
                                if (transport is Airplane airplane)
                                {
                                    ticketPrice = airplane[ticketClass.ToLower()];
                                }
                                break;
                            case TransportKind.Поезд:
                                if (transport is Train train)
                                {
                                    ticketPrice = train[ticketClass.ToLower()];
                                }
                                break;
                            case TransportKind.Автобус:
                                if (transport is Bus bus)
                                {
                                    ticketPrice = bus[ticketClass.ToLower()];
                                }
                                break;
                        }

                        if (ticketPrice < minPrice)
                        {
                            minPrice = ticketPrice;
                            minPriceFlight = transport;
                        }
                    }
                }
                catch
                {
                }
            }
            return minPriceFlight;
        }

        private void DeleteTransport_Click(object sender, RoutedEventArgs e)
        {
            if (transports == null)
            {
                MessageBox.Show("Пожалуйста, сначала прочитайте информацию о транспорте.");
                return;
            }

            string transportType = transportTypeComboBox.Text.ToLower();
            string transportKind = transportTypeComboBox.Text.ToLower();
            string ticketClass = ticketClassTextBox.Text.ToLower();

            int indexToDelete = -1;
            for (int i = 0; i < transports.Length; i++)
            {
                try
                {
                    double ticketPrice = 0;
                    switch (transports[i].Kind)
                    {
                        case TransportKind.Самолёт:
                            if (transports[i] is Airplane airplane)
                            {
                                ticketPrice = airplane[ticketClass.ToLower()];
                            }
                            break;
                        case TransportKind.Поезд:
                            if (transports[i] is Train train)
                            {
                                ticketPrice = train[ticketClass.ToLower()];
                            }
                            break;
                        case TransportKind.Автобус:
                            if (transports[i] is Bus bus)
                            {
                                ticketPrice = bus[ticketClass.ToLower()];
                            }
                            break;
                    }

                    if ((transports[i].TransportType.ToLower() == transportType && ticketPrice != 0) || (transports[i].Kind.ToString().ToLower() == transportKind && ticketPrice != 0))
                    {
                        indexToDelete = i;
                        break;
                    }
                }
                catch
                {
                }
            }
            if (indexToDelete != -1)
            {
                transports = RemoveTransportAtIndex(transports, indexToDelete);
                DisplayTransportInfo(transports);
            }
            else
            {
                MessageBox.Show($"Для данного типа или вида транспорта и класса билета не найдено записи для удаления. {transportType} и класс билета {ticketClass}");
            }
        }

        private static ITransport[] RemoveTransportAtIndex(ITransport[] array, int index)
        {
            // Создание нового массива длиной на один элемент меньше исходного массива
            ITransport[] newArray = new ITransport[array.Length - 1];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (i != index)
                {
                    // Копирование элементов из исходного массива в новый массив
                    newArray[j++] = array[i];
                }
            }
            return newArray;
        }

        private void AddTransport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(transportTypeComboBox2.Text) ||
                string.IsNullOrWhiteSpace(flightNumberTextBox2.Text) ||
                string.IsNullOrWhiteSpace(pointOfDepartureTextBox.Text) ||
                string.IsNullOrWhiteSpace(pointOfDestinationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ticketPricesTextBox.Text) ||
                string.IsNullOrWhiteSpace(freeSeatsTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все необходимые поля.");
                return;
            }

            string transportType = transportTypeComboBox2.Text;
            string flightNumberText = flightNumberTextBox2.Text;
            if (!int.TryParse(flightNumberText, out int flightNumber))
            {
                MessageBox.Show($"Неверный формат номера рейса: {flightNumberText}");
                return;
            }

            string pointOfDeparture = pointOfDepartureTextBox.Text;
            string pointOfDestination = pointOfDestinationTextBox.Text;
            string freeSeatsText = freeSeatsTextBox.Text;

            if (!int.TryParse(freeSeatsText, out int freeSeats))
            {
                MessageBox.Show($"Неверный формат количества свободных мест: {freeSeatsText}");
                return;
            }

            // Разбиваем входную строку на массив значений типа double
            string[] ticketPricesStringArray = ticketPricesTextBox.Text.Split(',');

            double[] ticketPrices = new double[ticketPricesStringArray.Length];

            for (int i = 0; i < ticketPricesStringArray.Length; i++)
            {
                if (double.TryParse(ticketPricesStringArray[i], out double price) && price >= 0)
                {
                    ticketPrices[i] = price;
                }
                else
                {
                    MessageBox.Show($"Неверный формат цены билета: {ticketPricesStringArray[i]}");
                    return;
                }
            }

            if (!IsValidTicketPricesLength(transportType.ToLower(), ticketPrices))
            {
                MessageBox.Show($"Неверное количество цен на билет для типа транспорта: {transportType}");
                return;
            }

            ITransport newTransport = null;
            switch (transportType.ToLower())
            {
                case "самолет":
                    try
                    {
                        newTransport = new Airplane(flightNumber, freeSeats, pointOfDeparture, pointOfDestination, ticketPrices);
                    }
                    catch (TransportAirplaneException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    catch (TransportException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    break;
                case "поезд":
                    try
                    {
                        newTransport = new Train(flightNumber, [freeSeats], pointOfDeparture, pointOfDestination, ticketPrices);
                    }
                    catch (TransportTrainException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    catch (TransportException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    break;
                case "автобус":
                    try
                    {
                        newTransport = new Bus(flightNumber, freeSeats, pointOfDeparture, pointOfDestination, ticketPrices);
                    }
                    catch (TransportBusException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    catch (TransportException ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}. Дополнительная информация: {ex.Value}.");
                    }
                    break;
                default:
                    MessageBox.Show($"Неверный тип транспорта: {transportType}");
                    return;
            }

            if (transports == null)
            {
                transports = new ITransport[0];
            }

            // Изменяем размер массива транспортов для размещения нового транспорта
            Array.Resize(ref transports, transports.Length + 1);

            // Добавляем новый транспорт в массив
            transports[transports.Length - 1] = newTransport;

            // Отображение обновленной информации о транспорте
            DisplayTransportInfo(transports);

            ClearInputFields();
        }

        private void ClearInputFields()
        {
            ticketClassTextBox.Clear();
            flightNumberTextBox2.Clear();
            pointOfDepartureTextBox.Clear();
            pointOfDestinationTextBox.Clear();
            ticketPricesTextBox.Clear();
            freeSeatsTextBox.Clear();
        }

        private bool IsValidTicketPricesLength(string transportType, double[] ticketPrices)
        {
            switch (transportType.ToLower())
            {
                case "самолет":
                    return ticketPrices.Length == 3;
                case "поезд":
                    return ticketPrices.Length == 4;
                case "автобус":
                    return ticketPrices.Length == 2;
                default:
                    return false;
            }
        }
    }
}