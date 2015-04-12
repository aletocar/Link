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

            label.Text = controller.GetArticles("NOVA", "").ToString();
        }
    }
}