﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Requests.RestaurantLocation
{
    public class NewRestaurantLocationModel
    {
        public NewRestaurantLocationModel()
        {
            this.Tables = new List<LocationTableItemModel>();
        }
        public string LocationAlias { get; set; }
        public string LocationAddress { get; set; }
        public Guid ManagerUserID { get; set; }
        public string FiscalID { get; set; }
        public string LocationPhone { get; set; }
        public string LocationEmail { get; set; }
        public List<LocationTableItemModel> Tables { get; set; }
    }

    public class LocationTableItemModel
    {

    }
}
