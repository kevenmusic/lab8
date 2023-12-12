using ClassLibrary8;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private ITransport[]? transports;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadTransportInfo_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "transport_info.txt";
            transports = ReadTransportsFromFile(filePath);
            DisplayTransportInfo(transports);
        }

        private void FindMinPriceFlight_Click(object sender, RoutedEventArgs e)
        {
            if (transports == null)
            {
                MessageBox.Show("Пожалуйста, сначала прочитайте информацию о транспорте.");
                return;
            }

            string transportType = transportTypeTextBox.Text;
            string ticketClass = ticketClassTextBox.Text;

            ITransport minPriceFlight = FindMinPriceFlight(transports, transportType, ticketClass);

            if (minPriceFlight != null)
            {
                DisplayTransportInfo([minPriceFlight]);
            }
            else
            {
                MessageBox.Show($"Для данного типа транспорта не найдено минимальной цены на рейс. {transportType} и класс билета {ticketClass}");
            }
        }

        private ITransport[] ReadTransportsFromFile(string filePath)
        {
            ITransport[]? transportArray = null;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                transportArray = new ITransport[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(' ');

                    ITransport transport;
                    switch (parts[0].ToLower())
                    {
                        case "самолет":
                            transport = new Airplane(
                                int.Parse(parts[1]),
                                int.Parse(parts[8]),
                                parts[2],
                                parts[3],
                                [double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6])]);
                            break;
                        case "поезд":
                            transport = new Train(
                                int.Parse(parts[1]),
                                parts.Skip(9).Select(int.Parse).ToArray(),
                                parts[2],
                                parts[3],
                                [double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6]), double.Parse(parts[7])]);
                            break;
                        case "автобус":
                            transport = new Bus(
                                 int.Parse(parts[1]),
                                 int.Parse(parts[7]),
                                 parts[2],
                                 parts[3],
                                 [double.Parse(parts[4]), double.Parse(parts[5])]);
                            break;
                        default:
                            throw new TransportException($"Неверный вид транспорта: {parts[0]}");
                    }

                    transportArray[i] = transport;
                }
            }
            catch (TransportException ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            return transportArray;
        }

        private void DisplayTransportInfo(ITransport[] transports)
        {
            // Создаем новый документ для RichTextBox
            FlowDocument flowDocument = new FlowDocument();

            // Перебираем все транспортные объекты
            foreach (var transport in transports)
            {
                string transportInfo = transport.DisplayInfo();

                // Создаем новый параграф с текстом транспорта
                Paragraph paragraph = new Paragraph(new Run(transportInfo));

                if (transport.FreeSeats == 0)
                {
                    paragraph.Foreground = Brushes.Red;
                }

                // Добавляем параграф в документ
                flowDocument.Blocks.Add(paragraph);
            }

            // Применяем созданный документ к RichTextBox
            transportInfoTextBox.Document = flowDocument;
        }



        public ITransport FindMinPriceFlight(ITransport[] transports, string transportType, string ticketClass)
        {
            ITransport? minPriceFlight = null;
            double minPrice = double.MaxValue;
            foreach (var transport in transports)
            {
                try
                {
                    if (string.Equals(transport.TransportType, transportType, StringComparison.OrdinalIgnoreCase))
                    {
                        double ticketPrice = transport[ticketClass.ToLowerInvariant()];
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

            string transportType = transportTypeTextBox.Text.ToLowerInvariant();
            string ticketClass = ticketClassTextBox.Text.ToLowerInvariant();

            int indexToDelete = -1;
            for (int i = 0; i < transports.Length; i++)
            {
                try
                {
                    if (transports[i].TransportType.ToLowerInvariant() == transportType && transports[i][ticketClass.ToLowerInvariant()] != 0)
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
                MessageBox.Show($"Для данного типа транспорта и класса билета не найдено записи для удаления. {transportType} и класс билета {ticketClass}");
            }
        }

        private static ITransport[] RemoveTransportAtIndex(ITransport[] array, int index)
        {
            ITransport[] newArray = new ITransport[array.Length - 1];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (i != index)
                {
                    newArray[j++] = array[i];
                }
            }
            return newArray;
        }

        private void AddTransport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(transportTypeTextBox2.Text) ||
                string.IsNullOrWhiteSpace(flightNumberTextBox2.Text) ||
                string.IsNullOrWhiteSpace(pointOfDepartureTextBox.Text) ||
                string.IsNullOrWhiteSpace(pointOfDestinationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ticketPricesTextBox.Text) ||
                string.IsNullOrWhiteSpace(freeSeatsTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все необходимые поля.");
                return;
            }

            string transportType = transportTypeTextBox2.Text;
            int flightNumber = int.Parse(flightNumberTextBox2.Text);
            string pointOfDeparture = pointOfDepartureTextBox.Text;
            string pointOfDestination = pointOfDestinationTextBox.Text;
            int freeSeats = int.Parse(freeSeatsTextBox.Text);

            // Разбиваем входную строку на массив значений типа double
            string[] ticketPricesStringArray = ticketPricesTextBox.Text.Split(',');

            double[] ticketPrices = new double[ticketPricesStringArray.Length];

            for (int i = 0; i < ticketPricesStringArray.Length; i++)
            {
                if (double.TryParse(ticketPricesStringArray[i], out double price))
                {
                    ticketPrices[i] = price;
                }
                else
                {
                    MessageBox.Show($"Неверный формат цены билета: {ticketPricesStringArray[i]}");
                    return;
                }
            }

            ITransport newTransport;
            switch (transportType.ToLowerInvariant())
            {
                case "самолет":
                    newTransport = new Airplane(flightNumber, freeSeats, pointOfDeparture, pointOfDestination, ticketPrices);
                    break;
                case "поезд":
                    newTransport = new Train(flightNumber, [freeSeats], pointOfDeparture, pointOfDestination, ticketPrices);
                    break;
                case "автобус":
                    newTransport = new Bus(flightNumber, freeSeats, pointOfDeparture, pointOfDestination, ticketPrices);
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
            transportTypeTextBox2.Clear();
            flightNumberTextBox2.Clear();
            pointOfDepartureTextBox.Clear();
            pointOfDestinationTextBox.Clear();
            ticketPricesTextBox.Clear();
            freeSeatsTextBox.Clear();
        }
    }
}