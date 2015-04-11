using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Business
{
    public interface IClientController
    {

        string Login(string userName, string password);
        string Signup(string userName, string password, string businessName);
        string Integrate(string userName, string token, string erpName, string ecommerceName, string integrationIp);
        string Publish(string username, string token);
        JObject GetArticles(string username, string token);

    }
}
