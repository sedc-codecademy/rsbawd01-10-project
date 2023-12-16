﻿namespace SEDC.Lamazon.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public bool IsActive { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual IEnumerable<OrderItem> Items { get; set;}

    // Shipping details
    public string ShippingUserFullName { get; set; }
    public string PhoneNumber { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public DateTime EstimatedShipingDate { get; set; }
}
