using System.Collections.Generic;
using erkulSale.entity;

namespace erkulSale.business.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
    }
}