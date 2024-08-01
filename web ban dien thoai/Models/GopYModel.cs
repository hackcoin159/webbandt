using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_ban_dien_thoai.Models
{
    // Trong file Models/GopYModel.cs
    public class GopYModel
    {
        public int MaGY { get; set; }
        public string TenKH { get; set; }
        public string VanDe { get; set; }
        public string NoiDung { get; set; }
        public string LienLac { get; set; }
        public DateTime NgayDang { get; set; }
    }

}