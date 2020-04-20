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
using Domino_Label_Production.ViewModel;

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
            MainView.Content = maskinval;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}