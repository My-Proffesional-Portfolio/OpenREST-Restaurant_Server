using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private OpenRestRestaurantDbContext _context;
        public UserRepository(OpenRestRestaurantDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
