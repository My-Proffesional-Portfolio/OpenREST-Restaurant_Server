using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class RestaurantStaff
{
    public Guid Id { get; set; }

    public long EmployeeNumber { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string SurName { get; set; } = null!;

    public string? Ssn { get; set; }

    public string? FiscalId { get; set; }

    public Guid UserId { get; set; }

    public Guid RestaurantCompanyId { get; set; }

    public Guid? RestaurantLocationId { get; set; }

    public string? PersonalEmail { get; set; }

    public string? PersonalPhone { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual RestaurantCompany RestaurantCompany { get; set; } = null!;

    public virtual RestaurantLocation? RestaurantLocation { get; set; }

    public virtual ICollection<RestaurantLocation> RestaurantLocations { get; } = new List<RestaurantLocation>();

    public virtual User User { get; set; } = null!;
}
