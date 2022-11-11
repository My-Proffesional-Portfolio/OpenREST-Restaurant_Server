using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class ProductMeal
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Cost { get; set; }

    public string Description { get; set; } = null!;

    public Guid RestaurantCompanyId { get; set; }

    public bool NeedTargetInventory { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Taxes { get; set; }

    public decimal? OtherTaxes { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
}
