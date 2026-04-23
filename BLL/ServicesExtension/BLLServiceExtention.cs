using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class BLLServiceExtention
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(BLLServiceExtention).Assembly);
            services.AddScoped<IProductMapper, ProductMapper>();
            services.AddScoped<IProductManager, ProductManager>();


            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<ICategoryMapper, CategoryMapper>();


            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ICartManager, CartManager>();

            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IErrorMapper,  ErrorMapper>();
        }
    }
}
