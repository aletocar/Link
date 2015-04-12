using Link.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft;

namespace Link.WebUI
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IClientController controller = new ClientController();
            controller.Signup("NOVA", "NOVA", "NOVA");
            controller.IntegrateERP("NOVA", "NOVA", "ZETA", "201.221.29.3");
            string url = controller.IntegrateEcommerce("NOVA", "", "Mercadolibre");
            Response.Redirect(url);
        }
    }
}