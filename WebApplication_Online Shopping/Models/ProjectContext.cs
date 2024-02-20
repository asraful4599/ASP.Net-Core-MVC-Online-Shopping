using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication_Online_Shopping.Models
{
    public class ProjectContext: DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext>options):base(options)
      
        {
            
        }
        public DbSet<Product> Products { get; set; }    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin>Admins{ get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData
                 (new Category { CategoryId = 1, CategoryName = "Fashions" },
                 new Category { CategoryId = 2, CategoryName = "Mobile" },
                 new Category { CategoryId = 3, CategoryName = "Laptop" },
                 new Category { CategoryId = 4, CategoryName = "Electronics" },
                 new Category { CategoryId = 5, CategoryName = "Personal Care" },
                 new Category { CategoryId = 6, CategoryName = "Sports" },
                 new Category { CategoryId = 7, CategoryName = "Furniture" }

                );
            modelBuilder.Entity<Admin>().HasData
                (new Admin { UserName = "sk@rana45", Password = "tapu4521" });
        }


       

    }
}
