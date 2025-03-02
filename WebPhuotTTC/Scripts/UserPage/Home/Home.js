//Display next and previous button when hover
document.getElementById("banner-bottom").addEventListener("mouseover", function () {
    var element = document.getElementById("banner-bottom");
    element.classList.add("hover");
    element.addEventListener("mouseout", function (event) {
        element.classList.remove("hover");
    })
})

document.getElementById("product-advanced-images").addEventListener("mouseover", function () {
    var element = document.getElementById("product-advanced-images");
    element.classList.add("hover");
    element.addEventListener("mouseout", function (event) {
        element.classList.remove("hover");
    })
})


    document.addEventListener('DOMContentLoaded', function() {
        var carousel = document.getElementById('product-advanced-images');

        carousel.addEventListener('slid.bs.carousel', function () {
            // Lấy ra mục hiện tại đang hiển thị
            var activeItem = document.querySelector('.carousel-item.active');

            // Xóa lớp hoạt ảnh cũ nếu có
            activeItem.classList.remove('animate__animated', 'animate__fadeInTopRight');

            // Thêm lại lớp hoạt ảnh khi chuyển đến mục mới
            setTimeout(function () {
                activeItem.classList.add('animate__animated', 'animate__fadeInTopRight');
            }, 10); // Đặt nhỏ thời gian chờ để chắc chắn rằng lớp hoạt ảnh được thêm sau khi chuyển
        });
    });
