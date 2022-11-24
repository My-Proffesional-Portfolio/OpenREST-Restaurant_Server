using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models.Responses.CompanyRestaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantCompanyService
    {
        Task<NewCompanyRestaurantResponseModel> AddRestaurantCompany(NewCompanyRestaurantModel newRestaurant);
        Guid GetRestaurantIdFromToken(string bearerToken);
        int GetEmployeeTypeFromToken(string bearerToken);
    }
}
