


function login() {

    var empid = $("#empidTxt").val();
    var password = $("#password-input").val();

    $.ajax({
        url: '/Home/Login',
        type: 'POST',
        data: JSON.stringify(
            {
                EmpID: empid,
                Password: password
            }),
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        cache: false,
        beforeSend: function () {
            $("#spinnerLoader").show();
        },
        success: function (flag) {

            if (flag == "1") {
                $("#spinnerLoader").hide();
                window.location.href = "/Dashboard/Dashboard?EmpID=" + empid + "#dashboard";
            } else if (flag == "2") {
                $("#spinnerLoader").hide();
                window.location.href = "/JobOrders/AllUnassignedJO#unassignedJo";
            } else if (flag == "gris") {
                $("#spinnerLoader").hide();
                window.location.href = "/Home/Canteen";
            }
            else {
                $("#spinnerLoader").hide();
                $("#errorBadge").show();
            }
        }
    });
}

function show_hide_password(target) {
    var input = document.getElementById('password-input');
    if (input.getAttribute('type') == 'password') {
        target.classList.add('view');
        input.setAttribute('type', 'text');
    } else {
        target.classList.remove('view');
        input.setAttribute('type', 'password');
    }
    return false;
}

$(function () {

    $("#password-input").keyup(function (e) {
        if (e.which == 13) {
            login();
        }
    });
});