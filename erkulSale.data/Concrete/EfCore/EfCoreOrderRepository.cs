using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using erkulSale.data.Abstract;
using erkulSale.entity;

namespace erkulSale.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, SaleContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using(var context = new SaleContext())
            {

                var orders = context.Orders
                                    .Include(i=>i.OrderItems)
                                    .ThenInclude(i=>i.Product)
                                    .AsQueryable();

                if(!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i=>i.UserId ==userId);
                }

                return orders.ToList();
            }
        }
    }
}