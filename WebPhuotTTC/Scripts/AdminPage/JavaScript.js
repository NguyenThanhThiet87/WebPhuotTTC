var ShowDownListItem = function () {
    var fa_chevron = document.querySelector(".item_manage > a > i ");

    if (fa_chevron.classList.contains("fa-chevron-right")) {
        fa_chevron.classList.remove("fa-chevron-right");
        fa_chevron.classList.add("fa-chevron-left")
    } else {
        fa_chevron.classList.remove("fa-chevron-left");
        fa_chevron.classList.add("fa-chevron-right")
    }

    var submenus = document.getElementsByClassName("submenu_item");
    for (let i = 0; i < submenus.length; i++)
    {
        submenus[i].classList.toggle("hide_element");
    }
}

var CloseDialog = function () {
    var close_btn = document.getElementById("dialog");
    close_btn.innerHTML="";
}

