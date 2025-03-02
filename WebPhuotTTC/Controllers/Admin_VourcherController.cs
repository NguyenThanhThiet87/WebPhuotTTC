using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_VourcherController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Admin_Vourcher/
        
        public ActionResult CreateVourcher()
        {
            var giamGiaSP = database.GIAMGIA_SANPHAMs.Select(row => row.SANPHAM.MaLoai);
            ViewBag.ThuongHieu = database.THUONGHIEUs.Select(row => row);
            ViewBag.LoaiSP = database.LOAISANPHAMs.Where(row=>!giamGiaSP.Contains(row.MaLoai)).Select(row => row);
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateVourcher(FormCollection collection)
        {
                // Tạo đối tượng GIAMGIA mới
                GIAMGIA newVoucher = new GIAMGIA();
                var vourcherDB = database.GIAMGIAs.Select(row => row).ToList();
            int count=0;
            if (vourcherDB.Count>0)
                count = int.Parse(vourcherDB.LastOrDefault().MaGiamGia.Substring(2));
                
                // Lấy dữ liệu từ form
                newVoucher.MaGiamGia ="VC"+String.Format("{0:D3}",count+1);
                newVoucher.NoiDungGiamGia = collection["MoTaGiamGia"];
                newVoucher.TiLeGiamGia = double.Parse(collection["TiLeGiamGia"]);
                newVoucher.ThoiGianBatDau = DateTime.Parse(collection["ThoiGianBatDau"]);
                newVoucher.ThoiGianKetThuc = DateTime.Parse(collection["ThoiGianKetThuc"]);

                // Lưu mã giảm giá vào CSDL
                database.GIAMGIAs.InsertOnSubmit(newVoucher);
                database.SubmitChanges();

                var thuongHieuAp=collection["ThuongHieu"].Split(',').ToList();
                var LoaiSPAp = collection["LoaiSP"].Split(',').ToList();
                var sanpham = database.SANPHAMs.Where(row => thuongHieuAp.Contains(row.MaThuongHieu.ToString()) && LoaiSPAp.Contains(row.MaLoai.ToString())).Select(row => row);
                foreach(var item in sanpham)
                {
                   //Gán khuyến mãi giá cho các sản phẩm
                   GIAMGIA_SANPHAM giamGiaSP = new GIAMGIA_SANPHAM();
                   giamGiaSP.MaGiamGia = newVoucher.MaGiamGia;
                   giamGiaSP.MaSanPham = item.MaSanPham;
                   database.GIAMGIA_SANPHAMs.InsertOnSubmit(giamGiaSP);
                   database.SubmitChanges();
                }
                // Trả về danh sách mã giảm giá sau khi tạo thành công
                return RedirectToAction("Vourcher","Admin");
        }

        public ActionResult EditVourcher(string id)
        {
            var rowData = database.GIAMGIAs.Where(row => row.MaGiamGia == id).FirstOrDefault();
            return PartialView(rowData);
        }
        [HttpPost]
        public ActionResult EditVourcher(string id, FormCollection collection)
        {
            var Vourcher = database.GIAMGIAs.Where(row => row.MaGiamGia == id).Select(row => row).FirstOrDefault();
                // Cập nhật mã giảm giá vào CSDL
            Vourcher.NoiDungGiamGia = collection["MoTaGiamGia"];
            Vourcher.TiLeGiamGia = double.Parse(collection["TiLeGiamGia"]);
            Vourcher.ThoiGianBatDau = DateTime.Parse(collection["ThoiGianBatDau"]);
            Vourcher.ThoiGianKetThuc = DateTime.Parse(collection["ThoiGianKetThuc"]);
            UpdateModel(Vourcher);
            database.SubmitChanges();
                // Trả về danh sách mã giảm giá sau khi chỉnh sửa thành công
            return RedirectToAction("Vourcher","Admin");
        }
        public ActionResult DeleteVourcher(string id)
        {
            // Tìm sản phẩm theo id
            var rowData = database.GIAMGIAs.Where(row => row.MaGiamGia == id).FirstOrDefault();
            return PartialView(rowData);
        }
        [HttpPost]
        public ActionResult DeleteVourcher(string id, FormCollection collection)
        {
           var giamGiaSP = database.GIAMGIA_SANPHAMs.Where(row => row.MaGiamGia == id).Select(row => row);
           database.GIAMGIA_SANPHAMs.DeleteAllOnSubmit(giamGiaSP);
           database.SubmitChanges();
           var giamgia = database.GIAMGIAs.Where(r => r.MaGiamGia == id).FirstOrDefault();
           // Xóa giảm giá
           database.GIAMGIAs.DeleteOnSubmit(giamgia);
           database.SubmitChanges();
            // Điều hướng về trang thuonghieu sau khi xóa thành công
            return RedirectToAction("Vourcher","Admin");
        }
        public ActionResult DetailVourcher(String id)
        {
            List<GIAMGIA_SANPHAM> listGGDetail=new List<GIAMGIA_SANPHAM>();
            var detailVourcherDB = database.GIAMGIA_SANPHAMs.Where(row => row.MaGiamGia == id).Select(row => row);
            listGGDetail = detailVourcherDB.ToList();
            if (listGGDetail.Count==0)
            {
                listGGDetail.Add(new GIAMGIA_SANPHAM
                {
                    MaGiamGia = id,
                    GIAMGIA = database.GIAMGIAs.Where(row => row.MaGiamGia == id).FirstOrDefault(),
                    MaSanPham = null
                });
            }
            return View(listGGDetail);
        }
	}
}