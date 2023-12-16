using SEDC.Lamazon.Services.ViewModels.OrderItem;
using SEDC.Lamazon.Services.ViewModels.User;

namespace SEDC.Lamazon.Services.ViewModels.Order;

public class OrderViewModel
{
    public int Id { get; set; }

    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public UserViewModel User { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderItemViewModel> Items { get; set; }

    // Shipping details
    public string ShippingUserFullName { get; set; }
    public string PhoneNumber { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public DateTime EstimatedShipingDate { get; set; }
}
