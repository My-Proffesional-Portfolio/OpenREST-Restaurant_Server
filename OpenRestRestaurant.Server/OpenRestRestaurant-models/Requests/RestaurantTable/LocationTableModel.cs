using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Requests.RestaurantTable
{
    public class LocationTableModel
    {
        public string TableNumber { get; set; }
        public int TableCapacity { get; set; }
        public Guid? RestaurantLocationId { get; set; }
    }
}
