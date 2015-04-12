using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Link.ERPIntegrationInterface;
using Common;


namespace IntegrationZetaPrueba2
{
    public class ZetaIntegrationImp:IERPIntegration
    {
        public ZetaIntegrationImp()
        {

        }

        private BasicHttpBinding CreateHttpBinding(string name)
        {
            BasicHttpBinding _HTTPBinding = new BasicHttpBinding();
            _HTTPBinding.Name = name;
            _HTTPBinding.CloseTimeout = TimeSpan.FromMinutes(1);
            _HTTPBinding.OpenTimeout = TimeSpan.FromMinutes(1);
            _HTTPBinding.ReceiveTimeout = TimeSpan.FromMinutes(1);
            _HTTPBinding.SendTimeout = TimeSpan.FromMinutes(1);
            _HTTPBinding.MaxBufferSize = 2147483647;
            _HTTPBinding.MaxBufferPoolSize = 524288;
            _HTTPBinding.MaxReceivedMessageSize = 2147483647;
            _HTTPBinding.MessageEncoding = WSMessageEncoding.Text;
            _HTTPBinding.TextEncoding = System.Text.Encoding.UTF8;
            _HTTPBinding.TransferMode = TransferMode.Buffered;
            _HTTPBinding.UseDefaultWebProxy = true;
            _HTTPBinding.Security.Mode = BasicHttpSecurityMode.None;
            _HTTPBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            _HTTPBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            _HTTPBinding.Security.Transport.Realm = "";
            _HTTPBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            return _HTTPBinding;
        }

        private EndpointAddress CreateEndpoint(String url)
        {
            String _EndPointUrl = url;
            EndpointAddress _Endpoint = new EndpointAddress(_EndPointUrl);
            return _Endpoint;
        }

        public List<EcommerceItem> GetArticles(string ip_company, string user_company, string password_company)
        {
            EXPWSArticulos.ArticulosSDTArticuloItem[] _ListArticles = new EXPWSArticulos.ArticulosSDTArticuloItem[1];

            string _MensajeError = "";
            bool _ClientesExportados = false;
            DateTime _Inicio = new DateTime();
            JObject _JsonArticles = new JObject();
            string _JsonList = "";

            BasicHttpBinding _HttpBinding = this.CreateHttpBinding("ExpWSArticulosSoapBinding");

            String url = "http://" + ip_company + ":8080/libra/servlet/aexpwsarticulos";
            EndpointAddress _Endpoint = this.CreateEndpoint(url);
            List<Common.EcommerceItem> listaArticulosCommon = new List<Common.EcommerceItem>();
            try
            {
                EXPWSArticulos.ExpWSArticulosSoapPortClient _WebServiceArticulos = new EXPWSArticulos.ExpWSArticulosSoapPortClient(_HttpBinding, _Endpoint);
                _WebServiceArticulos.Execute(user_company, password_company, "", 0, 0, "20150412", "", out _MensajeError, out _ListArticles);

                foreach (EXPWSArticulos.ArticulosSDTArticuloItem i in _ListArticles)
                {
                    listaArticulosCommon.Add(new EcommerceItem(){ Codigo = i.Codigo, Costo = i.Costo, Nombre = i.Nombre});
                }
            }
            catch (Exception e)
            {
                _MensajeError = e.Message;
            }

            return listaArticulosCommon;
        }


        public JObject PostPurchase(string ip_company, string user_company, string password_company)
        {

            IMPWSMovements.ImpMovimientosSDTMovimientoItem _Movement = CreateMovement();


            string _MensajeError = "";
            bool _ClientesExportados = false;
            DateTime _Inicio = new DateTime();
            JObject _JsonArticles = new JObject();
            string _JsonList = "";

            BasicHttpBinding _HttpBinding = this.CreateHttpBinding("ImpWSMovimientoSoapBinding");

            String url = "http://" + ip_company + ":8080/libra/servlet/aimpwsmovimiento";
            EndpointAddress _Endpoint = this.CreateEndpoint(url);

            try
            {

                IMPWSMovements.ImpWSMovimientoSoapPortClient _WebServiceMovement = new IMPWSMovements.ImpWSMovimientoSoapPortClient(_HttpBinding, _Endpoint);
                _WebServiceMovement.Execute(user_company, password_company, _Movement, out _MensajeError);




            }
            catch (Exception e)
            {
                _JsonArticles.Add(new JProperty("Error", true));
                _MensajeError = e.Message;
            }

            _JsonArticles.Add(new JProperty("Message", _MensajeError));

            return _JsonArticles;

        }

