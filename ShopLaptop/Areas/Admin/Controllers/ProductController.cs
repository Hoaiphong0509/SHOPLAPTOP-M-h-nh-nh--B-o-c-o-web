using ShopLaptop.Common;
using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/product

        ShopLaptionDbContextDataContext context = null;

        public void SetViewbag(long? selectedID = null)
        {
            var dao = new CategoryDao();
            ViewBag.Categorys = dao.brand();
        }

        public ActionResult Index()
        {
            context = new ShopLaptionDbContextDataContext();
            List<Product> products = context.Products.ToList();
            return View(products);

        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewbag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase image)
        {
            context = new ShopLaptionDbContextDataContext();

            if(image != null && image.ContentLength > 0)
            {
                //product.ImageByte = new byte[image.ContentLength];
                //image.InputStream.Read(product.ImageByte, 0, image.ContentLength);
                string filename = System.IO.Path.GetFileName(image.FileName);
                string urlname = Server.MapPath("~/Assets/Image/Product/General/" + filename);
                image.SaveAs(urlname);

                product.Image = "~/Assets/Image/Product/General/" + filename;
            }
            if (ModelState.IsValid)
            {
                var check_product_code = context.Products.Any(a => a.Code.Equals(product.Code));
                if (check_product_code)
                {
                    ModelState.AddModelError("", "Mã sản phẩm đã tồn tại !");
                    return View();
                }
                else
                {

                    product.CreatedBy = Session[CommonConstants.NAME_SESSION].ToString();
                    product.CreatedDate = DateTime.Now;
                    product.Name.ToUpper();

                    context.Products.InsertOnSubmit(product);
                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            SetViewbag();
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(long ID)
        {
            SetViewbag();

            context = new ShopLaptionDbContextDataContext();
            var product = context.Products.SingleOrDefault(x => x.ID == ID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            SetViewbag();
            context = new ShopLaptionDbContextDataContext();
           
            if (ModelState.IsValid)
            {
                var proc = context.Products.SingleOrDefault(x => x.ID == product.ID);

                if (image != null && image.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(image.FileName);
                    string urlname = Server.MapPath("~/Assets/Image/Product/General/" + filename);
                    image.SaveAs(urlname);

                    proc.Image = "~/Assets/Image/Product/General/" + filename;
                }
                proc.Name = product.Name;
                proc.Code = product.Code;
                proc.Brand = product.Brand;
                proc.Description = product.Description;
                proc.Price = product.Price;
                proc.Quantity = product.Quantity;
                proc.Screen = product.Screen;
                proc.CPU = product.CPU;
                proc.RAM = product.RAM;
                proc.Card = product.Card;
                proc.Disk = product.Disk;
                proc.ModifiedDate = DateTime.Now;
                proc.ModifiedBy = Session[CommonConstants.NAME_SESSION].ToString();

                context.SubmitChanges();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Details(long ID)
        {
            context = new ShopLaptionDbContextDataContext();
            var product = context.Products.SingleOrDefault(x => x.ID == ID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(long ID)
        {
            try
            {
                context = new ShopLaptionDbContextDataContext();

                var product = context.Products.FirstOrDefault(x => x.ID == ID);
                context.Products.DeleteOnSubmit(product);
                context.SubmitChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
    }
}