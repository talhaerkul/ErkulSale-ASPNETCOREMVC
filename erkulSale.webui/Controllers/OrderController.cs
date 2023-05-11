using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.webui.Extensions;
using erkulSale.webui.Identity;
using erkulSale.webui.Models;
using erkulSale.webui.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkulSale.Controllers
{
    public class OrderController : Controller
    {
        private UserManager<User> _userManager;
        private IOrderService _orderService;

        public OrderController(UserManager<User> userManager,IOrderService orderService)
        {
            this._userManager = userManager;
            this._orderService = orderService;

        }
        public IActionResult Index()
        {
            var orders = _orderService.GetOrders(_userManager.GetUserId(User));
            var OrderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            if (orders.Count == 0)
            {
                TempData.Put("message", new AlertMessage("Uyarı Mesajı", $"Siparişiniz Bulunmamaktadır.", "warning"));
            }
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentType = order.PaymentType;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImgUrl
                }).ToList();
                OrderListModel.Add(orderModel);
            }
            return View(OrderListModel);
        }
    }
}