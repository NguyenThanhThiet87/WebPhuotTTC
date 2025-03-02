using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_NhomSPController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Admin_NhomSP/
        
        public ActionResult CreateNhomsanpham()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNhomsanpham(FormCollection collection)
        {
            var nhomsp=database.NHOMSANPHAMs.Select(row => row).ToList();
            // Tạo đối tượng nhóm sản phẩm mới
            NHOMSANPHAM item = new NHOMSANPHAM();

            // Lấy dữ liệu từ form
            item.MaNhom = "NSP" + string.Format("{0:D2}", nhomsp.Count + 1);
            item.TenNhom = collection["TenNhom"];

            // Thêm nhóm sản phẩm mới vào CSDL
            database.NHOMSANPHAMs.InsertOnSubmit(item);
            database.SubmitChanges();

            // Sau khi thêm thành công, quay lại danh sách nhóm sản phẩm
            return RedirectToAction("Nhomsanpham","Admin");
        }
        public ActionResult EditNhomsanpham(string id)
        {
            var rowData = database.NHOMSANPHAMs.Where(row => row.MaNhom == id).FirstOrDefault();
            return PartialView(rowData);
        }

        [HttpPost]
        public ActionResult EditNhomsanpham(string id, FormCollection collection)
        {
            var row = database.NHOMSANPHAMs.Where(r => r.MaNhom == id).FirstOrDefault();

            var TenNhom = collection["name"];
            row.TenNhom = TenNhom;
            UpdateModel(row);
            database.SubmitChanges();
            return RedirectToAction("Nhomsanpham", "Admin");
        }

        public ActionResult DeleteNhomsanpham(string id)
        {
            var rowData = database.NHOMSANPHAMs.Where(row => row.MaNhom == id).FirstOrDefault();
            return PartialView(rowData);
        }

        [HttpPost]
        public ActionResult DeleteNhomsanpham(string id, FormCollection collection)
        {
            var row = database.NHOMSANPHAMs.Where(r => r.MaNhom == id).FirstOrDefault();

            database.NHOMSANPHAMs.DeleteOnSubmit(row);
            database.SubmitChanges();
            return RedirectToAction("Nhomsanpham","Admin");
        }
	}
}