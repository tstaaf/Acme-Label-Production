using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Domino_Label_Production.Models;
using Domino_Label_Production.Service;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Domino_Label_Production.UserControls;
using Domino_Label_Production.Windows;
using System.Net.Sockets;
using System.IO;
using WPFCustomMessageBox;

namespace Domino_Label_Production
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrderService service = new OrderService();
        MaskinStatus maskinStatusUC = new MaskinStatus();
        OrderListaUC orderListaUC = new OrderListaUC();
        MaskinValUC maskinval = new MaskinValUC();
        bool maskinStatusView;
        public MainWindow()
        {
            InitializeComponent();
            MainView.Content = maskinStatusUC;
            maskinStatusView = true;
            service.Watch();
        }

        private void TillverkningsOrderLista_Click(object sender, RoutedEventArgs e)
        {
            //var orderListaWindow = new TillverkningsOrderListaWindow();
            //orderListaWindow.Show();
            MainView.Content = orderListaUC;
            maskinStatusView = false;

        }

        public void SelectedOrderMaskin1(Orders order)
        {
            maskinStatusUC.orderLabel.Content = order.TillverkningsOrderNummer;
            maskinStatusUC.kundLabel.Content = order.KundNummer;
            maskinStatusUC.datumLabel.Content = order.Leveransdatum;
            maskinStatusUC.artnrLabel.Content = order.ArtikelNummer;
            maskinStatusUC.artnamnLabel.Content = order.ArtikelNamn;
            maskinStatusUC.antalLabel.Content = order.AntalRulle;
            maskinStatusUC.cylinderLabel.Content = order.Cylinder;
            maskinStatusUC.stansLabel.Content = order.Stans;
            maskinStatusUC.diameterLabel.Content = order.Diameter;
            maskinStatusUC.rawmatLabel.Content = order.RåMaterialNummer;
            maskinStatusUC.LOTLabel.Content = order.LotNr;
        }
        public void SelectedOrderMaskin2(Orders order)
        {
            maskinStatusUC.orderLabelM2.Content = order.TillverkningsOrderNummer;
            maskinStatusUC.kundLabelM2.Content = order.KundNummer;
            maskinStatusUC.datumLabelM2.Content = order.Leveransdatum;
            maskinStatusUC.artnrLabelM2.Content = order.ArtikelNummer;
            maskinStatusUC.artnamnLabelM2.Content = order.ArtikelNamn;
            maskinStatusUC.antalLabelM2.Content = order.AntalRulle;
            maskinStatusUC.cylinderLabelM2.Content = order.Cylinder;
            maskinStatusUC.stansLabelM2.Content = order.Stans;
            maskinStatusUC.diameterLabelM2.Content = order.Diameter;
            maskinStatusUC.rawmatLabelM2.Content = order.RåMaterialNummer;
            maskinStatusUC.LOTLabelM2.Content = order.LotNr;
        }

        private void Avsluta_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimera_Click(object sender, RoutedEventArgs e)
        {
            
            if(!maskinStatusView)
            {
                MainView.Content = maskinStatusUC;
                maskinStatusView = true;
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void Ladda_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.ShowYesNoCancel("Vilken maskin vill du skicka till?", "Skicka Order", "Maskin 1", "Maskin 2", "Avbryt", MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SendToMSerie(maskinStatusUC.artnrLabel.Content.ToString(), 
                        maskinStatusUC.artnamnLabel.Content.ToString(),
                        maskinStatusUC.antalLabel.Content.ToString(),
                        maskinStatusUC.LOTLabel.Content.ToString(),
                        maskinStatusUC.orderLabel.Content.ToString());
                    SendToAX1(maskinStatusUC.artnamnLabel.Content.ToString(),
                        maskinStatusUC.orderLabel.Content.ToString(),
                        maskinStatusUC.LOTLabel.Content.ToString());
                    break;
                case MessageBoxResult.No:
                    SendToMSerie2(maskinStatusUC.artnrLabelM2.Content.ToString(),
                        maskinStatusUC.artnamnLabelM2.Content.ToString(),
                        maskinStatusUC.antalLabelM2.Content.ToString(),
                        maskinStatusUC.LOTLabelM2.Content.ToString(),
                        maskinStatusUC.orderLabelM2.Content.ToString());
                    break;
                default:
                    break;
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            
        }
        static void SendToMSerie(string artNr, string artNamn, string antal, string lot, string orderNr)
        {
            try
            {
                string MserieIP = Properties.Settings.Default.MSerieIP;
                int MSeriePort = Properties.Settings.Default.MSeriePort;
                TcpClient client = new TcpClient(MserieIP, MSeriePort);
                string sendString1 = (char)2 + "019E1??" + (char)13;
                string sendString2 = (char)2 + "02C??" + (char)13;
                string message = (char)2 + "00D" + artNr + (char)10 + artNamn + (char)10 + antal + (char)10 + lot + (char)10 + orderNr + "??" + (char)13;

                byte[] data = Encoding.ASCII.GetBytes(sendString1);
                byte[] data2 = Encoding.ASCII.GetBytes(sendString2);
                byte[] dataMessage = Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Write(data2, 0, data2.Length);
                stream.Write(dataMessage, 0, dataMessage.Length);
                Console.WriteLine("Sent: {0}", sendString1);
                Console.WriteLine("Sent: {0}", sendString2);
                Console.WriteLine("Sent: {0}", message);

                data = new byte[256];
                string response = string.Empty;

                int bytes = stream.Read(data, 0, data.Length);
                response = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recieved: {0}", response);

                stream.Close();
                client.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }
        static void SendToMSerie2(string artNr, string artNamn, string antal, string lot, string orderNr)
        {
            try
            {
                string MserieIP = Properties.Settings.Default.MSerieIP;
                int MSeriePort = Properties.Settings.Default.MSeriePort;
                TcpClient client = new TcpClient(MserieIP, MSeriePort);
                string sendString1 = (char)2 + "019E2??" + (char)13;
                string sendString2 = (char)2 + "02C??" + (char)13;
                string message = (char)2 + "00D" + artNr + (char)10 + artNamn + (char)10 + antal + (char)10 + lot + (char)10 + orderNr + "??" + (char)13;

                byte[] data = Encoding.ASCII.GetBytes(sendString1);
                byte[] data2 = Encoding.ASCII.GetBytes(sendString2);
                byte[] dataMessage = Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Write(data2, 0, data2.Length);
                stream.Write(dataMessage, 0, dataMessage.Length);
                Console.WriteLine("Sent: {0}", sendString1);
                Console.WriteLine("Sent: {0}", sendString2);
                Console.WriteLine("Sent: {0}", message);

                data = new byte[256];
                string response = string.Empty;

                int bytes = stream.Read(data, 0, data.Length);
                response = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recieved: {0}", response);

                stream.Close();
                client.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }
        static void SendToAX1(string artNr, string orderNr, string lot)
        {
            try
            {
                string path = Properties.Settings.Default.AxFilePath;
                if (File.Exists(path))
                {
                    File.Delete(path);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(orderNr);
                        sw.WriteLine(lot);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(orderNr);
                        sw.WriteLine(lot);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }
    }
}