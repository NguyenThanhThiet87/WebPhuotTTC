//Config Toastr
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-center", // Vị trí hiển thị thông báo
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300", // Thời gian hiển thị hiệu ứng
    "hideDuration": "1000", // Thời gian ẩn hiệu ứng
    "timeOut": "5000", // Thời gian thông báo tự động ẩn
    "extendedTimeOut": "1000", // Thời gian mở rộng khi hover vào thông báo
    "showEasing": "swing", // Hiệu ứng hiển thị
    "hideEasing": "linear", // Hiệu ứng ẩn
    "showMethod": "fadeIn", // Phương thức hiển thị
    "hideMethod": "fadeOut", // Phương thức ẩn
    "opacity": 1
};

var DisplayDropListMenuItem = function (MaNhom)
{
    var element = document.getElementById(MaNhom);
    var parent = element.parentElement;
    var handleHide = function (e) {
        if (!parent.contains(e.target)) {
            element.innerHTML = "";
            document.removeEventListener("click",handleHide);
        }
    };
    document.addEventListener("click", handleHide);
}

var DisplayGoogleMap=function()
{
    var ggmapElement = document.getElementsByClassName("google_map")[0];
    ggmapElement.classList.toggle("hide_element");
}




var searchBtn = document.querySelector(".search-box > input");

searchBtn.addEventListener("change", function () {
    var data = {
        key: searchBtn.value
    }
    $.ajax({
        type: "POST", //phương thức gửi
        url: '/Product/Search', // Gửi dữ liệu đến controller qua Ajax
        data: data, //dữ liệu gửi đi
        success: function (response) {
            var container_result = document.getElementById("search-result");
            container_result.innerHTML = response;
        },
        error: function () {
            toastr["error"](response);
        }
    });
    var searchBox = document.getElementsByClassName("search-box")[0];
    document.addEventListener("click", function (event) {
        if (!searchBox.contains(event.target)) {
            var container_result = document.getElementById("search-result");
            container_result.innerHTML = "";
        }
    })
});
