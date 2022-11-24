using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Responses.CompanyRestaurants
{
    public class NewCompanyRestaurantResponseModel
    {
        public Guid RestaurantID { get; set; }
        public long RestaurantNumber { get; set; }
        public string CompanyName { get; set; }
    }
}
