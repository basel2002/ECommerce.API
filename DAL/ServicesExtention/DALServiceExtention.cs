using DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace DAL
{
    public static class DALServiceExtention
    {

        public static void AddDALServices(this IServiceCollection services ,IConfiguration configuration )
        {
            //Get Connection String 
            var connectionString = configuration.GetConnectionString("E-Commerce");

            services.AddDbContext<AppDbContext>(options =>

            {
                options
                .UseSqlServer(connectionString)
                .UseAsyncSeeding(async (context, _, _) =>
                {
                    if (await context.Set<Product>().AnyAsync())
                    {
                        return;
                    }
                    if (await context.Set<Categorie>().AnyAsync())
                    {
                        return;
                    }

                    var Products = SeedDataProvider.GetProducts();
                    var Categories = SeedDataProvider.GetCategories();

                    await context.AddRangeAsync(Products);
                    await context.AddRangeAsync(Categories);

                    await context.SaveChangesAsync();
                })
                .UseSeeding((context, _) =>
                {
                    if (context.Set<Product>().Any())
                    {
                        return;
                    }

                    if (context.Set<Categorie>().Any() )
                    {
                        return;
                    }

                    var Products = SeedDataProvider.GetProducts();
                    var Categories = SeedDataProvider.GetCategories();

                    context.AddRange(Products);
                    context.AddRange(Categories);
                    context.SaveChanges();
                });



            } );




            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

    }
}
