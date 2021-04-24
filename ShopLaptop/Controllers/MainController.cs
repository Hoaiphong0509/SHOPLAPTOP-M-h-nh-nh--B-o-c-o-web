using ShopLaptop.Common;
using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        ShopLaptionDbContextDataContext context = null;

        public void SetViewbag(long? selectedID = null)
        {
            var dao = new CategoryDao();
            ViewBag.Categorys = dao.brand();

        }
        public void SetRelatedProduct()
        {
            context = new ShopLaptionDbContextDataContext();
            var listRelatedPro = context.Products.ToList();
            ViewBag.RelatedProducts = listRelatedPro;

        }

        public ActionResult Index()
        {
            SetViewbag();
            context = new ShopLaptionDbContextDataContext();
            List<Product> products = context.Products.ToList();
            return View(products);
        }

        public ActionResult Details(long ID)
        {
            SetViewbag();
            SetRelatedProduct();
            context = new ShopLaptionDbContextDataContext();
            var product = context.Products.SingleOrDefault(x => x.ID == ID);
            return View(product);
        }

        public ActionResult FullProduct()
        {
            SetViewbag();
            context = new ShopLaptionDbContextDataContext();
            List<Product> products = context.Products.ToList();
            return View(products);
        }

        public ActionResult ShowByBrand(string brand)
        {
            SetViewbag();
            SetRelatedProduct();
            context = new ShopLaptionDbContextDataContext();
            var brandThisProduct = context.Products.Where(x => x.Brand == brand).ToList();
            return View(brandThisProduct);
        }
    }
}