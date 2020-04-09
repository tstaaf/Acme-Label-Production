using System;
using System.Collections.Generic;
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

namespace Domino_Label_Production.UserControls
{
    /// <summary>
    /// Interaction logic for MaskinValUC.xaml
    /// </summary>
    public partial class MaskinValUC : UserControl
    {
        OrderListaUC orderLista = new OrderListaUC();
        public MaskinValUC()
        {
            InitializeComponent();
        }

        private void Maskin1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = orderLista;
        }

        private void Maskin2_Click(object sender, RoutedEventArgs e)
        {
            this.Content = orderLista;
        }
    }
}
