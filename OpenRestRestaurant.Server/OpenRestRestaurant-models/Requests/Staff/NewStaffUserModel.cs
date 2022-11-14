using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Requests.Staff
{
    public class NewStaffUserModel
    {
        public string Name { get; set; }
        public string LastName { get; set; } 
        public string SurName { get; set; }
        public string Ssn { get; set; }
        public string FiscalId { get; set; }
        public string PersonalEmail { get; set; }
        public string PersonalPhone { get; set; }
        public string Address { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NewEmployeeType { get; set; }
        public Guid? LocationId { get; set; }
    }
}
