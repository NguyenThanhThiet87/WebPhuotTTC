//Hiển thị form đăng nhập
var DisplaySignIn = function () {
    var signInContainer = document.getElementsByClassName("account-frame")[0];
    signInContainer.classList.toggle("hide_element");
}
/*Toggle Form Login*/
const container = document.getElementsByClassName("auth-container")[0];
const regisBtn = document.getElementById("sign-up-btn");
const logInBtn = document.getElementById("sign-in-btn");
regisBtn.addEventListener("click", function () {
    container.classList.add("active");
})
logInBtn.addEventListener("click", function () {
    container.classList.remove("active");
})

//Gửi dữ liệu của form đăng ký qua controller để xử lý mà không tải lại trang
document.getElementById("form-sign-up").addEventListener("submit",function(e)
{
    e.preventDefault(); //ngăn form chuyển đến một đường dẫn và reload
    var input = document.querySelectorAll("#form-sign-up input");
    var formData = {
        name: input[0].value,
        email: input[1].value,
        password: input[2].value
    };

    $.ajax({
        type: "POST", //phương thức gửi
        url: '/Account/DangKy', // Gửi dữ liệu đến controller qua Ajax
        data: formData, //dữ liệu gửi đi
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message);
                input[0].value = " ";
                input[1].value = " ";
                input[2].value = " ";
                document.getElementsByClassName("account-container")[0].classList.remove("active");
            } else {
                toastr["error"](response.message);
            }
        },
        error: function () {
            toastr["error"](response.message);
        }
    });
})
//Gửi dữ liệu của form đăng nhập qua controller để xử lý mà không tải lại trang
document.getElementById("form-sign-in").addEventListener("submit", function (e) {
    e.preventDefault(); //ngăn form chuyển đến một đường dẫn và reload
    var input = document.querySelectorAll("#form-sign-in input");
    var formData = {
        email: input[0].value,
        password: input[1].value
    };

    $.ajax({
        type: "POST", //phương thức gửi
        url: '/Account/DangNhap', // Gửi dữ liệu đến controller qua Ajax
        data: formData, //dữ liệu gửi đi
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message);
                input[0].value = " ";
                input[1].value = " ";
                if(response.vaitro=="user")
                {
                    window.location.href = '/User/Index';
                }
                else if(response.vaitro=="admin")
                    window.location.href = '/Admin/Home';
            } else {
                toastr["error"](response.message);
            }
        },
        error: function () {
            toastr["error"](response.message);
        }
    });
})
