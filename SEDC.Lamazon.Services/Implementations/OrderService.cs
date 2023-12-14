using SEDC.Lamazon.DataAccess.Interfaces;
using SEDC.Lamazon.Domain.Entities;
using SEDC.Lamazon.Services.Interfaces;
using SEDC.Lamazon.Services.ViewModels.Order;

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
                TotalPrice = activeOrder.TotalPrice,
                User = new ViewModels.User.UserViewModel()
                {
                    FullName = activeOrder.User.FullName
                }
            };
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
}
