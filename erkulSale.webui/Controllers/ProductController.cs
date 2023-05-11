using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.webui.Identity;
using erkulSale.webui.Models;
using erkulSale.webui.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkulSale.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        public ProductController(IProductService productService)                 
        {
            _productService = productService;
        }

        //var q2 = HttpContext.Request.Query["q"].ToString();  bunu unutma çok önemli, filtrelemede global çağırabilirsin seçimi
        public IActionResult List(string category, string q, int page = 1)
        {
            var pageSize = _productService.pageSize();
            var products = _productService.GetAllApprovedProducts(page, pageSize);
            if (category != null)
                products = _productService.GetAllApprovedProductsByCategoryUrl(category, page, pageSize);
            if (!string.IsNullOrEmpty(q))
                products = _productService.GetApprovedProductsByName(q, page, pageSize);

            var productListViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategoryUrl(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategoryUrl = category
                },
                Products = products
            };

            return View(productListViewModel);
        }


        public IActionResult Details(string url)
        {
            if (url is not null)
            {
                var product = _productService.GetProductByUrl(url);
                if (product is not null)
                {
                    return View(product);
                }
                return RedirectToAction("list");
            }
            else
                return RedirectToAction("list");
        }

    }
}