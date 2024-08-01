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
    public class AdminController : Controller
    {
        // GET: Admin

        testEntities database = new testEntities();


        public ActionResult ThongTinAdmin()
        {
            if (Session["TaiKhoanAD"] == null) //Chưa đăng nhập
                return RedirectToAction("DangNhap", "Admin");
            return View();
        }
        public ActionResult LogOutPartial()
        {
            return PartialView();
        }







        public ActionResult TrangQuanLy()
        {
            return View();
        }
        //Phần đăng nhập//
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        //Phần đăng nhập//
        [HttpPost]
        public ActionResult DangNhap(NhanVien ad)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(ad.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(ad.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tìm khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var admin = database.NhanViens.FirstOrDefault(k => k.TenDN ==
                     ad.TenDN && k.Matkhau == ad.Matkhau);
                    if (admin != null)
                    {
                        ViewBag.ThongBao = "Đăng nhập thành công";
                        //Lưu vào session
                        Session["TaiKhoanAD"] = admin;

                        return RedirectToAction("TrangQuanLy", "Admin");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();//remove session
            return RedirectToAction("DangNhap");
        }






        // ... Các hàm khác trong Controller ...
        public ActionResult EditPass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm khách hàng theo MaKH trong CSDL
            NhanVien nhanVien = database.NhanViens.Find(id);

            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            return View(nhanVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPass(int MaNV, string MatKhau, string XacNhanMatKhauCu, string XacNhanMatKhauMoi)
        {
            if (ModelState.IsValid)
            {
                // Lấy khách hàng từ cơ sở dữ liệu bằng MaKH
                var existKhachHang = database.NhanViens.FirstOrDefault(kh => kh.MaNV == MaNV);

                if (existKhachHang != null)
                {
                    // Kiểm tra mật khẩu cũ có khớp với mật khẩu trong cơ sở dữ liệu
                    if (XacNhanMatKhauCu == existKhachHang.Matkhau)
                    {
                        // Kiểm tra xác nhận mật khẩu mới
                        if (MatKhau == XacNhanMatKhauMoi)
                        {
                            // Cập nhật mật khẩu mới
                            existKhachHang.Matkhau = MatKhau;

                            // Lưu thông tin khách hàng đã chỉnh sửa vào CSDL
                            database.Entry(existKhachHang).State = EntityState.Modified;
                            database.SaveChanges();

                            return RedirectToAction("TrangQuanLy");
                        }
                        else
                        {
                            ModelState.AddModelError("XacNhanMatKhauMoi", "Xác nhận mật khẩu mới không khớp.");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("XacNhanMatKhauCu", "Mật khẩu cũ không đúng.");
                    }
                }
            }

            return View();
        }





        public ActionResult EditInfo(int? id)
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
        public ActionResult EditInfo([Bind(Include = "HoTenNV,DiachiNV,DienthoaiNV,Ngaysinh,Gioitinh,Email")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin khách hàng từ CSDL dựa trên MaKH
                var existingKhachHang = database.NhanViens.FirstOrDefault(kh => kh.MaNV == nhanVien.MaNV);

                if (existingKhachHang != null)
                {
                    // Giữ nguyên TenDN và Matkhau
                    nhanVien.Luong = existingKhachHang.Luong;
                    nhanVien.VaiTro = existingKhachHang.VaiTro;
                    nhanVien.MaNV = existingKhachHang.MaNV;
                    nhanVien.TenDN = existingKhachHang.TenDN;
                    nhanVien.Matkhau = existingKhachHang.Matkhau;


                    // Lưu thông tin khách hàng đã chỉnh sửa vào CSDL
                    database.Entry(existingKhachHang).CurrentValues.SetValues(nhanVien);
                    database.SaveChanges();
                    return RedirectToAction("TrangQuanLy", "Admin");

                }

            }

            return View(nhanVien);
        }

















    }
}