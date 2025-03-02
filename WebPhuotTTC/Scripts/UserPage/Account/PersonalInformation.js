var editBtn = document.getElementById("editBtn");
var saveBtn = document.getElementById("saveBtn");
editBtn.addEventListener("click", function (e) {
    e.target.classList.add("hide-element");
    saveBtn.classList.remove("hide-element");
    var inputs = document.querySelectorAll(".personal_information input");
    console.log(inputs);
    // Xóa thuộc tính readonly
    inputs.forEach(function (item) {
        item.removeAttribute("readonly");
    });
})
saveBtn.addEventListener("click", function (e) {
    e.target.classList.add("hide-element");
    editBtn.classList.remove("hide-element");
    var inputs = document.querySelectorAll(".personal_information input");
    var data = {
        HoTen: inputs[0].value,
        SDT: inputs[1].value,
        DiaChi: inputs[2].value,
        Username: inputs[3].value,
        Password: inputs[4].value,
        Email: inputs[5].value
    }

    inputs.forEach(function (item) {
        item.setAttribute("readonly",true);
    });

    $.ajax({
        type: "POST", //phương thức gửi
        url: '/Account/UpdatePersonalInformation', // Gửi dữ liệu đến controller qua Ajax
        data: data, //dữ liệu gửi đi
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message);
            } else {
                toastr["error"](response.message);
            }
        },
        error: function () {
            toastr["error"](response.message);
        }
    });
})