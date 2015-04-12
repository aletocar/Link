using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link.ERPIntegrationInterface
{
    public interface IERPIntegration
    {
        List<EcommerceItem> GetArticles(string ip, string username, string password);
        double GetStock(string ip, string username, string password);
    }
}
