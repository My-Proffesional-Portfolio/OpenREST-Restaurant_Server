using OpenRestRestaurant_models.Requests.RestaurantLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantLocationService
    {
        Task<object> AddNewLocationToRestaurant(NewRestaurantLocationModel newLocation, string token);
    }
}
