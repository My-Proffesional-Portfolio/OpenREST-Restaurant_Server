using OpenRestRestaurant_models.Responses.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<object> GetUsersList(Guid companyRestaurantID, int page, int itemsPerPage, string searchTerm = null);
        Task<LoginResponseModel> Login(string userName, string password);
    }
}
