using SEDC.Lamazon.DataAccess.Interfaces;
using SEDC.Lamazon.Domain.Entities;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.Order;
using SEDC.Lamazon.Services.ViewModels.OrderItem;

namespace SEDC.Lamazon.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public void CreateOrder(OrderViewModel order)
    {
        Order newOrder = new Order()
        {
            OrderDate = DateTime.UtcNow,
            IsActive = true,
            OrderNumber = $"{DateTime.UtcNow.ToLongTimeString().ToString()}_{order.UserId}",
            UserId = order.UserId,
        };

        _orderRepository.Insert(newOrder);
    }

    public OrderViewModel GetActiveOrder(int userId)
    {
        Order activeOrder = _orderRepository.GetActiveOrder(userId);

        OrderViewModel activeOrderViewModel = null;

        if (activeOrder != null)
        {
            activeOrderViewModel = new OrderViewModel()
            {
                OrderDate = activeOrder.OrderDate,
                OrderNumber = activeOrder.OrderNumber,
                Id = activeOrder.Id,
                UserId = userId,
                User = new ViewModels.User.UserViewModel()
                {
                    FullName = activeOrder.User.FullName
                },

                Items = activeOrder.Items.Select(o => new OrderItemViewModel() 
                { 
                    Id = o.Id,
                    Price = o.Price,
                    OrderId = o.OrderId,
                    Qty = o.Quantity,
                    Product = new ViewModels.Product.ProductViewModel() 
                    { 
                        Name = o.Product.Name,
                        Description = o.Product.Description,
                        ImageUrl = o.Product.ImageUrl,
                        Price = o.Product.Price,
                        Id = o.ProductId,
                    }
                }).ToList(),

                City = activeOrder.City,
                PhoneNumber = activeOrder.PhoneNumber,
                State = activeOrder.State,
                PostalCode = activeOrder.PostalCode,
                ShippingUserFullName = activeOrder.ShippingUserFullName,
                StreetAddress = activeOrder.StreetAddress,
            };

            activeOrderViewModel.TotalPrice = activeOrderViewModel
                .Items
                .Sum(o => o.Price * o.Qty);
        }

        return activeOrderViewModel;
    }

    public List<OrderViewModel> GetAllOrders(int userId)
    {
        throw new NotImplementedException();
    }

    public OrderViewModel GetOrderById(int id)
    {
        throw new NotImplementedException();
    }

    public OrderViewModel SubmitOrder(OrderViewModel order)
    {
        Order existingActiveOrder = _orderRepository.Get(order.Id);

        if (existingActiveOrder == null)
            throw new Exception($"There is no existing order with provided ID {order.Id}");

        existingActiveOrder.ShippingUserFullName = order.ShippingUserFullName;
        existingActiveOrder.PhoneNumber = order.PhoneNumber;
        existingActiveOrder.StreetAddress = order.StreetAddress;
        existingActiveOrder.City = order.City;
        existingActiveOrder.State = order.State;
        existingActiveOrder.PostalCode = order.PostalCode;
        existingActiveOrder.TotalPrice = order.TotalPrice;

        _orderRepository.Update(existingActiveOrder);

        return order;
    }
}
