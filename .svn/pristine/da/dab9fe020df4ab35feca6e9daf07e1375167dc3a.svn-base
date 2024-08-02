var _ccDetails = "";

window.onload = function () {

    if ($("#lblCreationOfJo").text() == "No" && $("#lblTempAccess").text() !== $("#lblEmpID").text()) {
        $(".mdJODisable").modal("show");
        $('#createBtn')[0].disabled = true;

    }
    console.log($("#lblTempAccess").text());
    console.log($("#lblEmpID").text());

};

$(document).ready(function () {

    var lines = 7;
    var linesUsed = $('#display_count');
    var linesUsed_p = $('#display_count-p');

    $('#workDetailsID').keydown(function (e) {

        newLines = $(this).val().split("\n").length;

        if (e.keyCode == 13 && newLines >= lines) {
            linesUsed.css('color', 'red');
            linesUsed.text("New line limit exceeds, the system cannot proceed.");

            return false;
        }
        else {
            linesUsed.css('color', '');
        }
    });

    $('#purposeID').keydown(function (e) {

        newLines = $(this).val().split("\n").length;

        if (e.keyCode == 13 && newLines >= lines) {
            linesUsed_p.css('color', 'red');
            linesUsed_p.text("New line limit exceeds, the system cannot proceed.");

            return false;
        }
        else {
            linesUsed_p.css('color', '');
        }
    });
});

function submitJO() {

    $.ajax({
        url: '/JobOrders/SaveJobOrder',
        type: 'POST',
        data: JSON.stringify(
            {
                CostCenter: $("#txt-costcenter").val(),
                WorkDetails: $("#workDetailsID").val(),
                Purpose: $("#purposeID").val(),
                RequestedBy: $("#txtRequestorName").val(),
                NotedBy: $("#notedbyID").val(),
                ApprovedBy: $("#approvedbyID").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {

            $('#btn-submitjo')[0].disabled = true;
            $(".assignjo").modal("hide");
            $("#tblverlay").show();
        },
        success: function (JobNo) {
            $('#btn-submitjo')[0].disabled = false;
            window.location.href = "/JobOrders/JobOrderDetails?JoNumber=" + JobNo + "#AllJo";

        },
        complete: function () {

        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            alert("Ooppps! Something went wrong. \n" + msg);
        }
    });

}

function getDivision(ID) {

    if (ID.value == "") {
        $("#txt-costcenter").val("");
        $("#slct-sbu").addClass("is-invalid");
    }
    else {
        //Proceed this operation if ID has value
        $.ajax({
            url: '/JobOrders/GetSBUDivision',
            type: 'POST',
            data: JSON.stringify(
                {
                    SBUId: ID.value
                }),
            dataType: "json",
            traditional: true,
            contentType: 'application/json;',
            cache: false,
            beforeSend: function () {

            },
            success: function (Division) {
                var costcenter = $("#lbl-user-category").text() + "-" + Division["sbu"] + "-" + Division["category"] + "-";
                _ccDetails = costcenter;
                setCCDetails();
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert("Ooppps! Something went wrong. \n" + msg);
            }
        });
    }
}

function setCCDetails() {
    var section = $("#slct-sect").val();

    if (section == "") {
        $("#slct-sect").addClass("is-invalid");
        $("#txt-costcenter").val("");
    }
    else if ($("#slct-sbu").val() == "") {
        $("#slct-sbu").addClass("is-invalid");
        $("#txt-costcenter").val("");
    }
    else {
        var completecc = _ccDetails + section;
        $("#txt-costcenter").val(completecc);

        $("#slct-sbu").removeClass("is-invalid");
        $("#slct-sect").removeClass("is-invalid");
    }
}
