using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Responses.Account
{
    public class UserItemModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Guid UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
