using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Requests.CompanyRestaurant
{
    public class NewCompanyRestaurantModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Restaurant name missing")]
        [StringLength(100, ErrorMessage = "The minimum lenght is 2 characters", MinimumLength = 2)]
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Must be a legal owmner for restaurant")]
        public string LegalOwner { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The nickname for user is required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The password for user is required")]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string SurName { get; set; }

        public string Ssn { get; set; }
        public string PersonalEmail { get; set; }
        public string PersonalPhone { get; set; }
        public string FiscalId { get; set; }
        public string FiscalAddress { get; set; }

    }
}
