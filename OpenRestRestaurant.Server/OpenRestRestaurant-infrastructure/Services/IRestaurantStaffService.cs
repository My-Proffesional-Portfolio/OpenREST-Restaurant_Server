using OpenRestRestaurant_models.Requests.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantStaffService
    {
        Task<object> AddUserToRestaurantCompany(NewStaffUserModel newUserStaff, string token);
    }
}
