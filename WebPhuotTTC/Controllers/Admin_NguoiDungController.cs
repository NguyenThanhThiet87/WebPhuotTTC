using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_NguoiDungController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Admin_NguoiDung/
        // Phương thức GET để hiển thị thông tin người dùng trong form chỉnh sửa
        public ActionResult EditNguoidung(int id)
        {
            // Tìm người dùng theo ID
            var nguoidung = database.NGUOIDUNGs.Where(row=>row.ID==id).FirstOrDefault();
            ViewBag.VaiTro = database.VAITROs.Select(row => row).ToList();
            // Trả về view _EditNguoidung và truyền đối tượng nguoidung để hiển thị thông tin hiện tại
            return PartialView(nguoidung);
        }

        // Phương thức POST để xử lý việc cập nhật thông tin người dùng
        [HttpPost]
        public ActionResult EditNguoidung(int id, FormCollection collection)
        {
                // Tìm người dùng theo ID
                var nguoidung = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                // Cập nhật dữ liệu từ form
                nguoidung.HoTen = collection["HoTen"];
                nguoidung.Email = collection["Email"];
                nguoidung.SDT = collection["SDT"];
                nguoidung.DiaChi = collection["DiaChi"];
                nguoidung.MaVaiTro = collection["MaVaiTro"];

                UpdateModel(nguoidung);
                // Cập nhật thay đổi vào CSDL
                database.SubmitChanges();   
                return RedirectToAction("NguoiDung","Admin");
        }

        public ActionResult DeleteNguoiDung(int id)
        {
            var nguoidung = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
            return PartialView(nguoidung);
        }
        [HttpPost]
        public ActionResult DeleteNguoiDung(int id, FormCollection collection)
        {
            var nguoidung = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
            database.NGUOIDUNGs.DeleteOnSubmit(nguoidung);
            database.SubmitChanges();
            return RedirectToAction("NguoiDung","Admin");
        }
    }
}