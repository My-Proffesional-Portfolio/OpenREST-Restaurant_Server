using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantCompanyService
    {
        Task<object> AddRestaurantCompany(NewCompanyRestaurantModel newRestaurant);
        Guid GetRestaurantIdFromToken(string bearerToken);
        int GetEmployeeTypeFromToken(string bearerToken);
    }
}
