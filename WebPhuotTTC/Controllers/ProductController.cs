using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPhuotTTC.Models;
using WebPhuotTTC.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace WebPhuotTTC.Controllers
{
    public class ProductController : Controller
    {
        DatabaseDataContext database = new DatabaseDataContext();
        //
        // GET: /Product/
        private ProductDetail GetProductDetail(string MaSP)
        {
            // Lấy sản phẩm
            var Product = database.SANPHAMs.Where(row => row.MaSanPham == MaSP).FirstOrDefault();
            if (Product == null) return null; // Kiểm tra nếu sản phẩm không tồn tại
            // Lấy những thông tin chi tiết về sản phẩm
            var DetailProduct = database.QLSANPHAMs.Where(row => row.MaSanPham == MaSP).Select(row => row).ToList();
            // Lấy tất cả ảnh và ảnh bìa về sản phẩm
            var Images=database.HINHANHs.Where(row=>row.MaSanPham==MaSP).Select(row=>row).ToList();
            if (Product != null)
                Images.Insert(0, new HINHANH{Url=Product.AnhBia});
            // Lấy Thông tin về giảm giá dành cho sản phẩm
            var maDiscount=database.GIAMGIA_SANPHAMs.Where(row=>row.MaSanPham==MaSP).FirstOrDefault();
            GIAMGIA Discount;
            if(maDiscount!=null)
            {
                Discount = database.GIAMGIAs.Where(row => row.MaGiamGia == maDiscount.MaGiamGia).FirstOrDefault();
            }else
            {
                Discount = new GIAMGIA
                {
                    TiLeGiamGia = 0,
                    MaGiamGia=null
                };
            }
            if(DetailProduct==null)
            {
                DetailProduct = new List<QLSANPHAM>();
                DetailProduct.Add(new QLSANPHAM
                {
                    MaMau="null",
                    MaKichThuoc="null",
                    MaSanPham=MaSP
                });
            }
            // Lấy thông tin về các bình luận trên sản phẩm
            var Comment=database.BINHLUANs.Where(row=>row.MaSanPham==MaSP).Select(row=>row).ToList();
            var data = new ProductDetail
            {
                product = Product,
                detailProduct = DetailProduct,
                images = Images,
                discount = Discount,
                comment = Comment
            };

            return data;
        }
        public ActionResult Detail(String MaSP)
        {
            var data = GetProductDetail(MaSP);
            ViewBag.TongDanhGia = TrungBinhDanhGia(MaSP);
            ViewBag.ThongKeDanhGia = ThongKeDanhGia(MaSP);
            return View(data);
        }
        private List<ProductThumnail> GetProduct(string MaLoai)
        {
            var data = new List<ProductThumnail>();
            var allproductML = database.SANPHAMs.Where(row => row.MaLoai == MaLoai).Select(row => row).ToList();
            for (var i = 0; i < allproductML.Count; i++)
            {
                var MaDiscount = database.GIAMGIA_SANPHAMs.Where(row => row.MaSanPham == allproductML[i].MaSanPham).FirstOrDefault();
                var discount = MaDiscount != null ? database.GIAMGIAs.Where(row => row.MaGiamGia == MaDiscount.MaGiamGia).FirstOrDefault() : null;
                float SoSao = TrungBinhDanhGia(allproductML[i].MaSanPham);
                data.Add(new ProductThumnail
                {
                    product = allproductML[i],
                    discount = discount,
                    SaoTB = SoSao == 0 ? 5 : SoSao
                });
            }
            return data;
        }
        public ActionResult ProductByCategory(string MaLoai, String sort, int ?page)
        {
            var listSP = GetProduct(MaLoai);
            switch(sort)
            {
                case "increase":
                    listSP.Sort((item1, item2) =>
                    {
                        double gia1 = item1.product.Gia;
                        double gia2 = item2.product.Gia;
                        if (item1.discount != null)
                            gia1 = gia1 - (gia1 / 100 * item1.discount.TiLeGiamGia ?? 0);
                        if (item2.discount != null)
                            gia2 = gia2 - (gia2 / 100 * item2.discount.TiLeGiamGia ?? 0);
                        return gia1.CompareTo(gia2);
                    });
                    break;
                case "descrease":
                    listSP.Sort((item1, item2) =>
                    {
                        double gia1 = item1.product.Gia;
                        double gia2 = item2.product.Gia;
                        if (item1.discount != null)
                            gia1 = gia1 - (gia1 / 100 * item1.discount.TiLeGiamGia ?? 0);
                        if (item2.discount != null)
                            gia2 = gia2 - (gia2 / 100 * item2.discount.TiLeGiamGia ?? 0);
                        return gia2.CompareTo(gia1);
                    });
                    break;
                default:
                    listSP.Sort((item1, item2) =>
                    {
                        double tilegiam1 = 0;
                        double tilegiam2 = 0;
                        if (item1.discount != null)
                            tilegiam1 = item1.discount.TiLeGiamGia ?? 0;
                        if (item2.discount != null)
                            tilegiam2 = item2.discount.TiLeGiamGia ?? 0;
                        return tilegiam2.CompareTo(tilegiam1);
                    });
                    break;
            }
            ViewBag.MaLoai = MaLoai;
            ViewBag.TongSP = listSP.Count;
            ViewBag.Sort=sort;
            int pageNum = page ?? 1;
            return View(listSP.ToPagedList(pageNum, 15));
        }
        public ActionResult ProductListGrid(String sort, List<ProductThumnail> listSP)
        {
            listSP.Sort((item1, item2) =>
            {
                double gia1 = item1.product.Gia;
                double gia2 = item2.product.Gia;
                if (item1.discount != null)
                    gia1 = gia1-(gia1/100 * item1.discount.TiLeGiamGia ?? 0);
                if (item2.discount != null)
                    gia2 = gia2 - (gia2 / 100 * item2.discount.TiLeGiamGia ?? 0);
                return gia1.CompareTo(gia2);
            });
            ViewBag.TongSP = listSP.Count;
            return PartialView(listSP);
        }

        [HttpPost]
        public ActionResult SubmitComment(FormCollection collection, string MaSP, string Url)
        {
            var sanpham = database.SANPHAMs.Where(row => row.MaSanPham == MaSP).FirstOrDefault();
            var IDUSER = Session["IDUSER"];

            if(IDUSER!=null)
            {
                int id = int.Parse(IDUSER.ToString());
                var user = database.NGUOIDUNGs.Where(row => row.ID == id).FirstOrDefault();
                int SoSao = int.Parse(collection["SoSao"]);
                BINHLUAN comment = new BINHLUAN();
                comment.ID = id;
                comment.MaSanPham = sanpham.MaSanPham;
                comment.ThoiGian = DateTime.Now;
                comment.NoiDung = collection["comment"];
                comment.SoSao = SoSao;

                database.BINHLUANs.InsertOnSubmit(comment);
                database.SubmitChanges();
                TempData["ThongBao"] = "Bạn đã bình luận";
                return Redirect(Url);
            }
            return null;
        }
        [HttpPost]
        public ActionResult Search(String key)
        {
            var result = database.SANPHAMs.Where(row => row.TenSanPham.Contains(key)).Select(row => row).OrderByDescending(row => row.TenSanPham.StartsWith(key)).ThenBy(row => row.Gia).ToList();

            return PartialView("Search",result);
        }

        public ActionResult ProductAdvanced()
        {
            return PartialView();
        }

        public ActionResult ProductBanner(String maSP)
        {
            var data = GetProduct(maSP);
            return PartialView("_ProductBannerPartialView", data);
        }
        
        private int TrungBinhDanhGia(String maSP)
        {
            var danhgia=database.BINHLUANs.Where(row => row.MaSanPham == maSP).Select(row => row).ToList();
            var sum = 5;
            if(danhgia.Count>0)
            {
                sum = danhgia.Sum(row => row.SoSao);
                return sum / danhgia.Count;
            }
            return sum; 
        }
        private int[] ThongKeDanhGia(String maSP)
        {
            int[] thongke = {0,0,0,0,0};
            var danhgia = database.BINHLUANs.Where(row => row.MaSanPham == maSP).Select(row => row).ToList();
            thongke[0]=danhgia.Where(x => x.SoSao == 1).Count();
            thongke[1] = danhgia.Where(x => x.SoSao == 2).Count();
            thongke[2] = danhgia.Where(x => x.SoSao == 3).Count();
            thongke[3] = danhgia.Where(x => x.SoSao == 4).Count();
            thongke[4] = danhgia.Where(x => x.SoSao == 5).Count();
            for (int i = 0; i < thongke.Length; i++)
            {
                if (thongke[i] != 0)
                {
                    thongke[i] = (thongke[i] / danhgia.Count) * 100;
                }
            }
            return thongke;
        }
	}
}