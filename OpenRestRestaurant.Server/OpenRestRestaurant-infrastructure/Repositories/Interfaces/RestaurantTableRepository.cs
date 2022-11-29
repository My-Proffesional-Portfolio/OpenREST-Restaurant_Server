using OpenRestRestaurant_data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_infrastructure.Repositories.Interfaces
{
    public class RestaurantTableRepository : BaseRepository<RestaurantTable>, IRestaurantTableRepository
    {
        public RestaurantTableRepository(OpenRestRestaurantDbContext dbContext) : base (dbContext)
        {

        }
    }
}
