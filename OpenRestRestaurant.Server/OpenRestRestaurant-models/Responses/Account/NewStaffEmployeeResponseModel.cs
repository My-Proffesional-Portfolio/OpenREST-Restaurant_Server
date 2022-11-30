using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Responses.Account
{
    public class NewStaffEmployeeResponseModel
    {
        public Guid RestaurantStaffId { get; set; }
        public Guid UserID { get; set; }
    }
}
