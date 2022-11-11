using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_core.Backend.Services;

namespace OpenRestRestaurant_api
{
    public static class StartupExtensions
    {

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();
            services.AddScoped<IRestaurantCompanyRepository, RestaurantCompanyRepository>();
            services.AddScoped<IRestaurantStaffRepository, RestaurantStaffRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();
            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();

            services.AddScoped<IApiCallerUtil, ApiCallerUtil>();
            services.AddScoped<TransactionManager>();
            services.AddScoped<RestaurantCompanySerivce>();
        }
    }
}
