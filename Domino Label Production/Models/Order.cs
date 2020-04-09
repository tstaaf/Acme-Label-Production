using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domino_Label_Production.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string TillverkningsOrderNummer { get; set; }
        public string KundNummer { get; set; }
        public string Leveransdatum { get; set; }
        public string AntalRulle { get; set; }
        public string Cylinder { get; set; }
        public string Stans { get; set; }
        public string Diameter { get; set; }
        public string ArtikelNummer { get; set; }
        public string ArtikelNamn { get; set; }
        public string RåMaterialNummer { get; set; }
        public string LotNr { get; set; }
    }

    //public class OrderContext : DbContext
    //{
    //    public DbSet<Order> Orders { get; set; }
    //}
}
