using MercadoLibre.SDK;
using MercadoLibreController.LINK.Entidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Link.EcommerceIntegrationInterface;


namespace MercadoLibreController.LINK.Controllers
{
    public class MercadoLibreController : IEcommerceIntegration
    {
        public string Code { get; set; }
        private Meli meli;
        private string token;
        private string refreshtoken;
        private long app_id;
        private string secret;
        //private List<Category> categories;


        public void GetCategoriesMercadoLibre(){
            var p = new Parameter ();
            p.Name = "access_token";
            p.Value = token;
            var ps = new List<Parameter> ();
            ps.Add (p);
            IRestResponse r = meli.Get("/sites/MLU/categories", ps);
            //categories = JsonConvert.DeserializeObject<List<Category>>(r.Content);
        }

        public MercadoLibreController()
        {

        }

        public void SetCredentials(long id, string secret, string token)
        {
            this.app_id = id;
            this.secret = secret;
            this.token = token;
        }

        public string Connect()
        {
            meli = new Meli(6811854697761224, "tjNL5zqG1GYbrSKYL5kOXbc6XOZXuSIE");
            string redirectUrl = meli.GetAuthUrl("http://localhost/WebUI/ConfirmIntegrationEcommerce.aspx");
            return redirectUrl;
        }

        public string Authorize(string code)
        {
            meli = new Meli(6811854697761224, "tjNL5zqG1GYbrSKYL5kOXbc6XOZXuSIE");
            Code = code;
            meli.Authorize(Code, "http://localhost/WebUI/ConfirmIntegrationEcommerce.aspx");
            //guardar el token y el code en la base de datos
            refreshtoken = meli.RefreshToken;
            token = meli.AccessToken;
            return  token;
        
        }

        public string Publish(string json)
        {
            var p = new Parameter();
            p.Name = "access_token";
            p.Value = token;
            List<ArticleERP> articulosERP = JsonConvert.DeserializeObject<List<ArticleERP>>(json);
            List<ArticleMeli> articulosMeli = new List<ArticleMeli>();
            foreach (ArticleERP a in articulosERP)
            {
                var ps = new List<Parameter>();
                ps.Add(p);
                IRestResponse r = meli.Post("/items", ps, new { title = a.title, buying_mode = "buy_it_now", condition = "new", category_id = "MLU1443", currency_id = "UYU", description = a.description, listing_type_id = "bronze", available_quantity = a.available_quantity, price = a.price, video_id = "", warranty = a.warranty});
            }
            
            //aca va un conversor del json a articulomeli
            return "ok";
        }
    }
}
