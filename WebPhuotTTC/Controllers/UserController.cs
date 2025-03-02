using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;
using WebPhuotTTC.ViewModel;
namespace WebPhuotTTC.Controllers
{
    public class UserController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nav()
        {
            var NhomSP = from row in database.NHOMSANPHAMs select row;
            return PartialView("_NavPartialView", NhomSP);
        }
        public ActionResult DropListMenuItem(string MaNhom)
        {
            var data = from row in database.LOAISANPHAMs where row.MaNhom == MaNhom select row;
            return PartialView( "_DropListMenuItemPartialView",data);
        }
        public ActionResult GoogleMap()
        {
            return PartialView("_GoogleMapPartialView");
        }     
	}
}