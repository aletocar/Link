using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class ClientUser
    {
        public int ClientUserId { get; set; }
        public Client IsUserOf { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ClientUser() { }
    }
}
