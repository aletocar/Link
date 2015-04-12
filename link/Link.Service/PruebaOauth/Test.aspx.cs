using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MercadoLibreController.LINK;

namespace PruebaOauth
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MercadoLibreController.LINK.Controllers.MercadoLibreController m = MercadoLibreController.LINK.Controllers.MercadoLibreController.GetController();
            string url = m.Connect();
            Response.Redirect(url);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
           
            
        }
    }
}