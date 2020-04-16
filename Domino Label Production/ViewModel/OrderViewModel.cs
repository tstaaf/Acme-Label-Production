using Domino_Label_Production.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino_Label_Production.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Orders ord = new Orders();

        public int ordId {
            get { return ord.Id; } 
            set { ord.Id = value; }
        }
        public string ordTillverkningsOrderNummer {
            get { return ord.TillverkningsOrderNummer; }
            set { ord.TillverkningsOrderNummer = value; }
        }
        public string ordKundNummer {
            get { return ord.KundNummer; }
            set { ord.KundNummer = value; }
        }
        public string ordLeveransdatum {
            get { return ord.Leveransdatum; }
            set { ord.Leveransdatum = value; }
        }
        public string ordAntalRulle {
            get { return ord.AntalRulle; }
            set { ord.AntalRulle = value; }
        }
        public string ordCylinder {
            get { return ord.Cylinder; }
            set { ord.Cylinder = value; }
        }
        public string ordStans {
            get { return ord.Stans; }
            set { ord.Stans = value; }
        }
        public string ordDiameter {
            get { return ord.Diameter; }
            set { ord.Diameter = value; }
        }
        public string ordArtikelNummer {
            get { return ord.ArtikelNummer; }
            set { ord.ArtikelNummer = value; }
        }
        public string ordArtikelNamn {
            get { return ord.ArtikelNamn; }
            set { ord.ArtikelNamn = value; }
        }
        public string ordRåMaterialNummer {
            get { return ord.RåMaterialNummer; }
            set { ord.RåMaterialNummer = value; }
        }
        public string ordLotNr {
            get { return ord.LotNr; }
            set { ord.LotNr = value; }
        }

        
    }
}
