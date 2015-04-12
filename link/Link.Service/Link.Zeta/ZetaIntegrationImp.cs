using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Link.ERPIntegrationInterface;


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

        public JObject GetArticles(string ip_company, string user_company, string password_company)
        {
            EXPWSArticulos.ArticulosSDTArticuloItem[] _ListArticles = new EXPWSArticulos.ArticulosSDTArticuloItem[100];

            string _MensajeError = "";
            bool _ClientesExportados = false;
            DateTime _Inicio = new DateTime();
            JObject _JsonArticles = new JObject();
            string _JsonList = "";

            BasicHttpBinding _HttpBinding = this.CreateHttpBinding("ExpWSArticulosSoapBinding");

            String url = "http://" + ip_company + ":8080/libra/servlet/aexpwsarticulos";
            EndpointAddress _Endpoint = this.CreateEndpoint(url);

            try
            {
                EXPWSArticulos.ExpWSArticulosSoapPortClient _WebServiceArticulos = new EXPWSArticulos.ExpWSArticulosSoapPortClient(_HttpBinding, _Endpoint);
                _WebServiceArticulos.Execute(user_company, password_company, "", 0, 0, "", "", out _MensajeError, out _ListArticles);

                _JsonList = JsonConvert.SerializeObject(_ListArticles);
             

                _JsonArticles.Add(new JProperty("Error", false));
              
            }
            catch (Exception e)
            {
                _JsonArticles.Add(new JProperty("Error", true));
                _MensajeError = e.Message;
            }

            _JsonArticles.Add(new JProperty("Message", _MensajeError));
            _JsonArticles.Add(new JProperty("List", _JsonList));
          return _JsonArticles;
        }

        //public JObject PostPurchase(string ip_company, string user_company, string password_company)
        //{

            //ZetaIntegration.IMPWSMovements.ImpMovimientosSDTMovimientoItem _Movement = CreateMovement();
            

        //    string _MensajeError = "";
        //    bool _ClientesExportados = false;
        //    DateTime _Inicio = new DateTime();
        //    JObject _JsonArticles = new JObject();
        //    string _JsonList = "";

        //    BasicHttpBinding _HttpBinding = this.CreateHttpBinding("ImpWSMovimientoSoapBinding");

        //    String url = "http://" + ip_company + ":8080/libra/servlet/aimpwsmovimiento";
        //    EndpointAddress _Endpoint = this.CreateEndpoint(url);

        //    try
        //    {

        //        IMPWSMovements.ImpWSMovimientoSoapPortClient _WebServiceMovement = new IMPWSMovements.ImpWSMovimientoSoapPortClient(_HttpBinding, _Endpoint);
        //        _WebServiceMovement.Execute(user_company, password_company, _Movement,out _MensajeError);
                



        //    }
        //    catch (Exception e)
        //    {
        //        _JsonArticles.Add(new JProperty("Error", true));
        //        _MensajeError = e.Message;
        //    }

        //    _JsonArticles.Add(new JProperty("Message", _MensajeError));

        //    return _JsonArticles;

        //}

        //private IMPWSMovements.ImpMovimientosSDTMovimientoItem CreateMovement()
        //{
        //     IMPWSMovements.ImpMovimientosSDTMovimientoItem _Movement = new IMPWSMovements.ImpMovimientosSDTMovimientoItem();

            
        //    _Movement.CodigoComprobante = 195;
        //    _Movement.Serie = "A";
        //    _Movement.Numero = 1;
        //    _Movement.Fecha = "20150411";
        //    _Movement.CodigoMoneda = CSByte(Me.txtMovimientoCodigoMoneda.Text);
        //   // _Movement.Cotizacion = "";
        //    _Movement.CodigoCliente = Me.txtMovimientoCodigoCliente.Text
        //    //_Movement.CodigoVendedor = Me.txtMovimientoCodigoVendedor.Text
        //    //_Movement.CodigoPrecio = CShort(Me.txtMovimientoCodigoPrecio.Text)
        //    //_Movement.CodigoCondicionPago = Me.txtMovimientoCodigoCondicionPago.Text
        //   // _Movement.CodigoDepositoOrigen = CShort(Me.txtMovimientoCodigoDepositoOrigen.Text)
        //    //_Movement.CodigoDepositoDestino = CShort(Me.txtMovimientoCodigoDepositoDestino.Text)
        //    _Movement.FechaEntrega ="20150413";
        //    //_Movement.CodigoCentroCosto = Me.txtMovimientoCodigoCentroCosto.Text
        //    //_Movement.CodigoReferencia = Me.txtMovimientoCodigoReferencia.Text
        //    //_Movement.Notas = Me.txtMovimientoNotas.Text
        //    _Movement.CodigoLocal = CShort(Me.txtMovimientoCodigoLocal.Text)
        //    _Movement.CodigoUsuario = CShort(Me.txtMovimientoCodigoUsuario.Text)
        //    _Movement.CodigoCaja = CShort(Me.txtMovimientoCodigoCaja.Text)

        //    //_Movement.Lineas = _MovimientoNuevoLineas.ToArray()
        //    //_Movement.FormasPago = _MovimientoNuevoFormasPago.ToArray()

        //    return _Movement;
                  
        //}
    }
}