using Domino_Label_Production.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using Domino_Label_Production.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace Domino_Label_Production.Windows
{
    /// <summary>
    /// Interaction logic for TillverkningsOrderListaWindow.xaml
    /// </summary>
    public partial class TillverkningsOrderListaWindow : Window
    {
        public TillverkningsOrderListaWindow()
        {
            InitializeComponent();

            //using (var reader = new StreamReader(@"C:\Domino\Listener\OrderData.csv"))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    var records = csv.GetRecords<Order>();
            //    OrderLista.ItemsSource = records;
            //}


            string[] lines = File.ReadAllLines(@"C:\Domino\Listener\21.txt");
            List<string> sortedLines = new List<string>();
            foreach (var line in lines.Skip(1))
            {
                if (line == " 11")
                {
                    continue;
                }
                var vals = line.Split((char)59)[1];
                sortedLines.Add(vals);
            }

            Order order = new Order()
            {
                TillverkningsOrderNummer = sortedLines[0],
                KundNummer = sortedLines[14],
                Leveransdatum = sortedLines[1],
                ArtikelNummer = sortedLines[4],
                ArtikelNamn = sortedLines[5],
                RåMaterialNummer = "",
                LotNr = ""
            };

        }

        private void PopulateOrder()
        {

        }

        private void Stang_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
