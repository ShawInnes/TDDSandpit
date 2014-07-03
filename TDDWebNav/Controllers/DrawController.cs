using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDDWebNav.Controllers
{
    [RoutePrefix("draw")]
    public class DrawController : Controller
    {
        // GET: Product
        [MvcSiteMapNode(Title = "Draws", Key = "Draws", ParentKey = "Products", PreservedRouteParameters = "productClassId,productId")]
        [SiteMapTitle("Name", Target = AttributeTarget.ParentNode)]
        [Route("{productId}")]
        public ActionResult Index(int productId)
        {
            Models.Product model = new Models.Products().GetData().FirstOrDefault(p => p.Id == productId);

            var node = SiteMaps.Current.FindSiteMapNodeFromKey("ProductClasses");
            if (node != null)
            {
                Models.Product product = new Models.Products().GetData().FirstOrDefault(p => p.Id == productId);
                Models.ProductClass productClass = new Models.ProductClasses().GetData().FirstOrDefault(p => p.Id == product.ProductClassId);

                node.Title = productClass.Name;
            }

            return View(model);
        }

        [MvcSiteMapNode(Title = "Draw Details", Key = "DrawDetails", ParentKey = "Draws", PreservedRouteParameters = "productClassId,productId,drawNo")]
        [SiteMapTitle("FormattedDrawNo", Target = AttributeTarget.ParentNode)]
        [Route("details/{productId}/{drawNo}")]
        public ActionResult Details(int productId, int drawNo)
        {
            Models.Draw model = new Models.Draw();
            model.ProductId = productId;
            model.DrawNo = drawNo;

            var productClassNode = SiteMaps.Current.FindSiteMapNodeFromKey("ProductClasses");
            if (productClassNode != null)
            {
                Models.Product product = new Models.Products().GetData().FirstOrDefault(p => p.Id == productId);
                Models.ProductClass productClass = new Models.ProductClasses().GetData().FirstOrDefault(p => p.Id == product.ProductClassId);

                productClassNode.Title = productClass.Name;
            }

            var productNode = SiteMaps.Current.FindSiteMapNodeFromKey("Products");
            if (productNode != null)
            {
                Models.Product product = new Models.Products().GetData().FirstOrDefault(p => p.Id == productId);

                productNode.Title = product.Name;
                productNode.Attributes["productId"] = productId;
            }

            return View(model);
        }
    }
}
