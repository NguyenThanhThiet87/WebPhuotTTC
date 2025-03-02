using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_LoaiSPController : Controller
    {
        // GET: Admin_LoaiSP
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Admin_NhomSP/

      
        public ActionResult CreateLoaisanpham()
        {
            var LoaiSanPham = database.NHOMSANPHAMs.Select(row => row).ToList();
            return PartialView(LoaiSanPham);
        }

        [HttpPost]
        public ActionResult CreateLoaisanpham(FormCollection collection)
        {
            var LoaiSP = database.LOAISANPHAMs.Select(row => row).ToList();
            int count = 0;
            if (LoaiSP.Count > 0)
            {
                count = int.Parse(LoaiSP[LoaiSP.Count - 1].MaLoai.Substring(3));
            }
            // Tạo đối tượng nhóm sản phẩm mới
            LOAISANPHAM item = new LOAISANPHAM();
            // Lấy dữ liệu từ form
            item.MaLoai = "LSP" + string.Format("{0:D2}", count + 1);
            item.TenLoai = collection["TenLoai"];
            item.MaNhom = collection["MaNhom"];

            // Thêm nhóm sản phẩm mới vào CSDL
            database.LOAISANPHAMs.InsertOnSubmit(item);
            database.SubmitChanges();

            // Sau khi thêm thành công, quay lại danh sách loại sản phẩm
            return RedirectToAction("LoaiSanPham", "Admin");
        }
        public ActionResult EditLoaisanpham(string id)
        {
            var rowData = database.LOAISANPHAMs.Where(row => row.MaLoai == id).FirstOrDefault();
            var nhom = database.NHOMSANPHAMs.Select(row => row).ToList();
            var data = new Loai_NhomSP
            {
                loaisp = rowData,
                nhomsp=nhom
            };
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult EditLoaisanpham(string id, FormCollection collection)
        {
            var row = database.LOAISANPHAMs.Where(r => r.MaLoai == id).FirstOrDefault();

            var TenLoai = collection["name"];
            row.TenLoai = TenLoai;
            row.MaNhom = collection["MaNhom"];
            UpdateModel(row);
            database.SubmitChanges();
            return RedirectToAction("Loaisanpham", "Admin");
        }

        public ActionResult DeleteLoaisanpham(string id)
        {
            var rowData = database.LOAISANPHAMs.Where(row => row.MaLoai == id).FirstOrDefault();
            return PartialView(rowData);
        }
        [HttpPost]
        public ActionResult DeleteLoaisanpham(string id, FormCollection collection)
        {
            var row = database.LOAISANPHAMs.Where(r => r.MaLoai == id).FirstOrDefault();

            database.LOAISANPHAMs.DeleteOnSubmit(row);
            database.SubmitChanges();
            return RedirectToAction("Loaisanpham", "Admin");
        }
    }
}