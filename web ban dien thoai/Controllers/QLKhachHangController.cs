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
    public class QLKhachHangController : Controller
    {
        // GET: QLKhachHang
        testEntities database = new testEntities();

        public ActionResult QLKHPartial()
        {

            return PartialView();
        }
        public ActionResult Index()
        {
            var qlKH = database.KhachHangs.ToList();
            return View(qlKH);
        }

        // GET: QLKhachHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QLKhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTenKH,DiachiKH,DienthoaiKH,TenDN,Matkhau,Ngaysinh,Gioitinh,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại trong CSDL chưa
                bool tenDangNhapDaTonTai = database.KhachHangs.Any(kh => kh.TenDN == khachHang.TenDN);

                if (tenDangNhapDaTonTai)
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
                    return View(khachHang);
                }

                database.KhachHangs.Add(khachHang);
                database.SaveChanges();
                return RedirectToAction("Index"); // Chuyển hướng về trang Index sau khi thêm thành công
            }

            return View(khachHang);
        }

        // GET: QLKhachHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm khách hàng theo MaKH trong CSDL
            KhachHang khachHang = database.KhachHangs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        // POST: QLKhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTenKH,DiachiKH,DienthoaiKH,TenDN,Matkhau,Ngaysinh,Gioitinh,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Lưu thông tin khách hàng đã chỉnh sửa vào CSDL
                database.Entry(khachHang).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // ... Các hàm khác trong Controller ...

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = database.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = database.KhachHangs.Find(id);
            database.KhachHangs.Remove(khachHang);
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var dt = database.NhanViens.Where(c => c.MaNV == id).FirstOrDefault();
            return View(dt);
        }
    }
}