using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class CustomerErp
    {
        public int CustomerErpId { get; set; }
        public string Name { get; set; }
        public bool IsSaas { get; set; }

        public CustomerErp() { }
    }
}
