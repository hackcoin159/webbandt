using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        public ActionResult Index()
        {
            var dsTinTuc = ShowDTmoi(4);
            return View(dsTinTuc);
        }
        testEntities database = new testEntities();
        private List<DienThoai> ShowDTmoi(int SoLuong)
        {

            return database.DienThoais.OrderByDescending(DienThoai => DienThoai.Ngaycapnhat).Where(DienThoai => DienThoai.MaPL == 1).Take(SoLuong).ToList();

        }
        public ActionResult Map()
        {
            return View();
        }
    }
}