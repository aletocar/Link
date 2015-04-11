using ERPIntegrationInterface;
using Link.IntegrationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.IntegrationFactory
{
    public class IntegrationFactory
    {
        public static IEcommerceIntegration GetEcommerceIntegration(string ecommerce)
        {
            //Reflection
            return null;
        }

        public static IERPIntegration GetERPIntegration(string erp)
        {
            //Reflection
            return null;
        }
    }
}
