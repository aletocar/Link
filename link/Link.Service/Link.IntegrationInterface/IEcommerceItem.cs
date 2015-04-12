using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.EcommerceIntegrationInterface
{
    public interface IEcommerceItem
    {
        
       string title { get; set; }
       int price { get; set; }
       int available_quantity { get; set; }
       string description { get; set; }
       string warranty { get; set; }
    }
}
