using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ban_dien_thoai.Models;

namespace web_ban_dien_thoai.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang

        public List<MatHangMua> LayGioHang()
        {
            //Xây dựng hàm để lấy giỏ hàng hiện tại
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        //Xây dựng hàm để thêm một sản phẩm vào giỏ (với mã sản phẩm là tham số đầu vào)
        public ActionResult ThemSanPhamVaoGio(int MaSP)
        {
            //Lấy giỏ hàng hiện tại
            List<MatHangMua> gioHang = LayGioHang();
            //Kiểm tra xem có tồn tại mặt hàng trong giỏ hay chưa
            //Nếu có thì tăng số lượng lên 1, ngược lại thêm vào giỏ
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanPham == null) //Sản phẩm chưa có trong giỏ
            {
                sanPham = new MatHangMua(MaSP);
                gioHang.Add(sanPham);
            }

            //Sản phẩm đã có trong giỏ thì không làm gì hết

            return RedirectToAction("Details", "SanPham", new { id = MaSP });
        }

        //Xây dựng hàm tính tổng số lượng mặt hàng được mua
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }
        //Xây dựng hàm tính tổng tiền của các sản phẩm được mua
        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            return TongTien;
        }
        //Xây dựng hàm hiển thị thông tin bên trong giỏ hàng
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang); //Trả về View hiển thị thông tin giỏ hàng
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        public ActionResult XoaMatHang(int MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();
            //Lấy sản phẩm trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaDT == MaSP);
                return RedirectToAction("HienThiGioHang"); //Quay về trang giỏ hàng
            }
            if (gioHang.Count == 0) //Quay về trang chủ nếu giỏ hàng không có gì
                return RedirectToAction("Index", "SanPham");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult CapNhatMatHang(int MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = LayGioHang();
            //Lấy sản phẩm trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaSP);
            if (sanpham != null)
            {
                //Cập nhật lại số lượng tương ứng
                //Lưu ý số lượng phải lớn hơn hoặc bằng 1
                sanpham.SoLuong = SoLuong;
            }
            return RedirectToAction("HienThiGioHang"); //Quay về trang giỏ hàng
        }
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("DangNhap", "NguoiDung");
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0) //Chưa có giỏ hàng hoặc chưa có sp
                return RedirectToAction("Index", "SanPham");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang); //Trả về View hiển thị thông tin giỏ hàng
        }
        testEntities database = new testEntities();
        //Xác nhận đơn và lưu vào CSDL


        public ActionResult DongYDatHang()
        {
            KhachHang khach = Session["TaiKhoan"] as KhachHang; //Khách
            List<MatHangMua> gioHang = LayGioHang(); //Giỏ hàng
            DonHang DonHang = new DonHang(); //Tạo mới đơn đặt hàng
            DonHang.MaKH = khach.MaKH;
            DonHang.NgayDH = DateTime.Now;
            DonHang.Trigia = (int)TinhTongTien();
            DonHang.DaNhan = null;
            DonHang.TenKhachHang = khach.HoTenKH;
            database.DonHangs.Add(DonHang);
            database.SaveChanges();
            //Lần lượt thêm từng chi tiết cho đơn hàng trên
            foreach (var sanpham in gioHang)
            {
                CTDatHang chitiet = new CTDatHang();
                chitiet.SoDH = DonHang.SoDH;
                chitiet.MaDT = sanpham.MaDT;
                chitiet.SoLuong = sanpham.SoLuong;
                chitiet.GiaCuoi = (int)sanpham.GiaCuoi;
                database.CTDatHangs.Add(chitiet);
            }

            database.SaveChanges();
            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }

        public ActionResult XemDonHang()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("DangNhap", "NguoiDung");
            // Lấy dữ liệu từ bảng CTDonHang trong CSDL
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            var donHang = database.CTDatHangs.OrderByDescending(dh => dh.SoDH).ToList();
            // Truyền dữ liệu vào View
            return View(donHang);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FailureView()
        {
            return View();
        }






        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                // Một nguồn lực đại diện cho Người thanh toán chi trả cho một phương thức thanh toán PayPal  
                // Id của Người thanh toán sẽ được trả về khi thanh toán tiếp tục hoặc khi nhấp để thanh toán  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // Phần này sẽ được thực hiện trước vì PayerID không tồn tại  
                    // Nó được trả về bởi cuộc gọi hàm create của lớp Payment  
                    // Tạo một thanh toán  
                    // baseURL là URL mà PayPal gửi lại dữ liệu.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    // Ở đây, chúng tôi đang tạo một GUID để lưu trữ paymentID nhận được trong session  
                    // nó sẽ được sử dụng trong việc thực hiện thanh toán  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    // Hàm CreatePayment cho chúng ta URL phê duyệt thanh toán  
                    // nơi mà người thanh toán được chuyển hướng để thanh toán qua tài khoản PayPal  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    // Lấy các liên kết được trả về từ PayPal trong phản hồi của cuộc gọi hàm Create  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            KhachHang khach = Session["TaiKhoan"] as KhachHang; //Khách
            List<MatHangMua> gioHang = LayGioHang(); //Giỏ hàng
            DonHang DonHang = new DonHang(); //Tạo mới đơn đặt hàng
            DonHang.MaKH = khach.MaKH;
            DonHang.NgayDH = DateTime.Now;
            DonHang.Trigia = (int)TinhTongTien();
            DonHang.DaNhan = null;
            DonHang.TenKhachHang = khach.HoTenKH;
            database.DonHangs.Add(DonHang);
            database.SaveChanges();
            //Lần lượt thêm từng chi tiết cho đơn hàng trên
            foreach (var sanpham in gioHang)
            {
                CTDatHang chitiet = new CTDatHang();
                chitiet.SoDH = DonHang.SoDH;
                chitiet.MaDT = sanpham.MaDT;
                chitiet.SoLuong = sanpham.SoLuong;
                chitiet.GiaCuoi = (int)sanpham.GiaCuoi;
                database.CTDatHangs.Add(chitiet);
            }

            database.SaveChanges();
            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
            //on successful payment, show success page to user.  
            //return View("HoanThanhDonHang");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            foreach (var item in gioHang)
            {
                itemList.items.Add(new Item()
                {
                    name = item.TenDT, // Thay thế bằng thuộc tính thực sự đại diện cho tên sản phẩm
                    currency = "USD",
                    price = item.GiaCuoi.ToString(), // Thay thế bằng thuộc tính thực sự đại diện cho giá sản phẩm
                    quantity = item.SoLuong.ToString(), // Thay thế bằng thuộc tính thực sự đại diện cho số lượng sản phẩm
                    sku = item.MaDT.ToString() // Thay thế bằng thuộc tính thực sự đại diện cho SKU sản phẩm
                });
            }
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = gioHang.Sum(item => item.ThanhTien()).ToString() // Tính tổng tiền dựa trên yêu cầu của bạn
            };
            //Final amount with details  
            // Tổng cộng cuối cùng với chi tiết
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.subtotal) + Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping)).ToString(), // Tổng phải bằng tổng của thuế, vận chuyển và subtotal.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}