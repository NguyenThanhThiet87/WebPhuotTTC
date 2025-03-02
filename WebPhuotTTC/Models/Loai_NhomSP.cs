using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhuotTTC.Models;

namespace WebPhuotTTC.Models
{
    public class Loai_NhomSP
    {
        public LOAISANPHAM loaisp { get; set; }
        public List<NHOMSANPHAM> nhomsp { get; set; }
    }
}