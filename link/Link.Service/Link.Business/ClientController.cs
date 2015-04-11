using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link.Entities;
using Newtonsoft.Json.Linq;
using Link.ERPIntegrationInterface;
using Link.IntegrationFactory;
using Link.EcommerceIntegrationInterface;

namespace Link.Business
{
    public class ClientController : IClientController
    {
        public ClientController() { }
        public string Login(string userName, string password)
        {
            using (LinkContext ctx = new LinkContext())
            {
                ClientUser existing_user = ctx.ClientUsers.Where(usr => usr.UserName == userName).FirstOrDefault();
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
                Client existing_client = ctx.Clients.Where(cli => cli.ClientName == businessName).FirstOrDefault();
                if (existing_client == null)
                {
                    ClientUser new_user = new ClientUser() { UserName = userName, Password = password, IsUserOf = existing_client };
                    ClientUser existing_user = ctx.ClientUsers.Where(usr => usr.UserName == userName).FirstOrDefault();
                    if (existing_user == null)
                    {
                        ctx.ClientUsers.Add(new_user);
                        ctx.Clients.Add(new_client);
                        ctx.SaveChanges();
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
            JObject objetoAPublicar = GetArticles(username, token);
            foreach (var objects in objetoAPublicar)
            {
                if (objects.Key == "error" && objects.Value.ToString() == "true")
                {
                    return "error";
                }
                else
                {
                    Ecommerce ecommerce;
                    using(LinkContext ctx = new LinkContext())
                    {
                        Client client = ctx.ClientUsers.Where(usr => usr.UserName == username).FirstOrDefault().IsUserOf;
                        ecommerce = ctx.Integrations.Where(integ => integ.ClientIntegrated == client).FirstOrDefault().EcommerceIntegrated;
                    }
                    IEcommerceIntegration ecommerceIntegration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(ecommerce.EcommerceName);
                    return ecommerceIntegration.Publish();
                }
            }
            return "error";
        }

        public string Integrate(string userName, string token, string erpName, string ecommerceName, string integrationIp)
        {
            using (LinkContext ctx = new LinkContext())
            {
                CustomerErp erp = ctx.CustomerErps.Where(er => er.Name == erpName).FirstOrDefault();
                Ecommerce ecommerce = ctx.Ecommerces.Where(eco => eco.EcommerceName == ecommerceName).FirstOrDefault();
                ClientUser user = ctx.ClientUsers.Where(usr => usr.UserName == userName).FirstOrDefault();
                Client client = user.IsUserOf;

                Integration integration = new Integration() { ClientIntegrated = client, ErpIntegrated = erp, IntegrationIp = integrationIp };

                ctx.Integrations.Add(integration);
                ctx.SaveChanges();

                return "ok";
            }
        }

        public JObject GetArticles(string username, string token)
        {
            Integration integration;
            using (LinkContext ctx = new LinkContext())
            {
                Client client = ctx.ClientUsers.Where(cli => cli.UserName == username).FirstOrDefault().IsUserOf;
                integration = ctx.Integrations.Where(igr => igr.ClientIntegrated == client).FirstOrDefault();
                
            }
            IERPIntegration erp_integration = IntegrationFactory.IntegrationFactory.GetERPIntegration(integration.ErpIntegrated.Name);
            return erp_integration.GetArticles(integration.IntegrationIp, integration.ERPUserName, integration.ERPPassword);
        }
    }
}
