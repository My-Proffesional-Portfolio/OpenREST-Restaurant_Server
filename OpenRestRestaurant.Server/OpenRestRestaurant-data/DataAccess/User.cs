using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public Guid Salt { get; set; }

    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<RestaurantStaff> RestaurantStaffs { get; } = new List<RestaurantStaff>();
}
