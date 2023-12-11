using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ClassLibrary8;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private ITransport[] transports;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public MainWindow()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
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
                string minPriceFlightInfo = minPriceFlight.DisplayInfo();
                transportInfoTextBox.Text = minPriceFlightInfo;
            }
            else
            {
                MessageBox.Show($"Для данного типа транспорта не найдено минимальной цены на рейс. {transportType} и класс билета {ticketClass}");
            }
        }

        private ITransport[] ReadTransportsFromFile(string filePath)
        {
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            ITransport[] transportArray = null;
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

            try
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
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
                                new double[] { double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6]) });
                            break;
                        case "поезд":
                            transport = new Train(
                                int.Parse(parts[1]),
                                parts.Skip(9).Select(int.Parse).ToArray(),
                                parts[2],
                                parts[3],
                                new double[] { double.Parse(parts[4]), double.Parse(parts[5]), double.Parse(parts[6]), double.Parse(parts[7]) });
                            break;
                        case "автобус":
                            transport = new Bus(
                                 int.Parse(parts[1]),
                                 int.Parse(parts[7]),
                                 parts[2],
                                 parts[3],
                                 new double[] { double.Parse(parts[4]), double.Parse(parts[5]) });
                            break;
                        default:
                            throw new ArgumentException($"Неверный вид транспорта: {parts[0]}");
                    }

                    transportArray[i] = transport;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            return transportArray;
        }

        private void DisplayTransportInfo(ITransport[] transports)
        {
            StringBuilder fullInfoBuilder = new StringBuilder();

            foreach (var transport in transports)
            {
                string transportInfo = transport.DisplayInfo();

                if (transport.FreeSeats == 0)
                {
                    fullInfoBuilder.AppendLine(transportInfo);
                    transportInfoTextBox.Foreground = Brushes.Red;
                }
                else
                {
                    fullInfoBuilder.AppendLine(transportInfo);
                    transportInfoTextBox.Foreground = Brushes.Black;
                }
            }

            transportInfoTextBox.Text = fullInfoBuilder.ToString();
        }

        public ITransport FindMinPriceFlight(ITransport[] transports, string transportType, string ticketClass)
        {
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            ITransport minPriceFlight = null;
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
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
    }
}
