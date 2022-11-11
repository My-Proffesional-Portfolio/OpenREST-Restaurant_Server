using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class RestaurantLocation
{
    public Guid Id { get; set; }

    public string LocationName { get; set; } = null!;

    public string LocationAddress { get; set; } = null!;

    public Guid ManagerId { get; set; }

    public string? LocationPhone { get; set; }

    public string? LocationEmail { get; set; }

    public bool? IsActive { get; set; }

    public string FiscalId { get; set; } = null!;

    public Guid RestaurantCompanyId { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual RestaurantStaff Manager { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual RestaurantCompany RestaurantCompany { get; set; } = null!;

    public virtual ICollection<RestaurantStaff> RestaurantStaffs { get; } = new List<RestaurantStaff>();

    public virtual ICollection<RestaurantTable> RestaurantTables { get; } = new List<RestaurantTable>();
}
