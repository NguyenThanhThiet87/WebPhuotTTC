var detailproduct = document.querySelector(".product-detail .product-image .gallery-container");
detailproduct.addEventListener("mouseover",function()
{
    detailproduct.classList.add("hover");
    detailproduct.addEventListener("mouseout",function()
    {
        detailproduct.classList.remove("hover");
    })
})
//swiper library config
var swiper = new Swiper(".image-gallery", {
    slidesPerView: 3,
    spaceBetween: 20,
    freeMode: true,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
});
//thay đổi ảnh bìa
var ImageClicked=function(e)
{
    var main_image = document.querySelector(".product-detail #main-image");
    main_image.setAttribute("src", e.target.src);
}
$(document).ready(function () {
    //sự kiện cho các nút màu và mặc định là nút đầu
    var colors = document.querySelectorAll(".colors > input");
    for (let item = 0; item < colors.length; item++) {
        if (item == 0) {
            colors[item].classList.add("selected");
            colors[item].setAttribute("name", "color");
        }
        colors[item].addEventListener("click", function () {
            for (let i = 0; i < colors.length; i++) {
                colors[i].classList.remove("selected");
                colors[i].removeAttribute("name");
            }
            colors[item].classList.add("selected");
            colors[item].setAttribute("name", "color");
        })
    }
    //sự kiện cho các nút kích thước và mặc định là nút đầu
    var sizes = document.querySelectorAll(".sizes > input");
    for (let item = 0; item < colors.length; item++) {
        if (item == 0) {
            sizes[item].classList.add("selected");
            sizes[item].setAttribute("name", "size");
        }
        sizes[item].addEventListener("click", function () {
            for (let i = 0; i < sizes.length; i++) {
                sizes[i].classList.remove("selected");
                sizes[i].removeAttribute("name");
            }
            sizes[item].classList.add("selected");
            sizes[item].setAttribute("name", "size");
        })
    }

});
//Mua Ngay
var muaNgay = function (MaSP) {
    var color = document.getElementsByName("color")[0];
    var size = document.getElementsByName("size")[0];
    var dataForm = {
        maSP: MaSP,
        maKT: size.value,
        maMAU: color.value
    }
    $.ajax({
        type: 'POST',
        data: dataForm,
        url: '/Cart/ThemVaoGioHang',
        success: function (response) {
            if (response.success) {
                toastr['success'](response.message);
                $('#cart-menu').load('/cart/GioHangItemMenu');
            }
        },
        error: function () {
            
        }
    })
}
//lựa chọn đánh giá
var starts = document.querySelectorAll(".star");
for (let i = 0; starts.length; i++)
{
    starts[i].addEventListener("click", function () {
        for(let j=0;j<starts.length;j++)
        {
            if (j <= i)
                starts[j].classList.add("selected");
            else
                starts[j].classList.remove("selected");
        }
        var input = document.querySelector(".star-rating > input");
        input.value = i+1;
    })
}
