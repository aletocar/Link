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
                    Integration ecommerce_int;
                    using (LinkContext ctx = new LinkContext())
                    {
                        Client client = ctx.ClientUsers.Where(usr => usr.UserName == username).FirstOrDefault().IsUserOf;
                        ecommerce_int = ctx.Integrations.Where(integ => integ.ClientIntegrated == client).FirstOrDefault();
                    }
                    IEcommerceIntegration ecommerceIntegration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(ecommerce_int.EcommerceIntegrated.EcommerceName);
                    ecommerceIntegration.SetCredentials(ecommerce_int.EcommerceIntegrated.EcommerceAppId, ecommerce_int.EcommerceIntegrated.EcommerceSecret, ecommerce_int.EcommerceAccessToken);
                    string json = "";
                    //Build Json
                    return ecommerceIntegration.Publish(json);
                }
            }
            return "error";
        }

        public string IntegrateERP(string userName, string token, string erpName, string integrationIp)
        {
            Client client;
            CustomerErp erp;
            ClientUser user;
            using (LinkContext ctx = new LinkContext())
            {
                erp = ctx.CustomerErps.Where(er => er.Name == erpName).FirstOrDefault();
                user = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == userName).FirstOrDefault();
                client = user.IsUserOf;
                Integration integration = new Integration() { ClientIntegrated = client, ErpIntegrated = erp, IntegrationIp = integrationIp };
                ctx.Integrations.Add(integration);
                ctx.SaveChanges();
                return "ok";
            }
        }

        public string IntegrateEcommerce(string userName, string token, string ecommerceName)
        {
            Client client;
            Ecommerce ecommerce;
            ClientUser user;
            using (LinkContext ctx = new LinkContext())
            {
                ecommerce = ctx.Ecommerces.Where(eco => eco.EcommerceName == ecommerceName).FirstOrDefault();
                user = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == userName).FirstOrDefault();
                client = user.IsUserOf;
            }

            IEcommerceIntegration ecommerce_integration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(ecommerce.EcommerceName);
            return ecommerce_integration.Connect();
        }

        public string AuthorizeEcommerce(string userName, string token, string ecommerceName, string code)
        {
            Client client;
            Ecommerce ecommerce;
            ClientUser user;
            using (LinkContext ctx = new LinkContext())
            {
                ecommerce = ctx.Ecommerces.Where(eco => eco.EcommerceName == ecommerceName).FirstOrDefault();
                user = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == userName).FirstOrDefault();
                client = user.IsUserOf;
            }
            IEcommerceIntegration ecommerce_integration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(ecommerce.EcommerceName);
            string token_ecommerce = ecommerce_integration.Authorize(code);
            using (LinkContext ctx = new LinkContext())
            {
                Integration integration = ctx.Integrations.Include("ClientIntegrated").Include("EcommerceIntegrated").Where(inte => inte.ClientIntegrated.ClientId == client.ClientId && inte.EcommerceIntegrated == null).FirstOrDefault();
                ctx.Integrations.Attach(integration);
                integration.EcommerceIntegrated = ecommerce;
                integration.EcommerceCode = code;
                integration.EcommerceAccessToken = token_ecommerce;
                ctx.SaveChanges();
                return "ok";
            }
        }

        public JObject GetArticles(string username, string token)
        {
            Integration integration;
            using (LinkContext ctx = new LinkContext())
            {
                ClientUser user = ctx.ClientUsers.Include("IsUserOf").Where(cli => cli.UserName == username).FirstOrDefault();
                Client client = user.IsUserOf;
                integration = ctx.Integrations.Include("ErpIntegrated").Where(igr => igr.ClientIntegrated.ClientId == client.ClientId).FirstOrDefault();

            }
            IERPIntegration erp_integration = IntegrationFactory.IntegrationFactory.GetERPIntegration(integration.ErpIntegrated.Name);
            return erp_integration.GetArticles(integration.IntegrationIp, integration.ERPUserName, integration.ERPPassword);
        }
    }
}
