using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class OrderTable
{
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public Guid OrderId { get; set; }

    public string? Notes { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual RestaurantTable Table { get; set; } = null!;
}
