using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_ThuongHieuController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();

        //
        // GET: /Admin_ThuongHieu/
       
        public ActionResult CreateThuonghieu()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateThuonghieu(FormCollection collection)
        {
            var thuonghieu = database.THUONGHIEUs.Select(row => row).ToList();
            // Tạo đối tượng nhóm sản phẩm mới
            int count = 0;
            if (thuonghieu.Count > 0)
            {
                count = int.Parse(thuonghieu[thuonghieu.Count - 1].MaThuongHieu.Substring(2));
            }
            THUONGHIEU item = new THUONGHIEU();
            // Lấy dữ liệu từ form
            item.MaThuongHieu = "TH" + string.Format("{0:D3}", thuonghieu.Count + 1);
            item.TenThuongHieu = collection["TenThuongHieu"];

            // Thêm nhóm sản phẩm mới vào CSDL
            database.THUONGHIEUs.InsertOnSubmit(item);
            database.SubmitChanges();

            // Sau khi thêm thành công, quay lại danh sách nhóm sản phẩm
            return RedirectToAction("Thuonghieu", "Admin");
        }

        public ActionResult EditThuonghieu(string id)
        {
            var rowData = database.THUONGHIEUs.Where(row => row.MaThuongHieu == id).FirstOrDefault();
            return PartialView(rowData);
        }

        [HttpPost]
        public ActionResult EditThuonghieu(string id, FormCollection collection)
        {
            var row = database.THUONGHIEUs.Where(r => r.MaThuongHieu == id).FirstOrDefault();
            var TenThuongHieu = collection["name"];
            row.TenThuongHieu = TenThuongHieu;
            UpdateModel(row);
            database.SubmitChanges();
            return RedirectToAction("Thuonghieu", "Admin");
        }
        public ActionResult DeleteThuonghieu(string id)
        {
            var SanPham = database.SANPHAMs.Where(row => row.MaThuongHieu == id).FirstOrDefault();
            if(SanPham==null)
            {
                var rowData = database.THUONGHIEUs.Where(row => row.MaThuongHieu == id).FirstOrDefault();
                return PartialView(rowData);
            }else
                return PartialView();   
        }
        [HttpPost]
        public ActionResult DeleteThuonghieu(string id, FormCollection collection)
        {
            var row = database.THUONGHIEUs.Where(r => r.MaThuongHieu == id).FirstOrDefault();

            database.THUONGHIEUs.DeleteOnSubmit(row);
            database.SubmitChanges();
            return RedirectToAction("Thuonghieu", "Admin");
        }
	}
}