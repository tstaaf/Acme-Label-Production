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
        SettingsUC settingsUC = new SettingsUC();
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
            maskinStatusUC.tillPLCLabel.Content = FormatPLC(order.ArtikelNamn, order.LotNr);
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
            maskinStatusUC.tillPLCLabelM2.Content = FormatPLC(order.ArtikelNamn, order.LotNr);
        }
        private string FormatPLC(string artNamn, string LOT)
        {
            var split = artNamn.Split((char)32);
            var split2 = split[1].Split((char)120);
            string splitFinal = split2[0].PadLeft(3, '0');
            string LOTFinal = LOT.PadLeft(2, '0');
            Console.WriteLine(LOTFinal + splitFinal);

            return LOTFinal + splitFinal;

        }
        private void Avsluta_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Vill du avsluta programmet?", "Avsluta", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }   
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
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainView.Content = settingsUC;
            maskinStatusView = false;
        }
        private void Ladda_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.ShowYesNoCancel("Välj maskin att skicka order till:", "Skicka Order", "Maskin 1", "Maskin 2", "Avbryt", MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SendToMSerie(maskinStatusUC.artnrLabel.Content.ToString(), 
                        maskinStatusUC.artnamnLabel.Content.ToString(),
                        maskinStatusUC.antalLabel.Content.ToString(),
                        maskinStatusUC.LOTText.Text.ToString(),
                        maskinStatusUC.orderLabel.Content.ToString());
                    SendToQD1(maskinStatusUC.artnrLabel.Content.ToString(),
                        maskinStatusUC.artnamnLabel.Content.ToString(),
                        maskinStatusUC.antalLabel.Content.ToString(),
                        maskinStatusUC.orderLabel.Content.ToString(),
                        maskinStatusUC.LOTText.Text.ToString());
                    SendToPLC1(maskinStatusUC.tillPLCLabel.Content.ToString());
                    break;
                case MessageBoxResult.No:
                    SendToMSerie2(maskinStatusUC.artnrLabelM2.Content.ToString(),
                        maskinStatusUC.artnamnLabelM2.Content.ToString(),
                        maskinStatusUC.antalLabelM2.Content.ToString(),
                        maskinStatusUC.LOTTextM2.Text.ToString(),
                        maskinStatusUC.orderLabelM2.Content.ToString());
                    SendToQD2(maskinStatusUC.artnrLabelM2.Content.ToString(),
                        maskinStatusUC.artnamnLabelM2.Content.ToString(),
                        maskinStatusUC.antalLabelM2.Content.ToString(),
                        maskinStatusUC.LOTTextM2.Text.ToString(),
                        maskinStatusUC.orderLabelM2.Content.ToString());
                    SendToPLC2(maskinStatusUC.tillPLCLabelM2.Content.ToString());
                    break;
                default:
                    break;
            }
        }
        private void AvslutaOrder_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessageBox.ShowYesNoCancel("Välj maskin att avsluta order på:", "Avsluta Order", "Maskin 1", "Maskin 2", "Avbryt", MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    AvslutaOrder1(maskinStatusUC.artnrLabel.Content.ToString(), maskinStatusUC.LOTText.Text.ToString(), maskinStatusUC.orderLabel.Content.ToString());
                    break;
                case MessageBoxResult.No:
                    AvslutaOrder2(maskinStatusUC.artnrLabelM2.Content.ToString(), maskinStatusUC.LOTTextM2.Text.ToString(), maskinStatusUC.orderLabelM2.Content.ToString());
                    break;
                default:
                    break;
            }
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
                MessageBox.Show(err.Message, "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
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
        static void SendToQD1(string artNr, string artNamn, string antal, string orderNr, string lot)
        {
            try
            {
                string path = Properties.Settings.Default.QDFilePath;
                if (File.Exists(path))
                {
                    File.Delete(path);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(artNamn);
                        sw.WriteLine(antal);
                        sw.WriteLine(orderNr);
                        sw.WriteLine(lot);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(artNamn);
                        sw.WriteLine(antal);
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
        static void SendToQD2(string artNr, string artNamn, string antal, string orderNr, string lot)
        {
            try
            {
                string path = Properties.Settings.Default.QD2FilePath;
                if (File.Exists(path))
                {
                    File.Delete(path);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(artNamn);
                        sw.WriteLine(antal);
                        sw.WriteLine(orderNr);
                        sw.WriteLine(lot);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(artNr);
                        sw.WriteLine(artNamn);
                        sw.WriteLine(antal);
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
        static void SendToPLC1(string toPLC)
        {
            try
            {
                string PLC1IP = Properties.Settings.Default.PLC1IP;
                int PLC1Port = Properties.Settings.Default.PLC1Port;
                TcpClient client = new TcpClient(PLC1IP, PLC1Port);
                string message = toPLC;

                byte[] data = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
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
                MessageBox.Show(err.Message, "Fel",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        static void SendToPLC2(string toPLC)
        {
            try
            {
                string PLC2IP = Properties.Settings.Default.PLC2IP;
                int PLC2Port = Properties.Settings.Default.PLC2Port;
                TcpClient client = new TcpClient(PLC2IP, PLC2Port);
                string message = toPLC;

                byte[] data = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
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
                MessageBox.Show(err.Message, "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void AvslutaOrder1(string artNr, string lot, string orderNr)
        {
            using (StreamWriter w = File.AppendText(Properties.Settings.Default.Maskin1LogPath + "LoggMaskin1.txt"))
            {
                w.Write("\r\nAvslutad Order : ");
                w.WriteLine($"{DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()}");
                w.WriteLine($"{orderNr} | {artNr} | {lot}");
                w.WriteLine("---------------------------------");
            };

            maskinStatusUC.orderLabel.Content = "-";
            maskinStatusUC.kundLabel.Content = "-";
            maskinStatusUC.datumLabel.Content = "-";
            maskinStatusUC.artnrLabel.Content = "-";
            maskinStatusUC.artnamnLabel.Content = "-";
            maskinStatusUC.antalLabel.Content = "-";
            maskinStatusUC.cylinderLabel.Content = "-";
            maskinStatusUC.stansLabel.Content = "-";
            maskinStatusUC.diameterLabel.Content = "-";
            maskinStatusUC.tillPLCLabel.Content = "-";
            maskinStatusUC.rawmatText.Text = "-";
            maskinStatusUC.LOTText.Text = "-";
        }
        public void AvslutaOrder2(string artNr, string lot, string orderNr)
        {
            using (StreamWriter w = File.AppendText(Properties.Settings.Default.Maskin2LogPath + "LoggMaskin2.txt"))
            {
                w.Write("\r\nAvslutad Order : ");
                w.WriteLine($"{DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()}");
                w.WriteLine($"{orderNr} | {artNr} | {lot}");
                w.WriteLine("---------------------------------");
            }

            maskinStatusUC.orderLabelM2.Content = "-";
            maskinStatusUC.kundLabelM2.Content = "-";
            maskinStatusUC.datumLabelM2.Content = "-";
            maskinStatusUC.artnrLabelM2.Content = "-";
            maskinStatusUC.artnamnLabelM2.Content = "-";
            maskinStatusUC.antalLabelM2.Content = "-";
            maskinStatusUC.cylinderLabelM2.Content = "-";
            maskinStatusUC.stansLabelM2.Content = "-";
            maskinStatusUC.diameterLabelM2.Content = "-";
            maskinStatusUC.tillPLCLabelM2.Content = "-";
            maskinStatusUC.rawmatTextM2.Text = "-";
            maskinStatusUC.LOTTextM2.Text = "-";
        }
        
    }
}