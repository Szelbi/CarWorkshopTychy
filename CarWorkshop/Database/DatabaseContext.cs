using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace CarWorkshop.Database
{


    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<DeliverType> DeliverTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }


        public DatabaseContext() : base()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(new DbInitializer());
        }

        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }
    }

    public class DbInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole("admin"));
            roleManager.Create(new IdentityRole("user"));

            var user = new ApplicationUser { UserName = "admin@admin.pl", Email = "admin@admin.pl" };
            var result = userManager.Create(user, "Admin123.");
            if (result.Succeeded)
            {
                result = userManager.AddToRole(user.Id, "admin");
            }


            List<Category> list = new List<Category>();
            for (int i = 1; i < 5; i++)
            {
                Category c = new Category();
                c.CategoryName = "Kategoria " + i;
                c.Products = new List<Product>();
                for (int j = i; j < 5; j++)
                {
                    Product p = new Product();
                    p.ProductName = "Produkt " + i + j;
                    p.Description = "Opis produktu " + i + j;
                    p.Price = i * j;
                    c.Products.Add(p);
                }
                context.Categories.Add(c);
            }

            for (int i = 0; i < 4; i++)
            {
                DeliverType d = new DeliverType();
                if (i == 3)
                {
                    d.Name = "Odbiór osobisty";
                    d.Price = 0;
                }
                else
                {
                    d.Name = "Kurier " + i;
                    d.Price = 5 + i;
                }
                context.DeliverTypes.Add(d);
            }

            context.PaymentTypes.Add(new PaymentType() { Name = "Płatność DotPay", Price = 0 });
            context.PaymentTypes.Add(new PaymentType() { Name = "Płatne przy odbiorze", Price = 3 });

            context.SaveChanges();

            base.Seed(context);
        }
    }

}