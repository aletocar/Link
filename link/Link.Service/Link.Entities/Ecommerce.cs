using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class Ecommerce
    {
        public int EcommerceId { get; set; }
        public string EcommerceName { get; set; }
        public string ECommerceUrl { get; set; }
        public long EcommerceAppId { get; set; }
        public string EcommerceSecret { get; set; }
    }
}
