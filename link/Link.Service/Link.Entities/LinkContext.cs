using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Entities
{
    public class LinkContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<CustomerErp> CustomerErps { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<Ecommerce> Ecommerces { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
