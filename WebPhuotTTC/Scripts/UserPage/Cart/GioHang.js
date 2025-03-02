var DisplayCartShort = function () {
    var cart_container = document.getElementsByClassName("cart-container")[0];
    console.log(cart_container)
    cart_container.classList.add("hide_element");
}
var GiamSoLuong = function (button) {
    const input = button.nextElementSibling; // Lấy thẻ input liền kề trước đó
    if (input.value > 1) {
        input.value = parseInt(input.value) - 1; // Giảm số lượng, không cho phép dưới 0
    }
    // Kích hoạt sự kiện onchange
    input.dispatchEvent(new Event('change'));
}
var TangSoLuong = function (button) {
    const input = button.previousElementSibling; // Lấy thẻ input liền kề trước đó
    input.value = parseInt(input.value) + 1; // Tăng số lượng
    // Kích hoạt sự kiện onchange
    input.dispatchEvent(new Event('change'));
}
//lưu lại số lượng của sản phẩm trong giỏ hàng
var LuuSoLuong = function (input, maSP, maKT, maMau) {
    //const input = button.previousElementSibling; // Lấy thẻ input liền kề trước đó
    console.log(maSP + maKT + maMau)
    var dataPost = {
        maSP: maSP,
        maKT: maKT,
        maMAU: maMau,
        SoLuong: input.value
    }
    $.ajax({
        type: "POST",
        url: '/Cart/UpdateSoLuongItemGioHang',
        data: dataPost,
        success: function (response) {
            if (!response.success)
                toastr.warning("Thất bại");
        },

    })
}
//xóa sản phẩm trong giỏ hàng
var xoaSPGioHang = function (maSanPham, maKichThuoc, maMau) {
    var dataForm = {
        maSP: maSanPham,
        maKT: maKichThuoc,
        maMAU: maMau
    }
    $.ajax({
        type: "POST",
        data: dataForm,
        url: "/Cart/XoaItemGioHang",
        success: function (response) {
            if (response.success) {
                toastr['success'](response.message);
                $('#cart-menu').load('/cart/GioHangItemMenu');
            }
        },
        error: function (ex) {
            toastr.warning(ex);
        }
    })
}
//Hiển thị giỏ hàng
$('#cart-menu').off('click').on('click', function (event) {
    var cart_container = document.getElementsByClassName("cart-container")[0];
    if (event.target == document.getElementById("cart-menu") || event.target == document.querySelector("#cart-menu > i") || event.target == document.querySelector("#cart-menu > span"))
        cart_container.classList.toggle("hide_element");
});

var checkEmptyItemSP = function () {
    var flag = true;
    var inputCheck = document.getElementsByClassName("product-checkbox");
    for (let i = 0; i < inputCheck.length; i++) {
        if (inputCheck[i].checked) {
            flag = false;
            break;
        }
    }
    var dathangbtn=document.getElementById("dathangbtn");
    if (flag) {
        dathangbtn.setAttribute("disabled", true);
    } else {
        dathangbtn.removeAttribute("disabled");
    }
}