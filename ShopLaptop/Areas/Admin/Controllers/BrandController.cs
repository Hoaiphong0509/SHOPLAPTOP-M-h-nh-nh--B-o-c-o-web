using ShopLaptop.Common;
using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        ShopLaptionDbContextDataContext context = null;

        public ActionResult Index()
        {
            context = new ShopLaptionDbContextDataContext();
            List<Category> categories = context.Categories.ToList();
            return View(categories);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Category category)
        {
            context = new ShopLaptionDbContextDataContext();
            if (ModelState.IsValid)
            {
                var check_name = context.Categories.Any(a => a.Name.Equals(category.Name));
                if (check_name)
                {
                    ModelState.AddModelError("", "Thương hiệu này đã tồn tại !");
                    return View();
                }
                else
                {

                    category.CreatedBy = Session[CommonConstants.NAME_SESSION].ToString();
                    category.CreatedDate = DateTime.Now;
                    category.Name.ToUpper();

                    context.Categories.InsertOnSubmit(category);
                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long ID)
        {
            try
            {
                context = new ShopLaptionDbContextDataContext();
                var category = context.Categories.FirstOrDefault(x => x.ID == ID);
                context.Categories.DeleteOnSubmit(category);
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