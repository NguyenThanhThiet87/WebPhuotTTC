$.get("/Admin/TongDoanhThu", function (response) {
    const ctx = document.getElementById('myChart');
    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Mũ Fullface', 'Mũ 1/2', 'Mũ 3/4', 'Áo giáp', 'Phụ kiện'],
            datasets: [{
                label: 'Đã bán',
                data: response,
                borderWidth: 1
            }]
        },
        options: {

            plugins: {
                legend: {
                    position: 'bottom',
                },
                title: {
                    display: true,
                    text: 'Biểu đồ sản phẩm bán ra',
                    position: "bottom"
                }
            }
        }
    });
})
$.get("/Admin/TongDoanhThu", function (response) {
    const ctx = document.getElementById('chartDH');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7','Chủ nhật'],
            datasets: [{
                label: 'Đã bán',
                data: [20,25,10,40,12,100,50],
                borderWidth: 1
            }]
        },
        options: {
            plugins: {
                legend: {
                    position: 'bottom',
                },
                title: {
                    display: true,
                    text: 'Biểu đồ hóa đơn theo tuần',
                    position: "bottom"
                }
            }
        }
    });
})
