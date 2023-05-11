using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.entity;
using Microsoft.AspNetCore.Mvc;

namespace erkulSale.webui.ViewComponents
{
    public class CategoriesAdminDeleteViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesAdminDeleteViewComponent(ICategoryService categoryService)
        {
            this._categoryService=categoryService;
        }
        public IViewComponentResult Invoke()
        {
            if(RouteData.Values["action"].ToString()=="DeleteList")
                ViewBag.SelectedCategory = RouteData?.Values["id"];
            return View(_categoryService.GetAll());
        }
    }
}