using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MercadoLibreController.LINK.Controllers;

namespace PruebaOauth
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["code"] != "")
                {
                    MercadoLibreController.LINK.Controllers.MercadoLibreController m = MercadoLibreController.LINK.Controllers.MercadoLibreController.GetController();
                    m.Code = Request.QueryString["code"];
                    m.Authorize();

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}