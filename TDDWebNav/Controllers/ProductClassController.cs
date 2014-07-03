using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDDWebNav.Controllers
{
    public class ProductClassController : Controller
    {
        [MvcSiteMapNode(Title = "Product Classes", Key = "ProductClasses", ParentKey = "Home")]
        public ActionResult Index()
        {
            Models.ProductClasses data = new Models.ProductClasses();

            return View(data.GetData());
        }
    }
}
