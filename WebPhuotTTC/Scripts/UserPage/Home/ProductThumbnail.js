//swiper library config
var swiper = new Swiper(".product_banner", {
    slidesPerView: 5,
    grid: {
        rows: 2, // Số hàng (Rows)
        fill: 'row', // Điều này kiểm soát cách các slide được lấp đầy
    },
    spaceBetween: 20,
    freeMode: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    }
});
