using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebPhuotTTC.Models;
namespace WebPhuotTTC.Controllers
{
    public class CartController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Cart/
        private double TongTien(List<CHITIETGIOHANG> cart)
        {
            double total = cart.Sum(row => row.DonGia);
            return total;
        }
        
        public ActionResult GioHangShort()
        {
            var IDUser = Session["IDUSER"];
            if(IDUser!=null)
            {
                int ID = int.Parse(IDUser.ToString());
                var cart = database.CHITIETGIOHANGs.Where(row => row.ID == ID).Select(row => row).ToList();
                double Sum = TongTien(cart);
                ViewBag.TongTien = Sum;
                return PartialView(cart);
            }
            else
            {
                return PartialView();
            }
        }
        public ActionResult GioHangItemMenu()
        {
            var IDUser = Session["IDUSER"];
            if(IDUser!=null)
            {
                int id = int.Parse(IDUser.ToString());
                var cart = database.CHITIETGIOHANGs.Where(row => row.ID == id).Select(row => row).ToList();
                ViewBag.SoLuong = cart.Count;
                return PartialView(IDUser);
            }else
            {
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult ThemVaoGioHang(String maSP, String maKT, String maMAU)
        {
            var message="";
                var IDUser = Session["IDUSER"];
                if (IDUser != null )
                {
                    int id = int.Parse(IDUser.ToString());
                    var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                    var product = database.QLSANPHAMs.Where(row => row.MaSanPham == maSP && row.MaMau == maMAU && row.MaKichThuoc == maKT).FirstOrDefault();//lấy các thông tin về sản phẩm
                    if (product != null && user != null)
                    {
                        //Lấy giảm giá sản phẩm
                        var discount_SP = database.GIAMGIA_SANPHAMs.Where(row => row.MaSanPham == product.MaSanPham).FirstOrDefault();
                        double giaGiam=0;
                        if (discount_SP != null)
                        {
                            var Discount = database.GIAMGIAs.Where(row => row.MaGiamGia == discount_SP.MaGiamGia).FirstOrDefault();
                            giaGiam = product.SANPHAM.Gia / 100 * (Discount.TiLeGiamGia ?? 0);
                        }

                        // Tìm sản phẩm trong giỏ hàng
                        var giohang = database.CHITIETGIOHANGs.Where(row => row.ID == id && row.MaSanPham == product.MaSanPham && row.MaKichThuoc == product.MaKichThuoc && row.MaMau == product.MaMau).FirstOrDefault();
                       
                        if (giohang != null)
                        {
                            // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng và giá
                            giohang.SoLuong++;
                            giohang.DonGia = (product.SANPHAM.Gia-giaGiam) * (giohang.SoLuong);
                            UpdateModel(giohang);
                        }
                        else
                        {
                            // Nếu sản phẩm chưa có trong giỏ hàng, tạo mới
                            CHITIETGIOHANG item = new CHITIETGIOHANG();  // Khởi tạo đối tượng CHITIET mới
                            item.MaSanPham = product.MaSanPham;
                            item.ID = user.ID;
                            item.MaKichThuoc = product.MaKichThuoc;
                            item.MaMau = product.MaMau;
                            item.SoLuong = 1;
                            item.DonGia = (product.SANPHAM.Gia-giaGiam);

                            // Thêm vào database
                            database.CHITIETGIOHANGs.InsertOnSubmit(item);
                        }

                    // Lưu thay đổi
                        database.SubmitChanges();
                    message = "Thêm vào giỏ hàng thành công"; 
                }
                else
                    message = "Đã xảy ra lỗi";
                }else
                message = "Bạn chưa đăng nhập"; 
            
            return Json(new {success=true,message=message});
        }

        [HttpPost]
        public JsonResult UpdateSoLuongItemGioHang(String maSP, String maKT, String maMAU,int SoLuong)
        {
             var IDUser = Session["IDUSER"];
             if (IDUser != null)
             {
                 int id = int.Parse(IDUser.ToString());
                 var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                 // Tìm sản phẩm trong giỏ hàng
                 var giohang = database.CHITIETGIOHANGs.Where(row => row.ID == id && row.MaSanPham == maSP && row.MaKichThuoc == maKT && row.MaMau == maMAU).FirstOrDefault();
                 giohang.SoLuong = SoLuong;
                 UpdateModel(giohang);
                 database.SubmitChanges();
                 return Json(new { success = true });
             }
             return Json(new { success = false });
        }
        public JsonResult XoaItemGioHang(String maSP, String maKT, String maMAU)
        {
            var message = "";
            var IDUser = Session["IDUSER"];
            if (IDUser != null)
            {
                int id = int.Parse(IDUser.ToString());
                var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                var product = database.QLSANPHAMs.Where(row => row.MaSanPham == maSP && row.MaMau == maMAU && row.MaKichThuoc == maKT).FirstOrDefault();//lấy các thông tin về sản phẩm
                if (product != null && user != null)
                {
                    // Tìm sản phẩm trong giỏ hàng
                    var giohang = database.CHITIETGIOHANGs.Where(row => row.ID == id && row.MaSanPham == product.MaSanPham && row.MaKichThuoc == product.MaKichThuoc && row.MaMau == product.MaMau).FirstOrDefault();

                    if (giohang != null)
                    {
                        // Nếu sản phẩm đã tồn tại trong giỏ hàng, xóa sản phẩm
                        database.CHITIETGIOHANGs.DeleteOnSubmit(giohang);
                        // Lưu thay đổi
                        database.SubmitChanges();
                        message = "Xóa sản phẩm thành công";
                    }
                    else
                    {
                        // Nếu sản phẩm chưa có trong giỏ hàng, tạo mới
                        message = "Sản phẩm không tồn tại";
                    }
                }
                else
                    message = "Đã xảy ra lỗi";
            }
            else
                message = "Bạn chưa đăng nhập";
            // Redirect về URL đã cung cấp
            return Json(new {success=true, message=message});
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            var IDUser = Session["IDUSER"];
            // Logic xử lý đặt hàng
            // Kiểm tra xem collection có chứa dữ liệu không
            List<CHITIETGIOHANG> data = new List<CHITIETGIOHANG>();
            if (collection != null || IDUser != null)
            {
                int id = int.Parse(IDUser.ToString());
                var nguoidung = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                if (nguoidung.DiaChi == null || nguoidung.DiaChi == "")
                {
                    ViewBag.ThongBao = "Bạn chưa nhập địa chỉ";
                    return RedirectToAction("PersonalInfomation", "Account");
                }
                var itemGioHang = database.CHITIETGIOHANGs.Where(row => row.ID == id ).Select(row => row).ToList();

                foreach(var item in itemGioHang)
                {
                    if (collection[item.MaSanPham]!=null)
                        data.Add(item);
                }
                Session["SanPhamDonHang"] = data;
                ViewBag.TongTien = data.Sum(row => row.DonGia);
                return View(data);
            }

            // Trả về view hoặc redirect sau khi xử lý
            return View();
        }

        [HttpPost]
        public ActionResult MuaNgay(FormCollection collection, String maSP)
        {
            String maKT = collection["size"];
            String maMau = collection["color"];
            var IDUser = Session["IDUSER"];
            if (maKT == null || maMau == null )
                return new EmptyResult();
            // Logic xử lý đặt hàng
            List<CHITIETGIOHANG> data = new List<CHITIETGIOHANG>();
            if (IDUser != null)
            {
                int id = int.Parse(IDUser.ToString());
                var nguoidung=database.NGUOIDUNGs.Where(row=>row.ID==id).FirstOrDefault();

                var sanpham = database.QLSANPHAMs.Where(row => row.MaSanPham == maSP && row.MaKichThuoc == maKT && row.MaMau == maMau).FirstOrDefault();
                var MaGiamGia = database.GIAMGIA_SANPHAMs.Where(row => row.MaSanPham == maSP).FirstOrDefault();
                double giaGiam = 0;
                if (MaGiamGia != null)
                    giaGiam = database.GIAMGIAs.Where(row => row.MaGiamGia == MaGiamGia.MaGiamGia).FirstOrDefault().TiLeGiamGia ?? 0;
                data.Add(new CHITIETGIOHANG
                {
                    ID = id,
                    MaKichThuoc = maKT,
                    MaSanPham = maSP,
                    MaMau = maMau,
                    SoLuong = 1,
                    DonGia = sanpham.SANPHAM.Gia-giaGiam,
                    QLSANPHAM=sanpham,
                    NGUOIDUNG = nguoidung
                });
                if(data==null)
                    return new EmptyResult();
                Session["SanPhamDonHang"] = data;
                ViewBag.TongTien = data.Sum(row => row.DonGia);
                return View("DatHang",data);
            }
            else
                return new EmptyResult();
        }
        public ActionResult XacNhanDatHang()
        {
            var GioHangChon = Session["SanPhamDonHang"] as List<CHITIETGIOHANG>;
            // Kiểm tra listSP khác null và có ít nhất 1 sản phẩm
            // Khởi tạo đơn hàng
                DONHANG donhang = new DONHANG();

                // Lấy danh sách đơn hàng hiện có để tạo mã đơn hàng mới
                var listDonHang = database.DONHANGs.ToList();
                int count = 0;
                if(listDonHang.Count>0)
                {
                    count = int.Parse(listDonHang.Last().MaDonHang.Substring(2));
                }
                donhang.MaDonHang = "DH" + string.Format("{0:D3}", count + 1);

                DateTime utcNow = DateTime.UtcNow;
                DateTime vietNamTime = utcNow.AddHours(7); // Giờ Việt Nam là UTC+7
                donhang.NgayLap = vietNamTime;

                donhang.TongTien = GioHangChon.Sum(item => item.DonGia);
                donhang.ID = int.Parse(Session["IDUSER"].ToString());
                donhang.MaTrangThai = 1;
                // Thêm đơn hàng vào cơ sở dữ liệu
                database.DONHANGs.InsertOnSubmit(donhang);
                database.SubmitChanges();
            // Thêm chi tiết đơn hàng
                foreach (var item in GioHangChon)
                {
                        CHITIETDONHANG chitietdonhang = new CHITIETDONHANG
                        {
                            MaDonHang = donhang.MaDonHang,
                            MaSanPham = item.MaSanPham,
                            MaMau = item.MaMau,
                            MaKichThuoc = item.MaKichThuoc,
                            SoLuong=item.SoLuong,
                            DonGia=item.DonGia
                        };
                        database.CHITIETDONHANGs.InsertOnSubmit(chitietdonhang);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                database.SubmitChanges();
                TempData["ThongBao"] = "Đặt hàng thành công";

                return RedirectToAction("DonHangDaDat", "Cart");
        }
        public ActionResult DonHangDaDat()
        {
            var IDUser = Session["IDUSER"];
            // Logic xử lý đặt hàng
            if (IDUser != null)
            {
                int id = int.Parse(IDUser.ToString());
                var lstDonHang = database.DONHANGs.Where(row => row.ID == id).OrderBy(row => row.NgayLap).ToList();
                List<DonHangDetail> data=new List<DonHangDetail>();
                foreach(var item in lstDonHang)
                {
                    var chitiet = database.CHITIETDONHANGs.Where(row => row.MaDonHang == item.MaDonHang).Select(row => row).ToList();
                    data.Add(new DonHangDetail
                    {
                        donhang = item,
                        chitietdonhang = chitiet
                    });
                }
                return View(data);
            }
            return View();
        }
        public ActionResult ListDonHang(String sort, List<DonHangDetail> listSP)
        {
            if(sort!= "Tất cả")
            {
                // Lọc và gán lại giá trị cho listSP
                listSP = listSP.Where(item => item.donhang.TRANGTHAI.TenTrangThai == sort).ToList();
            }
            return PartialView(listSP);
        }
       
    }
}