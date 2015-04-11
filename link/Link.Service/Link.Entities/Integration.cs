using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class Integration
    {
        public int IntegrationId { get; set; }
        public Client ClientIntegrated { get; set; }
        public CustomerErp ErpIntegrated { get; set; }
        public Ecommerce EcommerceIntegrated { get; set; }
        public string IntegrationIp { get; set; }
        public string EcommerceUserName { get; set; }
        public string EcommerceAccessToken { get; set; }
        public string EcommerceCode { get; set; }

        public Integration()
        {

        }
    }
}
