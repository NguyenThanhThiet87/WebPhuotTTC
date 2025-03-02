var changeActive = function (e, listDH) {
    var tab = document.getElementsByClassName("tab");
    for (let i = 0; i < tab.length; i++) {
        tab[i].classList.remove("active");
    }
    e.classList.add("active");
    var sortStr = e.textContent;
    alert(listDH)
    //    $(".donhangdadat-container").first().load("/Cart/ListDonHang", { sort: sortStr, listSP:listDH });
}