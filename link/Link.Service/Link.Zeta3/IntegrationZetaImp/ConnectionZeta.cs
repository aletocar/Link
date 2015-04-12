using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;


namespace IntegrationZeta
{
    public class ConnectionZeta
    {
        public ConnectionZeta()
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

        private EndpointAddress CreateEndpoint(String url )
        {
            String  _EndPointUrl= url;
            EndpointAddress _Endpoint = new EndpointAddress(_EndPointUrl);
            return _Endpoint;
        }

        public void GetArticles(string ip_company, string user_company, string password_company)
        {
            Zeta.WS.EXPWSArticulos.ArticulosSDTArticuloItem[] _ListArticles = new Zeta.WS.EXPWSArticulos.ArticulosSDTArticuloItem[100];

           string  _MensajeError = "";
           bool _ClientesExportados = false;
           DateTime _Inicio = new DateTime();


           BasicHttpBinding _HttpBinding =  this.CreateHttpBinding("ExpWSArticulosSoapBinding");
       
           String url = "http://"+ip_company+":8080/libra/servlet/aexpwsarticulos";
           EndpointAddress _Endpoint = this.CreateEndpoint(url);
            
           Zeta.WS.EXPWSArticulos.ExpWSArticulosSoapPortClient _WebServiceArticulos = new Zeta.WS.EXPWSArticulos.ExpWSArticulosSoapPortClient(_HttpBinding, _Endpoint);
            _WebServiceArticulos.Execute(user_company, password_company,"", "","", out _MensajeError,out _ListArticles);

            
        if (_MensajeError.Length == 0)
        {
             MessageBox.Show("Exito " + Me.tabZetaWS.SelectedTab.Text + vbCrLf + _MensajeError.Trim(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        }

        else
        {
        }

           



             
        }




    }
}