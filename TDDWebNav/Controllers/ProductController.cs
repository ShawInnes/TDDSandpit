using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDDWebNav.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        // GET: Product
        [MvcSiteMapNode(Title = "Products", Key = "Products", ParentKey = "ProductClasses", PreservedRouteParameters = "productClassId,productId")]
        [SiteMapTitle("Name", Target = AttributeTarget.ParentNode)]
        [Route("{productClassId}")]
        public ActionResult Index(int productClassId)
        {
            Models.ProductClass model = new Models.ProductClasses().GetData().FirstOrDefault(p => p.Id == productClassId);
            model.Products = new Models.Products().GetData().Where(p => p.ProductClassId == productClassId).ToList();

            return View(model);
        }
    }
}