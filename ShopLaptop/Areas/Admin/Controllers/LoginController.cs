using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopLaptop.Areas.Admin.Common;
using ShopLaptop.Areas.Admin.Models;
using ShopLaptop.Models;
using ShopLaptop.Common;
using ShopLaptop.Models.DBContext;

namespace ShopLaptop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public  ActionResult Index(LoginModel model)
        {
            using (ShopLaptionDbContextDataContext context = new ShopLaptionDbContextDataContext())
            {
                int check = 0;
                if (ModelState.IsValid)
                {
                    var res = context.Users.SingleOrDefault(x => x.UserName.Equals(model.UserName));
                    if (res == null) check = 0;
                    else
                    {
                        if (res.Password == model.Password) check = 1;
                        else check = - 1;
                    }
                    switch (check)
                    {
                        case 0:
                            {
                                ModelState.AddModelError("", "Tài khoản không tồn tại !");
                                break;
                            }
                        case 1:
                            {
                                var user = context.Users.SingleOrDefault(x => x.UserName == model.UserName);
                                var userSession = new UserLogin
                                {
                                    UserName = user.UserName,
                                    Name = user.Name,
                                    UserID = user.ID,
                                    Permission = (int)user.Permission
                                };

                                Session.Add(CommonConstants.USER_SESSION, userSession);
                                Session.Add(CommonConstants.NAME_SESSION, userSession.Name);
                                Session.Add(CommonConstants.IDUSER_SESSION, userSession.UserID);
                                Session.Add(CommonConstants.PERMISSION_SESSION, userSession.Permission);

                                return RedirectToActionPermanent("Index", "Home");
                            }
                        case -1:
                            {
                                ModelState.AddModelError("", "Mật khẩu không đúng !");
                                break;
                            }
                    }
                }
                return View("Index");
            } 
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
    }
}