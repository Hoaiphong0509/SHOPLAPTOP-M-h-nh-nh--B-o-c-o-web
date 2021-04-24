using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopLaptop.Areas.Admin.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int Permission { get; set; }
    }
}