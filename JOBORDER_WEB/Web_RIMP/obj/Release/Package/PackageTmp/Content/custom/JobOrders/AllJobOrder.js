$(function () {
    $("#txt-excel-start").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#txt-excel-end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});

function gotoAllSoftware() {

    document.getElementById("jo-type-sw").style.backgroundColor = "red";
    document.getElementById("jo-type-hw").style.backgroundColor = "black";

    $("#dv-unassigned").hide();
    $("#hwID").hide();
    $("#swID").show();

}

function gotoAllHardware() {

    document.getElementById("jo-type-hw").style.backgroundColor = "red";
    document.getElementById("jo-type-sw").style.backgroundColor = "black";
    $("#dv-unassigned").hide();
    $("#swID").hide();
    $("#hwID").show();
}


function displayLoader() {
    $("#tblverlay").show();
    setTimeout(function () {
        $("#tblverlay").hide();
    }, 2000);
}

function exportToExcel() {

    var Department = $("#txtUser").val();
    var ID = $("#txt-EmpID").val();
    if (Department.trim() !== "ASIA PACIFIC CENTER" && ID.trim() != "g101756")
        alert("Access denied, please contact APC Department");
    else {
        var txtdatefrom = $("#txt-excel-start").val();
        var txtdateto = $("#txt-excel-end").val();

        if (txtdatefrom !== "" && txtdateto !== "") {
            if (new Date(txtdatefrom) > new Date(txtdateto))
                alert("Invalid date range.");
            else {
                var url = "/JobOrders/ExportToExcelJO?DateFrom=" + txtdatefrom + "&DateTo=" + txtdateto;
                window.location.href = url;
            }
        } else
            alert("Please add date range.");
    }
}

function joSearch(param) {

    if (param == 'hw') {

        var txtsearch = $("#txt-hw-search").val();
        var table = document.getElementById("tbl-hw");
        $('#tb-hw').empty();

        $.ajax({
            url: '/JobOrders/SearchHWJobOrder',
            type: 'POST',
            data: JSON.stringify(
                {
                    TextSearh: txtsearch
                }),
            dataType: "json",
            traditional: true,
            contentType: 'application/json;',
            cache: false,
            success: function (Result) {

                for (var i in Result) {
                    var rownum = table.insertRow(1);

                    var cell1 = rownum.insertCell(0);
                    var cell2 = rownum.insertCell(1);
                    var cell3 = rownum.insertCell(2);
                    var cell4 = rownum.insertCell(3);
                    var cell5 = rownum.insertCell(4);
                    var cell6 = rownum.insertCell(5);
                    var cell7 = rownum.insertCell(6);

                    cell1.innerHTML = Result[i].CodeNumber;
                    cell2.innerHTML = Result[i].Type;
                    cell3.innerHTML = Result[i].Details;
                    cell4.innerHTML = Result[i].Status;
                    cell5.innerHTML = Result[i].StandardTime;
                    cell6.innerHTML = Result[i].Assignee;
                    cell7.innerHTML = "<a href = '/JobOrders/JobOrderDetails?JoNumber=" + Result[i].JoNumber + "#AllJo' class = 'btn btn-outline-warning'><i class = 'fa fa-pencil-square'></i></a>";
                }

            },
        });
    } else {
        var txtsearch = $("#txt-sw-search").val();
        var table = document.getElementById("tbl-sw");
        $('#tb-sw').empty();

        $.ajax({
            url: '/JobOrders/SearchSWJobOrder',
            type: 'POST',
            data: JSON.stringify(
                {
                    TextSearh: txtsearch
                }),
            dataType: "json",
            traditional: true,
            contentType: 'application/json;',
            cache: false,
            success: function (Result) {

                for (var i in Result) {
                    var rownum = table.insertRow(1);

                    var cell1 = rownum.insertCell(0);
                    var cell2 = rownum.insertCell(1);
                    var cell3 = rownum.insertCell(2);
                    var cell4 = rownum.insertCell(3);
                    var cell5 = rownum.insertCell(4);
                    var cell6 = rownum.insertCell(5);

                    cell1.innerHTML = Result[i].CodeNumber;
                    cell2.innerHTML = Result[i].AppName;
                    cell3.innerHTML = Result[i].Status;
                    cell4.innerHTML = Result[i].Status;
                    cell5.innerHTML = Result[i].Assignee;
                    cell6.innerHTML = "<a href = '/JobOrders/JobOrderDetails?JoNumber=" + Result[i].JoNumber + "#AllJo' class = 'btn btn-outline-warning'><i class = 'fa fa-pencil-square'></i></a>";
                }
            },
        });
    }

}