        private IMPWSMovements.ImpMovimientosSDTMovimientoItem CreateMovement()
        {
            IMPWSMovements.ImpMovimientosSDTMovimientoItem _Movement = new IMPWSMovements.ImpMovimientosSDTMovimientoItem();


            _Movement.CodigoComprobante = 196;
            _Movement.Serie = "A";
            //  _Movement.Numero = 1;
            _Movement.Fecha = "20150411";
            _Movement.CodigoMoneda = 1;
            // _Movement.Cotizacion = "";
            _Movement.CodigoCliente = "441503766";
            //_Movement.CodigoVendedor = Me.txtMovimientoCodigoVendedor.Text
            //_Movement.CodigoPrecio = CShort(Me.txtMovimientoCodigoPrecio.Text)
            //_Movement.CodigoCondicionPago = Me.txtMovimientoCodigoCondicionPago.Text
            _Movement.CodigoDepositoOrigen = 100;
            _Movement.CodigoDepositoDestino = 101;
            _Movement.FechaEntrega = "20150411";
            //_Movement.CodigoCentroCosto = Me.txtMovimientoCodigoCentroCosto.Text
            //_Movement.CodigoReferencia = Me.txtMovimientoCodigoReferencia.Text
            //_Movement.Notas = Me.txtMovimientoNotas.Text
            _Movement.CodigoLocal = 12;
            _Movement.CodigoUsuario = 36;
            _Movement.CodigoCaja = 196;

            JObject _ArticleInfo = new JObject();

            List<IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem> _Lines = CreateLines(_ArticleInfo);

            _Movement.Lineas = _Lines.ToArray();
            //_Movement.FormasPago = _MovimientoNuevoFormasPago.ToArray()

            return _Movement;

        }

        public List<IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem> CreateLines(JObject _ArticleInfo)
        {
            IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem _Line = new IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem();
            List<IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem> _ListLines = new List<IMPWSMovements.ImpMovimientosSDTMovimientoItemLineaItem>();
            _Line.Cantidad = 1.0;
            _Line.CodigoArticulo = "30540013";
            //no cambiar
            _Line.CodigoIVA = 1;
            _Line.CodigoLote = "";
            _Line.Concepto = "Descripcion";
            _Line.PrecioUnitario = 10;
            _Line.Descuento1 = 0.0;
            _Line.Descuento2 = 0.0;
            _Line.Descuento3 = 0.0;

            _ListLines.Add(_Line);




            return _ListLines;
        }

        public double ArticleQuantity(EXPWSStock.StockActualSdtStockActualItem[] _ListQuanityArticle)
        {
            double _Quantity = 0;

            foreach (EXPWSStock.StockActualSdtStockActualItem _ArticleStock in _ListQuanityArticle)
            {
                _Quantity = _Quantity + _ArticleStock.Stock;
            }

            return _Quantity;
        }

        public double GetStock(string ip_company, string user_company, string password_company)
        {
            string _JsonList = "";
            List<JObject> _ListJsonStock = new List<JObject>();
            string _MensajeError = "";
            BasicHttpBinding _HttpBinding = this.CreateHttpBinding("ExpWSStockActualSoapBinding");
            String url = "http://" + ip_company + ":8080/libra/servlet/aexpwsstockactual";
            EndpointAddress _Endpoint = this.CreateEndpoint(url);
            EXPWSStock.StockActualSdtStockActualItem[] _ListStock = new EXPWSStock.StockActualSdtStockActualItem[100];


            EXPWSStock.ExpWSStockActualSoapPortClient _WebServiceStock = new EXPWSStock.ExpWSStockActualSoapPortClient(_HttpBinding, _Endpoint);
            _WebServiceStock.Execute(user_company, password_company, "98002455", 0, out _MensajeError, out _ListStock);

            List<EXPWSStock.StockActualSdtStockActualItem> _ListOrderByArticleCode = _ListStock.ToList().OrderBy(o => o.CodigoArticulo.ToString()).ToList();

            string _ArticleCode = "";
            double _Count = 0.0;
            foreach (EXPWSStock.StockActualSdtStockActualItem _Stock in _ListOrderByArticleCode)
            {
                if (_Stock.CodigoArticulo.Equals(_ArticleCode))
                {
                    _ArticleCode = _Stock.CodigoArticulo;
                    _Count = _Count + _Stock.Stock;
                }
                else
                {
                    if (!_ArticleCode.Equals(""))
                    {

                        JObject _JsonStock = new JObject();
                        _JsonStock.Add(new JProperty("Codigo", _ArticleCode));
                        _JsonStock.Add(new JProperty("Cantidad", _Count));
                        _ListJsonStock.Add(_JsonStock);
                        _ArticleCode = _Stock.CodigoArticulo;

                        if (_MensajeError.Equals("No se encontró stock actual"))
                        {
                            _Count = 0;
                        }
                        else
                        {
                            _Count = _Stock.Stock;
                        }
                    }
                    else
                    {
                        _ArticleCode = _Stock.CodigoArticulo;
                        _Count = _Stock.Stock;
                    }

                }
            }

            JObject _JsonStockFinal = new JObject();
            _JsonStockFinal.Add(new JProperty("Codigo", _ArticleCode));
            _JsonStockFinal.Add(new JProperty("Cantidad", _Count));
            _ListJsonStock.Add(_JsonStockFinal);
            _JsonList = JsonConvert.SerializeObject(_ListJsonStock);
            JObject ret = new JObject();
            ret.Add(new JProperty("List", _JsonList));
            return _Count;

        }
    }
}