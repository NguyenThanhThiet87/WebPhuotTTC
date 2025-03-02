using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Controllers
{
    public class Admin_DonHangController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        // GET: Admin_DonHang
        public ActionResult DetailDonHang(string id)
        {
            var donhang = database.DONHANGs.Where(row => row.MaDonHang == id).FirstOrDefault();
            var chitietdonhang = database.CHITIETDONHANGs.Where(row => row.MaDonHang == id).Select(row => row).ToList();
            DonHangDetail detail = new DonHangDetail { 
                donhang = donhang,
                chitietdonhang = chitietdonhang
            };
            return View(detail);
        }
        public ActionResult EditDonHang(string id)
        {
            var donhang = database.DONHANGs.Where(row => row.MaDonHang == id).FirstOrDefault();
            ViewBag.TrangThai = database.TRANGTHAIs.Select(row => row).ToList();
            return PartialView(donhang);
        }
        [HttpPost]
        public ActionResult Edit(string MaDonHang, FormCollection collection)
        {
            if (MaDonHang == null)
                return new EmptyResult();
            var donhang = database.DONHANGs.Where(row => row.MaDonHang == MaDonHang).First();
            if (donhang == null || donhang.ID == null)
                return new EmptyResult();
            donhang.MaTrangThai = int.Parse(collection["MaTrangThai"]);
            UpdateModel(donhang);
            database.SubmitChanges();
            return RedirectToAction("DonHang", "Admin");
        }
       
        public ActionResult DeleteDonHang(string id)
        {
            var donhang = database.DONHANGs.Where(row => row.MaDonHang == id).FirstOrDefault();
            return PartialView(donhang);
        }
        [HttpPost]
        public ActionResult DeleteDonHang(string id,FormCollection collection)
        {
            var chitietdonhang = database.CHITIETDONHANGs.Where(row => row.MaDonHang == id).Select(row => row).ToList();
            database.CHITIETDONHANGs.DeleteAllOnSubmit(chitietdonhang);

            var donhang = database.DONHANGs.Where(row => row.MaDonHang == id).FirstOrDefault();
            database.DONHANGs.DeleteOnSubmit(donhang);
            database.SubmitChanges();
            return RedirectToAction("DonHang", "Admin");
        }
    }
}