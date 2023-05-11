using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.entity;
using Microsoft.AspNetCore.Mvc;

namespace erkulSale.webui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this._categoryService=categoryService;
        }
        public IViewComponentResult Invoke()
        {
            if(RouteData.Values["action"].ToString()=="List")
                ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_categoryService.GetAll());
        }
    }
}