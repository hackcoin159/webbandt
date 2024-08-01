using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_ban_dien_thoai.Models
{
    public class MatHangMua
    {
        testEntities database = new testEntities();
        public int MaDT { get; set; }
        public string TenDT{ get; set; }
        public string HinhAnh { get; set; }
        public int GiaCuoi { get; set; }
        public int SoLuong { get; set; }

        public int ThanhTien()
        {
            return SoLuong * GiaCuoi;
        }
        public MatHangMua(int MaDT)
        {
            this.MaDT = MaDT;//Tìm sp trong CSDL có mã id cần và gán cho mặt hàng được mua
            var dt = database.DienThoais.Single(s => s.MaDT == this.MaDT);
            this.TenDT = dt.TenDT;
            this.HinhAnh = dt.HinhChiTiet0;
            this.GiaCuoi = int.Parse(dt.GiaCuoi.ToString());
            this.SoLuong = 1; //Số lượng mua ban đầu của một mặt hàng là 1 cái (cho lần click đầu)
        }
    }
}