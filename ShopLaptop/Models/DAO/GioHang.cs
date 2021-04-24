using ShopLaptop.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopLaptop.Models
{
    public class GioHang
    {
        ShopLaptionDbContextDataContext context = new ShopLaptionDbContextDataContext();

        public long _ID { get; set; }
        public string masp { get; set; }
        public string tensp { get; set; }
        public string anhsp { get; set; }
        public double gia { get; set; }
        public int sl { get; set; }
        public double thanhtien 
        { 
            get { return sl * gia; }
        }

        public GioHang(long ID)
        {
            this._ID = ID;
            Product product = context.Products.SingleOrDefault(n => n.ID == this._ID);
            this.masp = product.Code;
            this.tensp = product.Name;
            this.anhsp = product.Image;
            this.gia = double.Parse(product.Price.ToString());
            this.sl = 1;
        }

    }
}