using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class RestaurantCompany
{
    public Guid Id { get; set; }

    public long RestaurantNumber { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? FiscalId { get; set; }

    public string? FiscalAddress { get; set; }

    public string LegalOwner { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<RestaurantLocation> RestaurantLocations { get; } = new List<RestaurantLocation>();

    public virtual ICollection<RestaurantStaff> RestaurantStaffs { get; } = new List<RestaurantStaff>();
}
