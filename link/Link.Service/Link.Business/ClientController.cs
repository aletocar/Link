﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link.Entities;

namespace Link.Business
{
    public class ClientController : IClientController
    {
        public string Login(string userName, string password)
        {
            using (LinkContext ctx = new LinkContext())
            {
                ClientUser existing_user = ctx.ClientUsers.Find(userName);
                if (existing_user != null)
                {
                    if (existing_user.Password == password)
                    {
                        return "ok";
                    }
                    else
                    {
                        return "password error";
                    }
                }
                else
                {
                    return "user error";
                }
            }
        }
        public string Signup(string userName, string password, string businessName)
        {
            Client new_client = new Client() { ClientName = businessName };
            using (LinkContext ctx = new LinkContext())
            {
                Client existing_client = ctx.Clients.Find(businessName);
                if (existing_client == null)
                {
                    ClientUser new_user = new ClientUser() { UserName = userName, Password = password, IsUserOf = existing_client };
                    ClientUser existing_user = ctx.ClientUsers.Find(userName);
                    if (existing_user == null)
                    {
                        ctx.Clients.Add(new_client);
                        return "ok";
                    }
                    else
                    {
                        return "user already exists";
                    }
                }
                else
                {
                    return "client already exists";
                }
            }
        }

        public string Publish(string username, string token)
        {
            
        }

        public string Integrate(string userName, string token, string erpName, string integrationIp)
        {
            using (LinkContext ctx = new LinkContext())
            {
                CustomerErp erp = ctx.CustomerErps.Find(erpName);
                ClientUser user = ctx.ClientUsers.Find(userName);
                Client client = user.IsUserOf;

                Integration integration = new Integration() { ClientIntegrated = client, ErpIntegrated = erp, IntegrationIp = integrationIp };

                ctx.Integrations.Add(integration);
                return "ok";
            }
        }
    }
}
