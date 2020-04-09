using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;
using Domino_Label_Production.Models;
using Domino_Label_Production.Service;
using System.Data.Entity;

namespace Domino_Label_Production.UserControls
{
    /// <summary>
    /// Interaction logic for OrderListaUC.xaml
    /// </summary>
    public partial class OrderListaUC : UserControl
    {
        Entities context = new Entities();
        CollectionViewSource ordersViewSource;
        public OrderListaUC()
        {
            InitializeComponent();
            ordersViewSource = ((CollectionViewSource)(FindResource("ordersViewSource")));
            DataContext = this;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var maskinStatus = new MaskinStatus();
            this.Content = maskinStatus;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }

            context.Orders.Load();
            
            ordersViewSource.Source = context.Orders.Local;
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            context.Orders.Load();

            Orders order = (Orders)ordersDataGrid.SelectedItem;
            Console.WriteLine(order.ArtikelNamn);

            ordersViewSource.Source = context.Orders.Local;
        }
    }
}
