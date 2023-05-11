using System.Collections.Generic;
using erkulSale.entity;

namespace erkulSale.data.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}