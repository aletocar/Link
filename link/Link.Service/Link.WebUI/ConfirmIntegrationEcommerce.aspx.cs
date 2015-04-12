using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Link.Business;

namespace Link.WebUI
{
    public partial class ConfirmIntegrationEcommerce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != "")
            {
                IClientController controller = new ClientController();
                controller.AuthorizeEcommerce("NOVA", "", "mercadolibre", Request.QueryString["code"]);
            }
        }

        protected void btnPublicar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/WebUI/ConfirmIntegration.aspx");
        }
    }
}