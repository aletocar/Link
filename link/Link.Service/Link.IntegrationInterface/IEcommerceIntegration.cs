using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.EcommerceIntegrationInterface
{
    public interface IEcommerceIntegration
    {
        string Connect();
        string Authorize(string code);
        string Publish(List<IEcommerceItem> json);
        void SetCredentials(long id, string secret, string token);
        
    }
}
