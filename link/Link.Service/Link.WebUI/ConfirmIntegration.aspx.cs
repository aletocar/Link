using System;
using System.Collections.Generic;
using System.Linq;
using Link.Business;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace Link.WebUI
{
    public partial class ConfirmIntegration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IClientController controller = new ClientController();
            string result = controller.GetArticles("NOVA", "").ToString();
            if (result == "ok")
            {
                controller.Publish("NOVA", "");
            }
            else { lblAnuncio.Text = "Ha ocurrido un error, intente nuevamente"; }

        }

        protected void btnGetLastPurchase_Click(object sender, EventArgs e)
        {
            IClientController controller = new ClientController();
            DtoPurchase p = controller.GetLastPurchase();
            controller.PostPurchase("201.221.29.3", "NOVA", "NOVA", "98002455", Convert.ToDouble(p.Quantity));
        }
    }
}