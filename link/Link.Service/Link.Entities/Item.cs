using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public Integration Integration { get; set; }
        public string title
        {
            get;
            set;
        }

        public int price
        {
            get;
            set;

        }

        public int available_quantity
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string warranty
        {
            get;
            set;
        }

        public string Nombre
        {
            get;
            set;
        }

        public double Costo
        {
            get;
            set;
        }

        public string Codigo
        {
            get;
            set;
        }

        public string Cantidad { get; set; }
        public Item() { }
    }
}
