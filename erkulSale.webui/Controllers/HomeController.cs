using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using erkulSale.webui.Service;
using erkulSale.webui.Models;
using erkulSale.business.Abstract;

namespace ErkulSale.Controllers;

public class HomeController : Controller
{
    private IProductService _productService;
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(IProductService productService, ILogger<HomeController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public IActionResult Index(string q, int page = 1)
        {
            var pageSize = _productService.pageSize();
            var products = _productService.GetAllApprovedHomeProducts(page,pageSize);
            

            var productListViewModel = new ProductListViewModel(){
                PageInfo = new PageInfo(){
                    TotalItems = _productService.GetAllApprovedHomeProductsCount(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategoryUrl = ""
                },
                Products = products
            };

            return View(productListViewModel);
        }

    public IActionResult Privacy()
    {
        return View();
    }
}
