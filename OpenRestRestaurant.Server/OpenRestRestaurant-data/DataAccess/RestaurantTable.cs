using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class RestaurantTable
{
    public Guid Id { get; set; }

    public string TableNumber { get; set; } = null!;

    public long TableCapacity { get; set; }

    public Guid RestaurantLocationId { get; set; }

    public string? UicoordLocation { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual RestaurantLocation RestaurantLocation { get; set; } = null!;
}
