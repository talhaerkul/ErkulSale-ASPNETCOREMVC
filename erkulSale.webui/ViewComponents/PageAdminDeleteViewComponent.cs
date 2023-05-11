using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.entity;
using erkulSale.webui.Models;
using Microsoft.AspNetCore.Mvc;

namespace erkulSale.webui.ViewComponents
{
    public class PageAdminDeleteViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PageInfo pageInfo)
        {
            
            return View(pageInfo);
        }
    }
}