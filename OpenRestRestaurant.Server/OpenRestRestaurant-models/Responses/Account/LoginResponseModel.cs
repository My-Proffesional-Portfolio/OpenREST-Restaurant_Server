using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Responses.Account
{
    public class LoginResponseModel
    {
        public string token { get; set; }
        public string userName { get; set; }
        public string staffPersonal { get; set; }
        public Guid restaurantID { get; set; }
        public int employeeType { get; set; }
        public Guid userID { get; set; }
    }
}
