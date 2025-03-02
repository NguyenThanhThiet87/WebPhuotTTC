using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhuotTTC.Models;
namespace WebPhuotTTC.Models
{
    public class DonHangDetail
    {
        public DONHANG donhang { get; set; }
        public List<CHITIETDONHANG> chitietdonhang{get; set;}
    }
}