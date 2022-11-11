using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Quantity { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public decimal SubtotalLine { get; set; }

    public decimal TaxesLine { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ProductMeal Product { get; set; } = null!;
}
