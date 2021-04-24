using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopLaptop.Models.DBContext;
using ShopLaptop.Models;
using ShopLaptop.Common;
using ShopLaptop.Areas.Admin.Models;
using ShopLaptop.Code;

namespace ShopLaptop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User

        ShopLaptionDbContextDataContext context = null;

        public ActionResult Index()
        {
            context = new ShopLaptionDbContextDataContext();
            List<User> users = context.Users.ToList();
            return View(users);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(User user) 
        {
            context = new ShopLaptionDbContextDataContext();
            if (ModelState.IsValid)
            {
                var check_user = context.Users.Any(a => a.UserName.Equals(user.UserName));
                var check_email = context.Users.Any(a => a.Email.Equals(user.Email));
                var res_val = new MyValidate();

                if (check_user)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại !");
                    return View();
                }
                else if (check_email)
                {
                    ModelState.AddModelError("", "Email đã tồn tại !");
                    return View();
                }
                else if (res_val.isEmail(user.Email)==false)
                {
                    ModelState.AddModelError("", "Email không đúng định dạng !");
                    return View();
                }
                else if (res_val.IsValidPhone(user.Phone) == false)
                {
                    ModelState.AddModelError("", "Số điện thoại không đúng !");
                    return View();
                }
                else
                {

                    user.CreatedBy = Session[CommonConstants.NAME_SESSION].ToString();
                    user.CreatedDate = DateTime.Now;

                    context.Users.InsertOnSubmit(user);
                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            return  RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long ID)
        {
            context = new ShopLaptionDbContextDataContext();
            var user = context.Users.SingleOrDefault(x => x.ID == ID);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User account)
        {
            context = new ShopLaptionDbContextDataContext();
            if (ModelState.IsValid)
            {


                var user = context.Users.SingleOrDefault(x => x.ID == account.ID);

                user.Name = account.Name;
                user.Address = account.Address;
                user.Email = account.Email;
                user.Phone = account.Phone;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = Session[CommonConstants.NAME_SESSION].ToString();
                context.SubmitChanges();

                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult Details(long ID)
        {
            context = new ShopLaptionDbContextDataContext();
            var user = context.Users.SingleOrDefault(x => x.ID == ID);
            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(long ID)
        {
            try
            {
                context = new ShopLaptionDbContextDataContext();

                //if (Session[CommonConstants.PERMISSION_SESSION == 1])
                //{

                //}
                var user = context.Users.FirstOrDefault(x => x.ID == ID);
                context.Users.DeleteOnSubmit(user);
                context.SubmitChanges();

                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                return RedirectToAction("Index");
            }
            
        }
    }
}