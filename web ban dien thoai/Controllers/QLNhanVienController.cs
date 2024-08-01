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
    public class QLNhanVienController : Controller
    {
        // GET: QLNhanVien
        testEntities database = new testEntities();
        public ActionResult QLNVPartial()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            var qlNhanVien = database.NhanViens.ToList();
            return View(qlNhanVien);
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(database.NhanViens, "MaNV", "MaVT");
            return View();
        }
        // POST: QLNhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,MaVT,HoTenNV,DiachiNV,DienthoaiNV,VaiTro,Host,TenDN,Matkhau,Ngaysinh,Gioitinh,Email,Luong")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại trong CSDL chưa
                bool tenDangNhapDaTonTai = database.NhanViens.Any(nv => nv.TenDN == nhanVien.TenDN);
                if (tenDangNhapDaTonTai)
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
                    return View(nhanVien);
                }
                database.NhanViens.Add(nhanVien);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(database.DonHangs, "MaNV", "MaVT", nhanVien.MaNV);
            return View(nhanVien);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NhanVien nhanVien = database.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            return View(nhanVien);
        }

        // POST: QLNhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,MaVT,HoTenNV,DiachiNV,DienthoaiNV,VaiTro,Host,TenDN,Matkhau,Ngaysinh,Gioitinh,Email,Luong")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                database.Entry(nhanVien).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index"); // Chuyển hướng về trang Index sau khi chỉnh sửa thành công
            }

            return View(nhanVien);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nv = database.NhanViens.Find(id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return View(nv);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nv = database.NhanViens.Find(id);
            database.NhanViens.Remove(nv);
            database.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Details(int id)
        {
            var kh = database.KhachHangs.Where(c => c.MaKH == id).FirstOrDefault();
            return View(kh);
        }
    }
}