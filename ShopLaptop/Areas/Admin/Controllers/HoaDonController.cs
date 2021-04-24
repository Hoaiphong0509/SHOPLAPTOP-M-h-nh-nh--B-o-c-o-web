using ShopLaptop.Models.DBContext;
using ShopLaptop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Areas.Admin.Controllers
{
    public class HoaDonController : BaseController
    {
        // GET: Admin/HoaDon

        ShopLaptionDbContextDataContext context = null;

        public ActionResult Index()
        {
            context = new ShopLaptionDbContextDataContext();
            var hd = context.Orders.Where(x => x.TinhTrang == false).ToList();
            return View(hd);
        }

        public ActionResult HoaDonDaXuLy()
        {
            context = new ShopLaptionDbContextDataContext();
            var hd = context.Orders.Where(x => x.TinhTrang == true).ToList();
            return View(hd);
        }

        public ActionResult XacNhanDonHang(string MaHD)
        {
            context = new ShopLaptionDbContextDataContext();

            var dh = context.Orders.SingleOrDefault(x => x.MaDH.Equals(MaHD));
            dh.TinhTrang = true;

            context.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string MaHD)
        {
            context = new ShopLaptionDbContextDataContext();
            var hd = context.Orders.SingleOrDefault(x => x.MaDH.Equals(MaHD));
            return View(hd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            context = new ShopLaptionDbContextDataContext();
            try
            {
                if (ModelState.IsValid)
                {
                    var hd = context.Orders.SingleOrDefault(x => x.MaDH.Equals(order.MaDH));

                    hd.DiaChi = order.DiaChi;
                    hd.GhiChu = order.GhiChu;

                    context.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Chỉnh sửa thất bại !");
                return View();
            }
        }

        public ActionResult Details(string MaHD)
        {
            context = new ShopLaptionDbContextDataContext();
            List<OrderDetail> ctHD = context.OrderDetails.Where(x => x.MaDH.Equals(MaHD)).ToList();

            var temp = context.OrderDetails.FirstOrDefault(x => x.MaDH.Equals(MaHD));
            var dh = context.Orders.SingleOrDefault(x => x.MaDH == MaHD);


            ViewBag.CTDonhang = dh;

            ViewBag.TongTien = temp.Tong; // Tổng này là tổng của 1 món hàng hóa thôi mà.
            ViewBag.MaHoaDon = temp.MaDH;


            return View(ctHD);
        }

        [HttpPost]
        public ActionResult Delete(string MaHD)
        {
            try
            {
                context = new ShopLaptionDbContextDataContext();

                var hd = context.Orders.FirstOrDefault(x => x.MaDH == MaHD);
                var cthd = context.OrderDetails.FirstOrDefault(x => x.MaDH == MaHD);

                context.Orders.DeleteOnSubmit(hd);
                context.SubmitChanges();

                context.OrderDetails.DeleteOnSubmit(cthd);
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