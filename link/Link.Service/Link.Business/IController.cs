﻿using System;
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
        string Integrate(string userName, string token, string erpName, string integrationIp);
        string Publish(string username, string token);

    }
}
