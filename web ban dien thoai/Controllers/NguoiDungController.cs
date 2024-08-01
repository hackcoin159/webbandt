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
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        //menu-icon
        public ActionResult UserPartial()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            return View();
        }


        testEntities database = new testEntities();


        //Phần đăng nhập//
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        //Phần đăng nhập//
        [HttpPost]
        public ActionResult DangNhap(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tìm khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var khach = database.KhachHangs.FirstOrDefault(k => k.TenDN ==
                    kh.TenDN && k.Matkhau == kh.Matkhau);
                    if (khach != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        //Lưu vào session
                        Session["TaiKhoan"] = khach;

                        return RedirectToAction("Index", "TrangChu");
                    }
                    if (kh.TenDN == "/login" || kh.TenDN == "/admin")
                    {
                        return RedirectToAction("DangNhap", "Admin");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        //-----------------------------------------------------------------------------------------------
        // log out
        public ActionResult DangXuat()
        {
            Session.Clear();//remove session
            return RedirectToAction("DangNhap");
        }
        //---------------------------------------------------------------------------------------------
        //dang ky
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTenKH))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");
                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var khachhang = database.KhachHangs.FirstOrDefault(k => k.TenDN ==
                kh.TenDN);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
                if (ModelState.IsValid)
                {
                    database.KhachHangs.Add(kh);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }

        //-------------
        // GET: QLKhachHang/Edit/5
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
        public ActionResult EditInfo([Bind(Include = "HoTenKH,DiachiKH,DienthoaiKH,Ngaysinh,Gioitinh,Email")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin khách hàng từ CSDL dựa trên MaKH
                var existingKhachHang = database.KhachHangs.FirstOrDefault(kh => kh.MaKH == khachHang.MaKH);

                if (existingKhachHang != null)
                {
                    // Giữ nguyên TenDN và Matkhau
                    khachHang.MaKH = existingKhachHang.MaKH;
                    khachHang.TenDN = existingKhachHang.TenDN;
                    khachHang.Matkhau = existingKhachHang.Matkhau;
                    

                    // Lưu thông tin khách hàng đã chỉnh sửa vào CSDL
                    database.Entry(existingKhachHang).CurrentValues.SetValues(khachHang);
                    database.SaveChanges();
                    return RedirectToAction("Index", "TrangChu");

                }

            }

            return View(khachHang);
        }


        // ... Các hàm khác trong Controller ...
        public ActionResult EditPass(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPass(int MaKH, string MatKhau, string XacNhanMatKhauCu, string XacNhanMatKhauMoi)
        {
            if (ModelState.IsValid)
            {
                // Lấy khách hàng từ cơ sở dữ liệu bằng MaKH
                var existKhachHang = database.KhachHangs.FirstOrDefault(kh => kh.MaKH == MaKH);

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

                            return RedirectToAction("Index");
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


    }
}