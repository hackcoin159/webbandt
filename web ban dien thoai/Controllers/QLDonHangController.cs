using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class QLDonHangController : Controller
    {
        // GET: QLDonHang
        // GET: QLDonHang
        testEntities database = new testEntities();
        // GET: QLMatBang
        public ActionResult QLDHPartial()
        {
            return PartialView();
        }

        public ActionResult Index(DateTime? fromDate, DateTime? toDate, string customerName)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            TempData["CustomerName"] = customerName;
            var qlDonHang = database.DonHangs.Include("CTDatHangs").ToList();
            if (fromDate.HasValue)
            {
                qlDonHang = qlDonHang.Where(item => item.NgayDH.HasValue && item.NgayDH.Value.Date >= fromDate.Value.Date).ToList();
            }
            if (toDate.HasValue)
            {
                qlDonHang = qlDonHang.Where(item => item.NgayDH.HasValue && item.NgayDH.Value.Date <= toDate.Value.Date).ToList();
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                qlDonHang = qlDonHang.Where(item => item.TenKhachHang.Contains(customerName)).ToList();
            }
            qlDonHang = qlDonHang.OrderByDescending(item => item.SoDH).ToList();
            return View(qlDonHang);
        }






        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = database.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            // Truyền vào view chỉ các thuộc tính cần chỉnh sửa, không phải đối tượng đầy đủ
            ViewBag.SoDH = donHang.SoDH;
            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int SoDH, bool DaNhan)
        {
            // Tìm đối tượng DonHang đã tồn tại bằng cách dùng SoDH
            DonHang donHang = database.DonHangs.Find(SoDH);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            // Cập nhật chỉ các thuộc tính cho phép           
            donHang.DaNhan = DaNhan;
            // Lưu các thay đổi vào cơ sở dữ liệu
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = database.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = database.DonHangs.Include("CTDatHangs").SingleOrDefault(dh => dh.SoDH == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            database.CTDatHangs.RemoveRange(donHang.CTDatHangs);
            database.DonHangs.Remove(donHang);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}