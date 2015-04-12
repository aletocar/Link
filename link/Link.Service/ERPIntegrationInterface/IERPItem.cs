using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.ERPIntegrationInterface
{
   public interface IERPItem
    {
        string Nombre { get; set; }
        double Costo { get; set; }
        string Codigo { get; set; }
    }
}
