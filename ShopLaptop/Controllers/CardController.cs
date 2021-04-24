using ShopLaptop.Code;
using ShopLaptop.Common;
using ShopLaptop.Models;
using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        ShopLaptionDbContextDataContext context = null;

        public void SetViewbag(long? selectedID = null)
        {
            var dao = new CategoryDao();
            ViewBag.Categorys = dao.brand();
        }

        public void SetListOrder()
        {
            List<GioHang> gioHangs = GetOrders();
            ViewBag.listOrders = gioHangs;
        }
        public void SetRelatedProduct()
        {
            context = new ShopLaptionDbContextDataContext();
            var listRelatedPro = context.Products.ToList();
            ViewBag.RelatedProducts = listRelatedPro;

        }

        public List<GioHang> GetOrders()
        {
            List<GioHang> listorder = Session["order"] as List<GioHang>;
            if (listorder == null)
            {
                listorder = new List<GioHang>();
                Session["order"] = listorder;
            }
            return listorder;
        }

        private int TongSL()
        {
            int tongsl = 0;
            List<GioHang> listorder = Session["order"] as List<GioHang>;
            if (listorder != null)
            {
                tongsl = listorder.Sum(n => n.sl);
            }
            return tongsl;
        }

        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> listorder = Session["order"] as List<GioHang>;
            if (listorder != null)
            {
                tongtien = listorder.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        public ActionResult ThemGioHang(long ID, string url)
        {
            SetViewbag();
            List<GioHang> listorder = GetOrders();
            GioHang gioHang = listorder.Find(n => n._ID == ID);
            if (gioHang == null)
            {
                gioHang = new GioHang(ID);
                listorder.Add(gioHang);
                return Redirect(url);
            }
            else
            {
                gioHang.sl++;
                return Redirect(url);
            }
        }

        public ActionResult XoaGioHang(long ID, string url)
        {
            SetViewbag();
            List<GioHang> listorder = GetOrders();
            GioHang gioHang = listorder.Find(n => n._ID == ID);
            if (gioHang != null)
            {
                listorder.RemoveAll(n => n._ID == ID);
                return RedirectToAction("Index", "Card");
            }
            if (listorder.Count == 0)
            {
                return RedirectToAction("Index", "Main");
            }
            return RedirectToAction("Index", "Card");
        }

        public ActionResult XoaTatCa()
        {
            SetViewbag();
            List<GioHang> listorder = GetOrders();
            listorder.Clear();
            return RedirectToAction("Index", "Card");
        }

        public ActionResult CapNhatGioHang(long ID, FormCollection collection)
        {
            SetViewbag();
            List<GioHang> listorder = GetOrders();
            GioHang gioHang = listorder.Find(n => n._ID == ID);
            if (gioHang != null)
            {
                gioHang.sl = int.Parse(collection["txtSoluong"].ToString());
            }
            return RedirectToAction("Index", "Card");
        }

        public ActionResult Index()
        {
            SetViewbag();
            List<GioHang> listorder = GetOrders();
            if (listorder.Count == 0)
            {
                return RedirectToAction("Index", "Main");
            }
            ViewBag.TongSoluong = TongSL();
            ViewBag.TongTien = TongTien();
            return View(listorder);
        }

        public ActionResult GioHangPartical()
        {
            SetViewbag();
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            SetViewbag();
            SetListOrder();
            ViewBag.TongTien = TongTien();
            return View();
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            context = new ShopLaptionDbContextDataContext();
            SetViewbag();
            SetListOrder();
            ViewBag.TongTien = TongTien();

            var res = new MyValidate();
            Order order = new Order();
            List<GioHang> gioHangs = GetOrders();

            order.MaDH = "HD" + DateTime.Now.ToString("yyyyMMddHHmmsss");
            order.TenKH = collection["tenKH"];
            order.Sdt = collection["sdt"];
            order.DiaChi = collection["diachi"];
            order.NgayDat = DateTime.Now;
            order.GhiChu = collection["ghichu"];
            order.TinhTrang = false;

            //bool v = res.IsValidPhone(order.Sdt);

            if (String.IsNullOrEmpty(order.TenKH)) ViewData["err1"] = "Vui lòng nhập họ tên của bạn !";
            else if (String.IsNullOrEmpty(order.Sdt)) ViewData["err2"] = "Vui lòng nhập số điện thoại liên lạc !";
            else if (res.IsValidPhone(order.Sdt) == false) ViewData["err3"] = "Số điện thoại bạn nhập không đúng định dạng!";
            else if (String.IsNullOrEmpty(order.DiaChi)) ViewData["err4"] = "Vui lòng nhập địa chỉ giao hàng !";
            else
            {
                context.Orders.InsertOnSubmit(order);
                context.SubmitChanges();

                foreach (var item in gioHangs)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.MaDH = order.MaDH;
                    orderDetail.MaSP = item.masp;
                    orderDetail.TenSP = item.tensp;
                    orderDetail.Soluong = item.sl;
                    orderDetail.DonGia = (decimal)item.gia;
                    orderDetail.Tong = (decimal)TongTien();

                    var pro = context.Products.SingleOrDefault(x => x.Code == orderDetail.MaSP);
                    pro.Quantity = pro.Quantity - orderDetail.Soluong;

                    context.OrderDetails.InsertOnSubmit(orderDetail);
                    context.SubmitChanges();
                }

                context.SubmitChanges();
                Session["order"] = null;
                return RedirectToAction("XacNhanDonHang", "Card");
            }
            return View();
        }


        public ActionResult XacNhanDonHang()
        {
            SetViewbag();
            return View();
        }
    }
}