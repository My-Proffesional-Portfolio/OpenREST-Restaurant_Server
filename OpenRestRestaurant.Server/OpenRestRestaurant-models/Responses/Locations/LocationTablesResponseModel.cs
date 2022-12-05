using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Responses.Locations
{
    public class LocationTablesResponseModel
    {
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
        public int TablesCount { get; set; }
    }
}
