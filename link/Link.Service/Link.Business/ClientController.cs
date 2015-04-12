using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link.Entities;
using Newtonsoft.Json.Linq;
using Link.ERPIntegrationInterface;
using Link.IntegrationFactory;
//using ERPIntegrationInterface;
using MercadoLibreController.LINK.Entidades;
using Link.EcommerceIntegrationInterface;
using Newtonsoft.Json;
using Common;

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

                    ClientUser new_user = new ClientUser() { UserName = userName, Password = password, IsUserOf = new_client };
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
            Integration ecommerce_int;
            using (LinkContext ctx = new LinkContext())
            {
                Client client = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == username).FirstOrDefault().IsUserOf;
                ecommerce_int = ctx.Integrations.Include("EcommerceIntegrated").Where(integ => integ.ClientIntegrated.ClientId == client.ClientId).FirstOrDefault();
            }
            IEcommerceIntegration ecommerceIntegration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(ecommerce_int.EcommerceIntegrated.EcommerceName);
            ecommerceIntegration.SetCredentials(ecommerce_int.EcommerceIntegrated.EcommerceAppId, ecommerce_int.EcommerceIntegrated.EcommerceSecret, ecommerce_int.EcommerceAccessToken);
            List<Item> listaItems = new List<Item>();
            using (LinkContext ctx = new LinkContext())
            {
                listaItems = ctx.Items.Where(itm => itm.Integration.IntegrationId == ecommerce_int.IntegrationId).ToList();
            }
            List<IEcommerceItem> listaItemsEcom = new List<IEcommerceItem>();
            foreach (Item item in listaItems)
            {
                listaItemsEcom.Add(new ArticleERP() { available_quantity = item.available_quantity, description = item.description, price = item.price, title = item.title, warranty = item.warranty });
            }
            //string json = JsonConvert.SerializeObject(listaItemsEcom);
            return ecommerceIntegration.Publish(listaItemsEcom);
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
                Integration integration = new Integration() { ClientIntegrated = client, ErpIntegrated = erp, IntegrationIp = integrationIp, ERPPassword = token, ERPUserName = userName };
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
                Integration integration = ctx.Integrations
                    .Include("ClientIntegrated")
                    .Include("EcommerceIntegrated")
                    .Where(inte => inte.ClientIntegrated.ClientId == client.ClientId)
                    .FirstOrDefault();
                ctx.Integrations.Attach(integration);
                integration.EcommerceIntegrated = ecommerce;
                integration.EcommerceCode = code;
                integration.EcommerceAccessToken = token_ecommerce;
                ctx.SaveChanges();
                return "ok";
            }
        }
        public string GetArticles(string username, string token)
        {
            Integration integration;
            using (LinkContext ctx = new LinkContext())
            {
                ClientUser user = ctx.ClientUsers.Include("IsUserOf").Where(cli => cli.UserName == username).FirstOrDefault();
                Client client = user.IsUserOf;
                integration = ctx.Integrations.Include("EcommerceIntegrated").Include("ErpIntegrated").Where(igr => igr.ClientIntegrated.ClientId == client.ClientId).FirstOrDefault();

            }
            IERPIntegration erp_integration = IntegrationFactory.IntegrationFactory.GetERPIntegration(integration.ErpIntegrated.Name);

            List<Common.EcommerceItem> articulos_erp = erp_integration.GetArticles(integration.IntegrationIp, integration.ERPUserName, integration.ERPPassword);
            double stock = erp_integration.GetStock(integration.IntegrationIp, integration.ERPUserName, integration.ERPPassword);
            //JsonArticulos articulosERP = JsonConvert.DeserializeObject<JsonArticulos>(string_articulos_erp);
            //JsonCantArticulos articulosERPCant = JsonConvert.DeserializeObject<JsonCantArticulos>(string_articulos_erp_cant);
            using (LinkContext ctx = new LinkContext())
            {
                foreach (Common.EcommerceItem articuloERP in articulos_erp)
                {
                    //Item it_cantidad = articulosERPCant.List.Find(it => it.Codigo == articuloERP.Codigo);
                    Item item = new Item() { Integration = integration, Codigo = articuloERP.Codigo, Nombre = articuloERP.Nombre, Costo = articuloERP.Costo, description = articuloERP.Nombre, price = Convert.ToInt32(articuloERP.Costo), title = articuloERP.Nombre, warranty = "No Warranty", available_quantity = Convert.ToInt32(stock), Cantidad = stock.ToString() };
                    ctx.Integrations.Attach(integration);
                    ctx.Items.Add(item);
                    ctx.SaveChanges();
                }
            }
            return "ok";
        }
        class JsonArticulos
        {
            public bool Error { get; set; }
            public string Message { get; set; }
            public List<JsonItem> List { get; set; }
            public JsonArticulos() { }
        }
        class JsonCantArticulos
        {
            public List<Item> List { get; set; }
            public JsonCantArticulos() { }
        }
        class JsonItem : IERPItem
        {

            public string Nombre
            {
                get;
                set;
            }

            public double Costo
            {
                get;
                set;
            }

            public string Codigo
            {
                get;
                set;
            }
        }
        class ArticulosSDTArticuloItem
        {
            private string codigoField;

            private string nombreField;

            private string abreviacionField;

            private string codigoCategoriaField;

            private string codigoFamiliaField;

            private string codigoMarcaField;

            private string codigoProveedorField;

            private sbyte codigoIVAField;

            private string cuentaComprasField;

            private string cuentaVentasField;

            private string cuentaProduccionField;

            private string codigoOrigenField;

            private string codigoUnidadField;

            private string webField;

            private string codigoTextoField;

            private string notasField;

            private string activoField;

            private string listaDePreciosField;

            private string usaStockField;

            private string usaLotesField;

            private string usaVencimientosField;

            private sbyte codigoMonedaCostosField;

            private string fechaCostoField;

            private double costoField;

            private double porcentajeUtilidadField;

            /// <remarks/>
            public string Codigo
            {
                get
                {
                    return this.codigoField;
                }
                set
                {
                    this.codigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }

            /// <remarks/>
            public string Nombre
            {
                get
                {
                    return this.nombreField;
                }
                set
                {
                    this.nombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }

            /// <remarks/>
            public string Abreviacion
            {
                get
                {
                    return this.abreviacionField;
                }
                set
                {
                    this.abreviacionField = value;
                    this.RaisePropertyChanged("Abreviacion");
                }
            }

            /// <remarks/>
            public string CodigoCategoria
            {
                get
                {
                    return this.codigoCategoriaField;
                }
                set
                {
                    this.codigoCategoriaField = value;
                    this.RaisePropertyChanged("CodigoCategoria");
                }
            }

            /// <remarks/>
            public string CodigoFamilia
            {
                get
                {
                    return this.codigoFamiliaField;
                }
                set
                {
                    this.codigoFamiliaField = value;
                    this.RaisePropertyChanged("CodigoFamilia");
                }
            }

            /// <remarks/>
            public string CodigoMarca
            {
                get
                {
                    return this.codigoMarcaField;
                }
                set
                {
                    this.codigoMarcaField = value;
                    this.RaisePropertyChanged("CodigoMarca");
                }
            }

            /// <remarks/>
            public string CodigoProveedor
            {
                get
                {
                    return this.codigoProveedorField;
                }
                set
                {
                    this.codigoProveedorField = value;
                    this.RaisePropertyChanged("CodigoProveedor");
                }
            }

            /// <remarks/>
            public sbyte CodigoIVA
            {
                get
                {
                    return this.codigoIVAField;
                }
                set
                {
                    this.codigoIVAField = value;
                    this.RaisePropertyChanged("CodigoIVA");
                }
            }

            /// <remarks/>
            public string CuentaCompras
            {
                get
                {
                    return this.cuentaComprasField;
                }
                set
                {
                    this.cuentaComprasField = value;
                    this.RaisePropertyChanged("CuentaCompras");
                }
            }

            /// <remarks/>
            public string CuentaVentas
            {
                get
                {
                    return this.cuentaVentasField;
                }
                set
                {
                    this.cuentaVentasField = value;
                    this.RaisePropertyChanged("CuentaVentas");
                }
            }

            /// <remarks/>
            public string CuentaProduccion
            {
                get
                {
                    return this.cuentaProduccionField;
                }
                set
                {
                    this.cuentaProduccionField = value;
                    this.RaisePropertyChanged("CuentaProduccion");
                }
            }

            /// <remarks/>
            public string CodigoOrigen
            {
                get
                {
                    return this.codigoOrigenField;
                }
                set
                {
                    this.codigoOrigenField = value;
                    this.RaisePropertyChanged("CodigoOrigen");
                }
            }

            /// <remarks/>
            public string CodigoUnidad
            {
                get
                {
                    return this.codigoUnidadField;
                }
                set
                {
                    this.codigoUnidadField = value;
                    this.RaisePropertyChanged("CodigoUnidad");
                }
            }

            /// <remarks/>
            public string Web
            {
                get
                {
                    return this.webField;
                }
                set
                {
                    this.webField = value;
                    this.RaisePropertyChanged("Web");
                }
            }

            /// <remarks/>
            public string CodigoTexto
            {
                get
                {
                    return this.codigoTextoField;
                }
                set
                {
                    this.codigoTextoField = value;
                    this.RaisePropertyChanged("CodigoTexto");
                }
            }

            /// <remarks/>
            public string Notas
            {
                get
                {
                    return this.notasField;
                }
                set
                {
                    this.notasField = value;
                    this.RaisePropertyChanged("Notas");
                }
            }

            /// <remarks/>
            public string Activo
            {
                get
                {
                    return this.activoField;
                }
                set
                {
                    this.activoField = value;
                    this.RaisePropertyChanged("Activo");
                }
            }

            /// <remarks/>
            public string ListaDePrecios
            {
                get
                {
                    return this.listaDePreciosField;
                }
                set
                {
                    this.listaDePreciosField = value;
                    this.RaisePropertyChanged("ListaDePrecios");
                }
            }

            /// <remarks/>
            public string UsaStock
            {
                get
                {
                    return this.usaStockField;
                }
                set
                {
                    this.usaStockField = value;
                    this.RaisePropertyChanged("UsaStock");
                }
            }

            /// <remarks/>
            public string UsaLotes
            {
                get
                {
                    return this.usaLotesField;
                }
                set
                {
                    this.usaLotesField = value;
                    this.RaisePropertyChanged("UsaLotes");
                }
            }

            /// <remarks/>
            public string UsaVencimientos
            {
                get
                {
                    return this.usaVencimientosField;
                }
                set
                {
                    this.usaVencimientosField = value;
                    this.RaisePropertyChanged("UsaVencimientos");
                }
            }

            /// <remarks/>
            public sbyte CodigoMonedaCostos
            {
                get
                {
                    return this.codigoMonedaCostosField;
                }
                set
                {
                    this.codigoMonedaCostosField = value;
                    this.RaisePropertyChanged("CodigoMonedaCostos");
                }
            }

            /// <remarks/>
            public string FechaCosto
            {
                get
                {
                    return this.fechaCostoField;
                }
                set
                {
                    this.fechaCostoField = value;
                    this.RaisePropertyChanged("FechaCosto");
                }
            }

            /// <remarks/>
            public double Costo
            {
                get
                {
                    return this.costoField;
                }
                set
                {
                    this.costoField = value;
                    this.RaisePropertyChanged("Costo");
                }
            }

            /// <remarks/>
            public double PorcentajeUtilidad
            {
                get
                {
                    return this.porcentajeUtilidadField;
                }
                set
                {
                    this.porcentajeUtilidadField = value;
                    this.RaisePropertyChanged("PorcentajeUtilidad");
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
        public DtoPurchase GetLastPurchase()
        {
            Integration integration;
            using (LinkContext ctx = new LinkContext())
            {
                ClientUser user = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == "NOVA").FirstOrDefault();
                Client cliente = user.IsUserOf;
                integration = ctx.Integrations.Include("EcommerceIntegrated").Where(inte => inte.ClientIntegrated.ClientId == cliente.ClientId).FirstOrDefault();
            }
            IEcommerceIntegration erp_integration = IntegrationFactory.IntegrationFactory.GetEcommerceIntegration(integration.EcommerceIntegrated.EcommerceName);
            erp_integration.SetCredentials(integration.EcommerceIntegrated.EcommerceAppId, integration.EcommerceIntegrated.EcommerceSecret, integration.EcommerceAccessToken);
            return erp_integration.GetLastPurchase();
        }

        public void PostPurchase(string ip_company, string user_company, string password_company, string product_id, double product_quantity)
        {
            Integration integration;
            using (LinkContext ctx = new LinkContext())
            {
                Client client = ctx.ClientUsers.Include("IsUserOf").Where(usr => usr.UserName == user_company).FirstOrDefault().IsUserOf;
                integration = ctx.Integrations.Include("ErpIntegrated").Where(integ => integ.ClientIntegrated.ClientId == client.ClientId).FirstOrDefault();
            }

            IERPIntegration erp_integration = IntegrationFactory.IntegrationFactory.GetERPIntegration(integration.ErpIntegrated.Name);
            erp_integration.PostPurchase(integration.IntegrationIp, integration.ERPUserName, integration.ERPPassword, product_id, product_quantity);
        }
    }
}
