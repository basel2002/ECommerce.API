using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Context
{
    public  class AppDbContext:IdentityDbContext<ApplicationUser>
    {

        public AppDbContext():base()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints here if needed
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }






        // DBsets
        public virtual DbSet<Cart> Carts => Set<Cart>();
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Categorie> Categories => Set<Categorie>();
        public virtual DbSet<Order> Orders => Set<Order>();
        public virtual DbSet<CartItem> CartItems => Set<CartItem>();
        public virtual DbSet<OrderItem> OrderItems => Set<OrderItem>();
        
        




    }
}
