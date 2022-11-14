using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_core.Backend.Services;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenRestRestaurant_models.DTOs.Auth;
using Microsoft.OpenApi.Models;

namespace OpenRestRestaurant_api
{
    public static class StartupExtensions
    {

        public static void InjectServices(this IServiceCollection services)
        {
            
            services.AddScoped<ITokenUtilHelper, TokenUtilHelper>();

            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();
            services.AddScoped<IRestaurantCompanyRepository, RestaurantCompanyRepository>();
            services.AddScoped<IRestaurantStaffRepository, RestaurantStaffRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();
            services.AddScoped<IProductMealRepository, ProductMealRepositoy>();

            services.AddScoped<IApiCallerUtil, ApiCallerUtil>();
            services.AddScoped<TransactionManager>();
            services.AddScoped<RestaurantCompanySerivce>();
            services.AddScoped<RestaurantStaffService>();
            services.AddScoped<AccountService>();
            services.AddScoped<UserService>();
            



        }

        public static async Task AddCustomAuthentication(this IServiceCollection services, string authUrl)
        {
            var _authUrl = authUrl;

            /* trying to get the value from injected singleton but at this point, build has´t occurs yet*/
            //https://stackoverflow.com/questions/57100465/how-do-i-run-trigger-service-in-startup-cs
            //var authURServiceInjected = services.Select(s => new
            //{
            //    s.ServiceType,
            //    s.ImplementationFactory
            //}).Where(w => w.ServiceType.Name == "AuthURLValue").FirstOrDefault().ImplementationFactory;

            //if (authURServiceInjected != null)
            //{
            //    AuthURLValue valueObj = (AuthURLValue)authURServiceInjected.Target;
            //    _authUrl = valueObj.UrlValue;
            //}

            var header = new KeyValuePair<string, string>("clientKey", "AB6B-B4C8BFEAD9B2");
            var headers = new List<KeyValuePair<string, string>>();
            headers.Add(header);
            var tokenBearerSecret = await new ApiCallerUtil().CallApiAsync<BearerAuthResponseDTO>(_authUrl, "getBearerAuthInformation", Method.Post, headers: headers);

            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenBearerSecret.privateKey));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "openSalt.auth",
                    ValidAudience = tokenBearerSecret.tokenAudience,
                    IssuerSigningKey = mySecurityKey,
                };
            });

        }

        public static void AddSwaggerGenWithAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}
