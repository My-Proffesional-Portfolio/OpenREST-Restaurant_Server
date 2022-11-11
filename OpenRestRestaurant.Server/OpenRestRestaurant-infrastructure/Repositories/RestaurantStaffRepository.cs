using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_infrastructure.Repositories
{
    public class RestaurantStaffRepository : BaseRepository<RestaurantStaff>, IRestaurantStaffRepository
    {
        public RestaurantStaffRepository(OpenRestRestaurantDbContext dbContext) : base(dbContext)
        {
        }
    }
}
