using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class SanPhamController : Controller
    {
        testEntities database = new testEntities();
        // GET: SanPham



        public ActionResult Search(string searchQuery)
        {

            TempData["SearchQuery"] = searchQuery;
            // Thực hiện truy vấn tìm kiếm trong cơ sở dữ liệu
            var searchResults = database.DienThoais
                .Where(DienThoai =>  DienThoai.TenDT.Contains(searchQuery))
                .ToList();

            // Truyền kết quả tìm kiếm đến view Index để hiển thị
            return View("Index", searchResults);
        }
        private List<DienThoai> LayDT(int soluong)
        {
            // Sắp xếp theo cập nhật giảm dần, lấy đúng số lượng cần
            // Chuyển qua dạng danh  kết quả đạt được
            return database.DienThoais.OrderByDescending(DienThoai => DienThoai.MaPL).Take(soluong).ToList();
        }

        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "price" ? "price_desc" : "price";

            var dsDT = LayDT(16);

            switch (sortOrder)
            {
                case "name_desc":
                    dsDT = dsDT.OrderByDescending(DienThoai => DienThoai.soluotxem).ToList();
                    break;
                case "price":
                    dsDT = dsDT.OrderBy(DienThoai => DienThoai.GiaCuoi).ToList();
                    break;
                case "price_desc":
                    dsDT = dsDT.OrderByDescending(DienThoai => DienThoai.GiaCuoi).ToList();
                    break;
                default:
                    dsDT = dsDT.OrderBy(DienThoai => DienThoai.TenDT).ToList();
                    break;
            }

            return View(dsDT);
        }

        public ActionResult LayChuDe()
        {
            var dsChuDe = database.PhanLoais.ToList();
            return PartialView(dsChuDe);
        }
        public ActionResult SPTheoChuDe(int id)
        {

            // Lấy tên chủ đề dựa trên id
            var tenChuDe = database.PhanLoais.Where(pl => pl.MaPL == id).Select(pl => pl.TenPhanLoai).FirstOrDefault();
            //Lấy các sách theo mã chủ đề được chọn
            var dsSachTheoChuDe = database.DienThoais.OrderByDescending(DienThoai => DienThoai.Ngaycapnhat).Where(PhanLoai=> PhanLoai.MaPL == id).ToList();
            //Trả về View để render các sách trên (tái sử dụng View Index ở trên, truyền vào danh sách)

            // Đặt tên chủ đề vào ViewBag để sử dụng trong view
            ViewBag.SelectedTopic = tenChuDe; // Thay "Tên Chủ đề" bằng logic để lấy tên chủ đề tương ứng.

            return View("Index", dsSachTheoChuDe);
        }
        public ActionResult Details(int id)
        {
            //Lấy sách có mã tương ứng
            var sp = database.DienThoais.FirstOrDefault(s => s.MaDT == id);
            return View(sp);
        }


        public ActionResult GetReviews(int productId)
        {
            var reviews = database.DanhGiaSPs.Where(review => review.MaDT == productId).ToList();
            return View(reviews);
        }


        [HttpPost]
        public ActionResult PostReview(int productId, int userId, string content)
        {
            var review = new DanhGiaSP
            {
                MaDT = productId,
                MaKH = userId,
                NoiDung = content,
                NgayDang = DateTime.Now
            };

            database.DanhGiaSPs.Add(review);
            database.SaveChanges();

            return RedirectToAction("Details", new { id = productId });

        }

    }
}