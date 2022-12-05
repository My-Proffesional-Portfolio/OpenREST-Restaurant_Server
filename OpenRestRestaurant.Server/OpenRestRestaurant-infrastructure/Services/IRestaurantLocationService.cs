using OpenRestRestaurant_models.Requests.RestaurantLocation;
using OpenRestRestaurant_models.Responses.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantLocationService
    {
        Task<LocationTablesResponseModel> AddNewLocationToRestaurant(NewRestaurantLocationModel newLocation, string token);
    }
}
