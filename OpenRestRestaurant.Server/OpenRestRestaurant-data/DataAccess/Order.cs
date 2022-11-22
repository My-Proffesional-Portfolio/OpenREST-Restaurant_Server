using System;
using System.Collections.Generic;

namespace OpenRestRestaurant_data.DataAccess;

public partial class Order
{
    public Guid Id { get; set; }

    public short OrderType { get; set; }

    public DateTime OpenOrderTime { get; set; }

    public DateTime? CloseOrderTime { get; set; }

    public Guid LocationId { get; set; }

    public Guid StaffId { get; set; }

    public string? OrderNotes { get; set; }

    public short Status { get; set; }

    public long OrderNumber { get; set; }

    public Guid RestaurantCompanyId { get; set; }

    public virtual RestaurantLocation Location { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public virtual ICollection<OrderTable> OrderTables { get; } = new List<OrderTable>();

    public virtual RestaurantCompany RestaurantCompany { get; set; } = null!;

    public virtual RestaurantStaff Staff { get; set; } = null!;
}
