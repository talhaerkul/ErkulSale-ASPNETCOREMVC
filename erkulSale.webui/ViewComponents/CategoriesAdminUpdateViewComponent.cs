using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.business.Abstract;
using erkulSale.entity;
using Microsoft.AspNetCore.Mvc;

namespace erkulSale.webui.ViewComponents
{
    public class CategoriesAdminUpdateViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesAdminUpdateViewComponent(ICategoryService categoryService)
        {
            this._categoryService=categoryService;
        }
        public IViewComponentResult Invoke()
        {
            if(RouteData.Values["action"].ToString()=="Update")
                ViewBag.SelectedCategory = RouteData?.Values["id"];
            return View(_categoryService.GetAll());
        }
    }
}