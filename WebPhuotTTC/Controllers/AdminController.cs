using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;
namespace WebPhuotTTC.Controllers
{
    public class AdminController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        // GET: Admin
        public ActionResult Home()
        {
            return View();
        }
       
        public JsonResult TongDoanhThu()
        {
            var SP = database.CHITIETDONHANGs.Select(row => row).ToList();
            var LoaiSanPham = database.LOAISANPHAMs.Select(row => row).ToList();

            // Kiểm tra số lượng sản phẩm
            int[] productSales = new int[LoaiSanPham.Count];

            for (int item = 0; item < LoaiSanPham.Count; item++)
            {
                var loai = database.CHITIETDONHANGs
                    .Where(row => row.QLSANPHAM.SANPHAM.MaLoai == LoaiSanPham[item].MaLoai)
                    .Select(row => row)
                    .ToList();

                // Tổng số lượng bán được của loại sản phẩm này
                productSales[item] = loai.Sum(row => row.SoLuong);
            }

            // Kiểm tra kết quả trước khi trả về
            System.Diagnostics.Debug.WriteLine("Product Sales: " + string.Join(", ", productSales));

            return Json(productSales, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Nhomsanpham()
        {
            var nhomsanpham = from row in database.NHOMSANPHAMs select row;
            return View(nhomsanpham.ToList());
        }
        public ActionResult LoaiSanPham()
        {
            var LoaiSP = database.LOAISANPHAMs.Select(row => row).ToList();
            return View(LoaiSP);
        }
        public ActionResult Thuonghieu()
        {
            var thuonghieu = from row in database.THUONGHIEUs select row;
            return View(thuonghieu.ToList());
        }
        public ActionResult Nguoidung()
        {
            var nguoidung = from row in database.NGUOIDUNGs select row;
            return View(nguoidung.ToList());
        }
        public ActionResult Sanpham()
        {
            var sanpham = database.SANPHAMs.Select(row=>row).ToList();
            sanpham.Reverse();
            return View(sanpham);
        }
        public ActionResult Vourcher()
        {
            var vourcher = from row in database.GIAMGIAs select row;
            return View(vourcher.ToList());
        }
        public ActionResult DonHang()
        {
            var donhang = database.DONHANGs.Select(row => row).ToList();
            return View(donhang);
        }
    }
}