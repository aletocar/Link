using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Business
{
    interface IClientController
    {

        string Login(string userName, string password);
        string Integrate(string userName, string token, string erpName);
        string Publish(string username, string token);
    }
}
