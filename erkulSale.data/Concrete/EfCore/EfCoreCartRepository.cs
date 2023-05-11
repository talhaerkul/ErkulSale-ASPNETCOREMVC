using System.Linq;
using Microsoft.EntityFrameworkCore;
using erkulSale.data.Abstract;
using erkulSale.entity;

namespace erkulSale.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, SaleContext>, ICartRepository
    {
        public void ClearCart(int cartId)
        {
            using(var context = new SaleContext())
           {
               var cmd = @"delete from CartItems where CartId=@p0";
               context.Database.ExecuteSqlRaw(cmd,cartId);
           }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
           using(var context = new SaleContext())
           {
               var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
               context.Database.ExecuteSqlRaw(cmd,cartId,productId);
           }
        }

        public Cart GetByUserId(string userId)
        {
            using(var context=new SaleContext())
            {
                return context.Carts
                            .Include(i=>i.CartItems)
                            .ThenInclude(i=>i.Product)
                            .FirstOrDefault(i=>i.UserId==userId);
            }
        }

        public override void Update(Cart entity)
        {
            using (var context = new SaleContext())
            {
               context.Carts.Update(entity);
               context.SaveChanges();
            }
        } 
    }
}