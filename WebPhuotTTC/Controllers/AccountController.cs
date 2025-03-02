using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;

namespace WebPhuotTTC.Controllers
{
    public class AccountController : Controller
    {
        DatabaseDataContext database=new DatabaseDataContext();
        //
        // GET: /Account/

        public ActionResult ItemAccountInHead()
        {
            var IDUSER = Session["IDUSER"];
            if(IDUSER!=null)
            {
                int id = int.Parse(IDUSER.ToString());
                var user1 = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                return PartialView(user1);
            }else
            {
                Console.WriteLine("Session đã xóa");
                return PartialView();
            }
        }
        public ActionResult ItemAccountInHeadAdmin()
        {
            var IDUSER = Session["IDAD"];
            if (IDUSER != null)
            {
                int id = int.Parse(IDUSER.ToString());
                var admin = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                return PartialView(admin);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        public ActionResult FormLogin()
        {
            return PartialView("_FormLoginPartialView");
        }

        [HttpPost]
        public JsonResult DangKy(FormCollection collection, NGUOIDUNG nguoidung)
        {
            try
            {
                var name = collection["name"];
                var email = collection["email"];
                var password = collection["password"];

                var exist_email=database.NGUOIDUNGs.Where(row => row.Email == email).FirstOrDefault();
                if(exist_email==null)
                {
                    nguoidung.HoTen = name;
                    nguoidung.Username = name;
                    nguoidung.Email = email;
                    nguoidung.Password = password;
                    nguoidung.MaVaiTro = "USE";
                    database.NGUOIDUNGs.InsertOnSubmit(nguoidung);
                    database.SubmitChanges();
                    return Json(new { success = true, message = "Đăng ký thành công" });
                }else
                {
                    return Json(new { success = false, message = "Đăng ký thất bại Vui lòng thử lại" });
                }
            }catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DangNhap(FormCollection collection)
        {
            var email = collection["email"];
            var password = collection["password"];
            NGUOIDUNG user = database.NGUOIDUNGs.Where(n => n.Email == email && n.Password == password).FirstOrDefault();
            if(user!=null)
            {
                
                if (user.MaVaiTro == "USE"){
                    Session["IDUSER"] = user.ID;
                    return Json(new { success = true, message = "Đăng nhập thành công", vaitro = "user" });
                } 
                else
                {
                    Session["IDAD"] = user.ID;
                    return Json(new { success = true, message = "Đăng nhập thành công", vaitro = "admin" });
                }
            }else
            {
                return Json(new { success = false, message = "Tài khoản hoặc mật khẩu không đúng",vaitro="" });
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index","User");
        }

        public ActionResult PersonalInfomation()
        {
            var IDUSER = Session["IDUSER"];
            if(IDUSER!=null)
            {
                int id = int.Parse(IDUSER.ToString());
                var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                if(user!=null)
                {
                    return View(user);
                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult UpdatePersonalInformation(UpdateUser.UserUpdateModel data)
        {

            var IDUSER = Session["IDUSER"];
            if (IDUSER != null)
            {
                int id = int.Parse(IDUSER.ToString());
                var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                if(user!=null)
                {
                    user.HoTen = data.HoTen;
                    user.SDT = data.SDT;
                    user.Email=data.Email;
                    user.Password=data.Password;
                    user.DiaChi=data.DiaChi;
                    user.Username = data.Username;

                    UpdateModel(user);
                    database.SubmitChanges();
                    return Json(new {success=true, message="Cập Nhật Thành Công"});
                }
            }
            return Json(new { success = false, message = "Không Thể Cập Nhật" });
        }
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        //phuong thuc gửi mã xác nhận qua email
        [HttpPost]
        public JsonResult SendEmail(string email)
       {
            var user = database.NGUOIDUNGs.Where(row => row.Email == email).FirstOrDefault();
            if (user == null)
                return Json(new { success = false, message = "Email chưa đăng ký tài khoản" });

            var maxacnhan = "";
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    Random random = new Random();
                    maxacnhan += random.Next(9);
                }

                var host = "smtp.gmail.com";
                var port =587;
                var fromEmail = "thanhthiet2004@gmail.com";
                var password = "gkzr xqss tjvi pqsf";
                var fromName ="TTC SHOP";
                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };
                var mail = new MailMessage
                {
                    Body = "Mã xác nhận của bạn là: "+maxacnhan,
                    Subject = "TTC SHOP",
                    From = new MailAddress(fromEmail, fromName)
                };
                mail.To.Add(new MailAddress(email));
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                smtpClient.Send(mail);

            }catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

           return Json(new { success = true, message = "Gửi mã xác nhận thành công", maxacnhan=maxacnhan });  
       }

        public ActionResult UpdateMK(FormCollection collection)
        {
            string email = collection["email"];
            string password = collection["new-password"];
            if(password==null || password=="")
                return new EmptyResult();
            var nguoidung = database.NGUOIDUNGs.Where(row => row.Email == email).FirstOrDefault();
            if(nguoidung!=null)
            {
                nguoidung.Password = password;
                UpdateModel(nguoidung);
                database.SubmitChanges();
                return RedirectToAction("Index", "User");
            }
            return new EmptyResult();
        }
	}
}