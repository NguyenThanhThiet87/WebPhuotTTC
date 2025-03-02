using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPhuotTTC.Models;
namespace WebPhuotTTC.Models
{
    public class ProductDetail
    {
        public SANPHAM product{get;set;}
        public List<QLSANPHAM> detailProduct { get; set; }
        public List<HINHANH> images { get; set; }
        public GIAMGIA discount { get; set; }
        public List<BINHLUAN> comment { get; set; }
    }
}