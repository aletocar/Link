using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPIntegrationInterface
{
    public interface IERPIntegration
    {
        JObject GetArticles(string ip, string username, string password);
    }
}
