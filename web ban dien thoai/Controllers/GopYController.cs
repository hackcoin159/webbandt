using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class GopYController : Controller
    {
        // GET: GopY
        testEntities database = new testEntities();

        public ActionResult Index(bool? completedFilter)
        {
            // Retrieve feedback entries based on the completion status and order by NgayDang
            var feedbackEntries = completedFilter.HasValue
                ? database.Gopies.Where(g => g.TrangThai == completedFilter.Value).OrderByDescending(g => g.NgayDang).ToList()
                : database.Gopies.OrderByDescending(g => g.NgayDang).ToList();

            return View(feedbackEntries);
        }


        public ActionResult QLHTPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult LuuGopY(GopYModel model)
        {
            // Lưu dữ liệu vào cơ sở dữ liệu
            using (var db = new testEntities())
            {
                var gopY = new GopY
                {
                    MaGY = model.MaGY,
                    TenKH = model.TenKH,
                    LienLac = model.LienLac,
                    VanDe = model.VanDe,                    
                    NoiDung = model.NoiDung,                    
                    NgayDang = DateTime.Now // Gán ngày đăng là ngày hiện tại
                };

                db.Gopies.Add(gopY);
                db.SaveChanges();
            }

            // Chuyển hướng đến trang cảm ơn hoặc trang khác
            return RedirectToAction("CamOn");
        }







        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GopY dg = database.Gopies.Find(id);
            if (dg == null)
            {
                return HttpNotFound();
            }
            return View(dg);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GopY dg = database.Gopies.Find(id);
            database.Gopies.Remove(dg);
            database.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GopY gopY = database.Gopies.Find(id);
            if (gopY == null)
            {
                return HttpNotFound();
            }
            return View(gopY);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GopY model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin góp ý từ cơ sở dữ liệu
                var gopY = database.Gopies.Find(model.MaGY);
                if (gopY == null)
                {
                    return HttpNotFound();
                }
                // Cập nhật thông tin
                gopY.TenKH = model.TenKH;
                gopY.LienLac = model.LienLac;
                gopY.VanDe = model.VanDe;
                gopY.NoiDung = model.NoiDung;
                // Ghi chú và cập nhật trạng thái
                gopY.GhiChu = model.GhiChu;
                gopY.TrangThai = model.TrangThai;
                // Cập nhật ngày chỉnh sửa
                gopY.NgayDang = DateTime.Now;
                // Lưu thay đổi vào cơ sở dữ liệu
                database.Entry(gopY).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }







    }
}