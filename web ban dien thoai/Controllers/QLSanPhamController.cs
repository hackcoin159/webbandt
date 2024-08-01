using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class QLSanPhamController : Controller
    {
        testEntities database = new testEntities();
        // GET: QLSanPham

        public ActionResult Index()
        {
            var qlSanPham = database.DienThoais.ToList();
            return View(qlSanPham);
        }

   

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaDC = new SelectList(database.PhanLoais, "MaPL", "TenPhanLoai");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDT,MaPL,TenDT,GiaGoc,Discount,GiaCuoi,ManHinh,LoaiManHinh,Chip,Ram,Rom,Cam,CamSau,Pin,Ngaycapnhat,Soluongban,soluotxem,Hinhchitiet0, HinhChiTiet1, HinhChiTiet2, HinhChiTiet3, HinhChiTiet4, HinhChiTiet5")]
            DienThoai dienThoai, 
            HttpPostedFileBase Hinhchitiet0, 
            HttpPostedFileBase HinhChiTiet1, 
            HttpPostedFileBase HinhChiTiet2, 
            HttpPostedFileBase HinhChiTiet3, 
            HttpPostedFileBase HinhChiTiet4, 
            HttpPostedFileBase HinhChiTiet5)
        {
            if (ModelState.IsValid)
            {
                if (Hinhchitiet0 != null && Hinhchitiet0.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Hinhchitiet0.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    Hinhchitiet0.SaveAs(path);
                    dienThoai.HinhChiTiet0 = "~/images/san-pham/" + fileName;
                }
                if (HinhChiTiet1 != null && HinhChiTiet1.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhChiTiet1.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    HinhChiTiet1.SaveAs(path);
                    dienThoai.HinhChiTiet1 = "~/images/san-pham/" + fileName;
                }
                else
                {
                    // Set HinhChiTiet5 to null or some default value
                    dienThoai.HinhChiTiet1 = null; // or dienThoai.HinhChiTiet5 = "~/images/default-image.jpg"; for example
                }
                // Tương tự cho HinhChiTiet2, HinhChiTiet3, HinhChiTiet4, HinhChiTiet5
                if (HinhChiTiet2 != null && HinhChiTiet2.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhChiTiet2.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    HinhChiTiet2.SaveAs(path);
                    dienThoai.HinhChiTiet2 = "~/images/san-pham/" + fileName;
                }
                else
                {
                    // Set HinhChiTiet5 to null or some default value
                    dienThoai.HinhChiTiet2 = null; // or dienThoai.HinhChiTiet5 = "~/images/default-image.jpg"; for example
                }
                if (HinhChiTiet3 != null && HinhChiTiet3.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhChiTiet3.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    HinhChiTiet3.SaveAs(path);
                    dienThoai.HinhChiTiet3 = "~/images/san-pham/" + fileName;
                }
                else
                {
                    // Set HinhChiTiet5 to null or some default value
                    dienThoai.HinhChiTiet3 = null; // or dienThoai.HinhChiTiet5 = "~/images/default-image.jpg"; for example
                }
                if (HinhChiTiet4 != null && HinhChiTiet4.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhChiTiet4.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    HinhChiTiet4.SaveAs(path);
                    dienThoai.HinhChiTiet4 = "~/images/san-pham/" + fileName;
                }
                else
                {
                    // Set HinhChiTiet5 to null or some default value
                    dienThoai.HinhChiTiet4 = null; // or dienThoai.HinhChiTiet5 = "~/images/default-image.jpg"; for example
                }
                if (HinhChiTiet5 != null && HinhChiTiet5.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhChiTiet5.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    HinhChiTiet5.SaveAs(path);
                    dienThoai.HinhChiTiet5 = "~/images/san-pham/" + fileName;
                }
                else
                {
                    // Set HinhChiTiet5 to null or some default value
                    dienThoai.HinhChiTiet5 = null; // or dienThoai.HinhChiTiet5 = "~/images/default-image.jpg"; for example
                }
                database.DienThoais.Add(dienThoai);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPL = new SelectList(database.PhanLoais, "MaPL", "TenPhanLoai", dienThoai.MaPL);
            return View(dienThoai);
        }
        //-------------------------
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Lấy sản phẩm cần chỉnh sửa từ CSDL dựa trên ID
            DienThoai dienThoai = database.DienThoais.Find(id);

            if (dienThoai == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaPL = new SelectList(database.PhanLoais, "MaPL", "TenPhanLoai", dienThoai.MaPL);

            return View(dienThoai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DienThoai dienThoai, HttpPostedFileBase Hinhchitiet0, HttpPostedFileBase HinhChiTiet1, HttpPostedFileBase HinhChiTiet2, HttpPostedFileBase HinhChiTiet3, HttpPostedFileBase HinhChiTiet4, HttpPostedFileBase HinhChiTiet5)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem sản phẩm có tồn tại trong CSDL hay không
                var existingDienThoai = database.DienThoais.Find(dienThoai.MaDT);

                if (existingDienThoai == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các thuộc tính của sản phẩm từ form chỉnh sửa
                existingDienThoai.MaPL = dienThoai.MaPL;
                existingDienThoai.TenDT = dienThoai.TenDT;
                existingDienThoai.GiaGoc = dienThoai.GiaGoc;
                existingDienThoai.Discount = dienThoai.Discount;
                existingDienThoai.GiaCuoi = dienThoai.GiaCuoi;
                existingDienThoai.ManHinh = dienThoai.ManHinh;
                existingDienThoai.LoaiManHinh = dienThoai.LoaiManHinh;
                existingDienThoai.Chip = dienThoai.Chip;
                existingDienThoai.Ram = dienThoai.Ram;
                existingDienThoai.Rom = dienThoai.Rom;
                existingDienThoai.Cam = dienThoai.Cam;
                existingDienThoai.CamSau = dienThoai.CamSau;
                existingDienThoai.Pin = dienThoai.Pin;
                existingDienThoai.Ngaycapnhat = dienThoai.Ngaycapnhat;
                existingDienThoai.Soluongban = dienThoai.Soluongban;
                existingDienThoai.soluotxem = dienThoai.soluotxem;

                // Xử lý lưu hình ảnh (tương tự như trong phần Create)
                if (Hinhchitiet0 != null && Hinhchitiet0.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Hinhchitiet0.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/san-pham/"), fileName);
                    Hinhchitiet0.SaveAs(path);
                    existingDienThoai.HinhChiTiet0 = "~/images/san-pham/" + fileName;
                }
                // Tương tự cho HinhChiTiet1, HinhChiTiet2, HinhChiTiet3, HinhChiTiet4, HinhChiTiet5

                // Lưu thay đổi vào CSDL
                database.Entry(existingDienThoai).State = EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.MaPL = new SelectList(database.PhanLoais, "MaPL", "TenPhanLoai", dienThoai.MaPL);
            return View(dienThoai);
        }
        //---------------------
        [HttpGet]
        public ActionResult Delete(int id)
        {
            // Lấy sản phẩm cần xóa từ CSDL dựa trên ID
            DienThoai dienThoai = database.DienThoais.Find(id);

            if (dienThoai == null)
            {
                return HttpNotFound();
            }

            return View(dienThoai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Lấy sản phẩm cần xóa từ CSDL dựa trên ID
            DienThoai dienThoai = database.DienThoais.Find(id);

            if (dienThoai == null)
            {
                return HttpNotFound();
            }

            // Xóa sản phẩm
            database.DienThoais.Remove(dienThoai);
            database.SaveChanges();            
            return RedirectToAction("Index");
        }

        public ActionResult QLSPPartial()
        {
            return PartialView();
        }

        public ActionResult Details(int id)
        {
            var dt = database.DienThoais.Where(c => c.MaDT ==id).FirstOrDefault();         
            return View(dt);
        }
    }
}