using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Link.Business
{
    public interface IClientController
    {

        string Login(string userName, string password);
        string Signup(string userName, string password, string businessName);
        //string Integrate(string userName, string token, string erpName, string ecommerceName, string integrationIp);
        string IntegrateERP(string userName, string token, string erpName, string integrationIp);
        string IntegrateEcommerce(string userName, string token, string ecommerceName);
        string AuthorizeEcommerce(string userName, string token, string ecommerceName, string code);
        string Publish(string username, string token);
        string GetArticles(string username, string token);
        DtoPurchase GetLastPurchase();
        void PostPurchase(string ip_company, string user_company, string password_company, string product_id, double product_quantity);
    }
}
