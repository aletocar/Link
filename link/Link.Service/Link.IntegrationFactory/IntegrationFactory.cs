using Link.ERPIntegrationInterface;
using Link.EcommerceIntegrationInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MercadoLibreController.LINK.Controllers;
using System.Reflection;
using System.Configuration;
using IntegrationZetaPrueba2;

namespace Link.IntegrationFactory
{
    public class IntegrationFactory
    {
        public static IEcommerceIntegration GetEcommerceIntegration(string ecommerce)
        {
            //Reflection
            //Assembly integrationAssembly = Assembly.LoadFile(ConfigurationManager.AppSettings[ecommerce.ToUpper() + "Integration".ToUpper()]);
            //Type integrationAssemblyType = integrationAssembly.GetType(ConfigurationManager.AppSettings[ecommerce.ToUpper() + "IntegrationType".ToUpper()]);
            ////MethodInfo getController = integrationAssemblyType.GetMethod("GetController");
            //return Activator.CreateInstance(integrationAssemblyType) as IEcommerceIntegration;
            return new MercadoLibreController.LINK.Controllers.MercadoLibreController();
        }

        public static IERPIntegration GetERPIntegration(string erp)
        {
            //string fileDir = ConfigurationManager.AppSettings[erp.ToUpper() + "Integration".ToUpper()];
            //string assemblyType = ConfigurationManager.AppSettings[erp.ToUpper() + "IntegrationType".ToUpper()];
            //Assembly integrationAssembly = Assembly.LoadFile(fileDir);
            //Type integrationAssemblyType = integrationAssembly.GetType(assemblyType);
            //return Activator.CreateInstance(integrationAssemblyType) as IERPIntegration;
            return new ZetaIntegrationImp();
        }
    }
}
