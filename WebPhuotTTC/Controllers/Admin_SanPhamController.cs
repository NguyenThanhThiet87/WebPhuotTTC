using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;
using System.Configuration;

namespace WebPhuotTTC.Controllers
{
    public class Admin_SanPhamController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Admin_SanPham/
        private Cloudinary InitializeCloudinary()
        {
            // Lấy thông tin cấu hình từ Web.config
            string cloudName = "duouljna1";
            string apiKey = "742816844663867";
            string apiSecret = "2O7azY63fL30VmyJ7piTybiWaBc";

            // Kiểm tra xem thông tin có được cấu hình hay không
            if (string.IsNullOrWhiteSpace(cloudName) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new ArgumentException("Cloudinary configuration values are not set properly.");
            }

            // Tạo đối tượng Account và Cloudinary
            Account account = new Account(cloudName, apiKey, apiSecret);
            return new Cloudinary(account);
        }

        public ActionResult CreateSanpham()
        {
            ViewBag.KichThuoc = database.KICHTHUOCs.Select(row => row).ToList();
            ViewBag.MauSac = database.MAUSACs.Select(row => row).ToList();
            ViewBag.ThuongHieu = database.THUONGHIEUs.Select(row => row).ToList();
            ViewBag.LoaiSP = database.LOAISANPHAMs.Select(row => row).ToList();
            ViewBag.BaoHanh = database.BAOHANHs.Select(row => row).ToList();
            return PartialView();
        }
        private List<String> UploadImageToCloudinary(IEnumerable<HttpPostedFileBase> AnhDetail, HttpPostedFileBase AnhBia)
        {
            //Tạo đối tượng cloudinary
            Cloudinary cloudinary = InitializeCloudinary();
            List<String> UrlImages = new List<String>();//lưu trữ đường dẫn trả về từ cloudinary
            //Upload Anh bìa lên cloudinary
            var resultUpload = cloudinary.Upload(new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new FileDescription(AnhBia.FileName, AnhBia.InputStream)
            });
            UrlImages.Add(resultUpload.SecureUri.ToString());
            //Upload Anh Detail lên cloudinary
            foreach (var item in AnhDetail)
            {
                resultUpload = cloudinary.Upload(new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new FileDescription(item.FileName, item.InputStream)
                });
                UrlImages.Add(resultUpload.SecureUri.ToString());
            }
            return UrlImages;
        }
        private void DeleteImageOnCloudinary(IEnumerable<HINHANH> images)
        {
            Cloudinary cloudinary = InitializeCloudinary();
            foreach(var item in images)
            {
                String url = item.Url.Substring(item.Url.LastIndexOf('/'));
                var result=cloudinary.Destroy(new CloudinaryDotNet.Actions.DeletionParams(url));
                if (result.Result == "ok")
                {
                    Console.WriteLine("Image deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Error deleting image: " + result.Result);
                }
            }
        }
        [HttpPost]
        public ActionResult CreateSanpham(FormCollection collection, IEnumerable<HttpPostedFileBase> AnhDetail, HttpPostedFileBase AnhBia)
        {

             //Đẩy ảnh và lấy đường dẫn ảnh từ cloudinary
             List<String> UrlImages = UploadImageToCloudinary(AnhDetail, AnhBia);
             
             // Tạo đối tượng SANPHAM mới
             SANPHAM newProduct = new SANPHAM();
             var spDB = database.SANPHAMs.Select(row => row).ToList();
             int count = int.Parse(spDB.LastOrDefault().MaSanPham);
             // Lấy dữ liệu từ form
             newProduct.MaSanPham = string.Format("{0:D5}", count + 1);
             newProduct.TenSanPham = collection["TenSanPham"];
             newProduct.MoTa = collection["MoTa"];
             newProduct.AnhBia = UrlImages[0];
             newProduct.Gia = int.Parse(collection["Gia"]);
             newProduct.MaBaoHanh = collection["BaoHanh"];
             newProduct.MaThuongHieu = collection["ThuongHieu"];
             newProduct.MaLoai = collection["LoaiSP"];

             database.SANPHAMs.InsertOnSubmit(newProduct);
             database.SubmitChanges();
             // thêm chi tiết chi sản phẩm
             var colors = collection.GetValues("Colors");
             var sizes = collection.GetValues("Sizes");
             var soLuong = collection.GetValues("SoLuong");
             for(int i=0;i<colors.Length;i++)
             {
                 QLSANPHAM detailsp = new QLSANPHAM();
                 detailsp.MaSanPham = newProduct.MaSanPham;
                 detailsp.MaMau = colors[i];
                 detailsp.MaKichThuoc = sizes[i];
                 detailsp.SoLuong = int.Parse(soLuong[i]);
                 database.QLSANPHAMs.InsertOnSubmit(detailsp);
                 database.SubmitChanges();
             }
            
            for(int i=1; i< UrlImages.Count;i++)
            {
                var anhDB = database.HINHANHs.Select(row => row).ToList();
                if(anhDB!=null)
                     count = int.Parse(anhDB.LastOrDefault().MaAnh);
                HINHANH anh = new HINHANH();
                anh.MaSanPham = newProduct.MaSanPham;
                anh.MaAnh = String.Format("{0:D5}", count + 1);
                anh.Url = UrlImages[i];
                database.HINHANHs.InsertOnSubmit(anh);
                database.SubmitChanges();
            }
            // Trả về danh sách sản phẩm sau khi tạo thành công
            return RedirectToAction("Sanpham", "Admin");
        }
      
        public ActionResult EditSanpham(string id)
        {
            ViewBag.KichThuoc = database.KICHTHUOCs.Select(row => row).ToList();
            ViewBag.MauSac = database.MAUSACs.Select(row => row).ToList();
            ViewBag.ThuongHieu = database.THUONGHIEUs.Select(row => row).ToList();
            ViewBag.LoaiSP = database.LOAISANPHAMs.Select(row => row).ToList();
            ViewBag.BaoHanh = database.BAOHANHs.Select(row => row).ToList();
            var spDB = database.SANPHAMs.Where(row => row.MaSanPham == id).Select(row => row).FirstOrDefault();
            return PartialView(spDB);
        }

        [HttpPost]
        public ActionResult EditSanpham(string id, FormCollection collection)
        {
            var product = database.SANPHAMs.Where(row => row.MaSanPham == id).FirstOrDefault();
            // Lấy dữ liệu từ form
            product.TenSanPham = collection["TenSanPham"];
            product.MoTa = Request.Form["MoTa"];
            product.Gia = int.Parse(Request.Form["Gia"]);
            product.MaThuongHieu = Request.Form["MaThuongHieu"];
            product.MaLoai = Request.Form["MaLoai"];           // Gán loại sản phẩm
            product.MaBaoHanh = Request.Form["MaBaoHanh"];         // Gán mã bảo hành
              // Gán thương hiệu

            UpdateModel(product);
            database.SubmitChanges();

            return RedirectToAction("SanPham","Admin");
        }
        public ActionResult DeleteSanpham(string id)
        {
            // Tìm sản phẩm theo id
            var rowData = database.SANPHAMs.Where(row => row.MaSanPham == id).FirstOrDefault();
            return PartialView(rowData); 
        }

        [HttpPost]
        public ActionResult DeleteSanpham(string id, FormCollection collection)
        {
            // Tìm sản phẩm theo id
            var spDB = database.SANPHAMs.Where(r => r.MaSanPham == id).FirstOrDefault();
            var qlSP = database.QLSANPHAMs.Where(row => row.MaSanPham == id).Select(row => row);
            var anhDB = database.HINHANHs.Where(row => row.MaSanPham == id).Select(row => row);
            database.QLSANPHAMs.DeleteAllOnSubmit(qlSP);
            database.HINHANHs.DeleteAllOnSubmit(anhDB);  
            database.SANPHAMs.DeleteOnSubmit(spDB);
            var ggSP = database.GIAMGIA_SANPHAMs.Where(row => row.MaSanPham == id).Select(row => row);
            database.GIAMGIA_SANPHAMs.DeleteAllOnSubmit(ggSP);
            database.SubmitChanges();
            // Điều hướng về trang thuonghieu sau khi xóa thành công
            return RedirectToAction("SanPham","Admin");
        }
        public ActionResult DetailSanPham(string id)
        {
            var spDB = database.SANPHAMs.Where(row => row.MaSanPham == id).Select(row => row).FirstOrDefault();
            var HinhAnh = database.HINHANHs.Where(row => row.MaSanPham == id).Select(row => row).ToList();
            var detailSP = database.QLSANPHAMs.Where(row => row.MaSanPham == id).Select(row => row).ToList();
            ProductDetail productDetail = new ProductDetail();
            productDetail.detailProduct = detailSP;
            productDetail.product = spDB;
            productDetail.images = HinhAnh;
            productDetail.discount = null;
            productDetail.comment = null;
            return View(productDetail);
        }
        
    }
}