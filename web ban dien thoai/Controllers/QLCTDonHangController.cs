using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class QLCTDonHangController : Controller
    {
        testEntities database = new testEntities();
        // GET: QLCTDonHang       
        public ActionResult Index()
        {
            var qlCTDonHang = database.CTDatHangs.ToList();
            return View(qlCTDonHang);
        }

        public ActionResult QLTKPartial()
        {
            return PartialView();
        }
            
    }
}