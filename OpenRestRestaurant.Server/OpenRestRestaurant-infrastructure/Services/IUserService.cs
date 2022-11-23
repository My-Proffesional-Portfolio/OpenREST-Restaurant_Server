using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_models.Requests.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IUserService
    {
        Task<User> SaveUser(NewStaffUserModel userModel, bool saveOnCall = false);
    }
}
