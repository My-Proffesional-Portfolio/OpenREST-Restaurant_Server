using OpenRestRestaurant_models.Requests.Staff;
using OpenRestRestaurant_models.Responses.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IRestaurantStaffService
    {
        Task<NewStaffEmployeeResponseModel> AddUserToRestaurantCompany(NewStaffUserModel newUserStaff, string token);
    }
}
