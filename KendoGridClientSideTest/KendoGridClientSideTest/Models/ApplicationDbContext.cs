using System.Data.Entity;

namespace KendoGridClientSideTest.Models
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext()
           : base("testDb")
        {
        }
    }
}