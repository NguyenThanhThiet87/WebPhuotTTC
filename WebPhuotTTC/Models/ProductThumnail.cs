using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebPhuotTTC.Models;
namespace WebPhuotTTC.Models
{
    public class ProductThumnail
    {
        public SANPHAM product { get; set; }
        public GIAMGIA discount { get; set; }
        public float SaoTB { get; set; }
    }
}