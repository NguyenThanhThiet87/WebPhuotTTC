function showStep(step) {
    document.querySelectorAll('.step').forEach(function (stepElement) {
        stepElement.classList.add('hidden');
    });
    document.getElementById('step' + step).classList.remove('hidden');
}
var GuiMaXacNhan=function(e)
{
    e.preventDefault();
    var email_input = document.getElementById("email");
    $.ajax({
        type: "POST",
        data: { email: email_input.value },
        url: "/Account/SendEmail",
        success: function(response)
        {
            if (response.success)
            {
                toastr.success(response.maxacnhan);
                console.log(response.message);
                document.querySelectorAll('.step').forEach(function (stepElement) {
                    stepElement.classList.add('hidden');
                });
                document.getElementById('step' + 2).classList.remove('hidden');
            }
            else {
                toastr.error(response.message);
                console.log(response.message);
            }
        },
        error: function (response) {
            toastr.error("lõi");
        }
    })
    
}