using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class Result
    {
       public string id { get; set; }
       public List<Order_item> order_items { get; set; }
       public Buyer buyer { get; set; }
       public Result() {

           order_items = new List<Order_item>();
       }
    }
}
