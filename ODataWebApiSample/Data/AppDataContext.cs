using Microsoft.EntityFrameworkCore;
using ODataWebApiSample.Entities;

namespace ODataWebApiSample.Data
{
    public class AppDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
            bool needToCreate = Database.EnsureCreated();
            if (needToCreate)
            {
                var brand1 = new Brand { Name = "Apple", Id = 1 };
                var brand2 = new Brand { Name = "Samsung", Id = 2 };
                Brands.Add(brand1);
                Brands.Add(brand2);
                Products.Add(new Product { Id = 1, Name = "12ProMax", Price = 1000, BrandId = 1 });
                Products.Add(new Product { Id = 2, Name = "12", Price = 1000, BrandId = 1 });
                Products.Add(new Product { Id = 3, Name = "A10", Price = 1000, BrandId = 2 });
                Products.Add(new Product { Id = 4, Name = "S2", Price = 1000, BrandId = 2 });
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
