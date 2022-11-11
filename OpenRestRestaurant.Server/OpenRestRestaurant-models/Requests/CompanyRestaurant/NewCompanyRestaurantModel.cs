using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Requests.CompanyRestaurant
{
    public class NewCompanyRestaurantModel
    {
        public long RestaurantNumber { get; set; }
        public string CompanyName { get; set; } 
        public string LegalOwner { get; set; }
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public string Email { get; set; }
        public string Name { get; set; } 
        public string LastName { get; set; } 
        public string SurName { get; set; }
        public string Ssn { get; set; }
        public string PersonalEmail { get; set; }
        public string PersonalPhone { get; set; }
        public string FiscalId { get; set; }
        public string FiscalAddress { get; set; }

    }
}
