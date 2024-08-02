//Global variables
_arrpreventive = [];
_jsonpreventive = {};
_preventive_chart_standard = [];
_preventive_chart_actual   = [];
_preventive_chart_month    = [];

$(function () {
    $("#txt-month").datepicker({ dateFormat: 'MM-yy' }).val();
    $("#txt-actual-date").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});


window.onload = function () {

    var count = $("#lbl-count").text();

    for (var i = 0; i < count; i++) {

        var actual = $("#lbl-actual-" + i).text();
        var standard = $("#lbl-standard-" + i).text();
        var month = $("#lbl-month-" + i).text();

        _preventive_chart_actual.push(actual);
        _preventive_chart_standard.push(standard);
        _preventive_chart_month.push(month);
    }

    console.log(_preventive_chart_standard);
    console.log(_preventive_chart_actual);

    displayChart();
};

function getAllPC() {

    if ($("#txt-month").val() == "" || $("#txt-pcnames").val() == "")
        $(".error-preventive-required").show();
    else {

        var table = document.getElementById("tbl-new-preventive");
        $('#tb-new-preventive').empty();

        $.ajax({
            url: '/Preventive/GetAllComputers',
            type: 'POST',
            data: JSON.stringify({
                FilePath: $("#txt-pcnames").val(),
                Plan: $("#txt-month").val(),
            }),
            dataType: "json",
            traditional: true,
            contentType: 'application/json;',
            cache: false,
            beforeSend: function() {
                $("#tbloverlay").show();
            },
            success: function (Preventive) {

                for (var i = 0; i < Preventive.length; i++) {
                    var rownum = table.insertRow(1);

                    var cell1 = rownum.insertCell(0);
                    var cell2 = rownum.insertCell(1);
                    var cell3 = rownum.insertCell(2);
                    var cell4 = rownum.insertCell(3);
                    var cell5 = rownum.insertCell(4);

                    cell1.innerHTML = Preventive[i].ComputerName;
                    cell2.innerHTML = Preventive[i].Equipment;
                    cell3.innerHTML = Preventive[i].Department;
                    cell4.innerHTML = Preventive[i].PCOwner;
                    cell5.innerHTML = Preventive[i].Incharge;

                    _jsonpreventive = {
                        "pcname"    : Preventive[i].ComputerName,
                        "equipment":  Preventive[i].Equipment,
                        "owner": Preventive[i].PCOwner,
                        "department": Preventive[i].Department,
                        "status": Preventive[i].Status,
                        "month": Preventive[i].PlannedMonth,
                        "year": Preventive[i].Year,
                        "incharge": Preventive[i].Incharge,
                        "pcid": Preventive[i].ExistingPCID,
                        "total": Preventive.length
                    };
                    _arrpreventive.push(JSON.stringify(_jsonpreventive));
                }

                $("#tbloverlay").hide();
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
                alert("[loadGanttChart()] \n Oops, something went wrong.\n Error: " + msg);
            }
        });
        $("#tbloverlay").hide();        
        $("#error-preventive-required").hide();
    }
}

function submitPreventive() {

    var table = document.getElementById("tbl-preventive-summary");

    $.ajax({
        url: '/Preventive/SavePreventive',
        type: 'POST',
        data: JSON.stringify({
            Preventive: _arrpreventive
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#tbloverlay").show();
        },
        success: function (Summary) {

            var toSummary = JSON.parse(Summary);
            var Month  = "'" + toSummary['month'] + "'";
            var rownum = table.insertRow(1);

            var cell1 = rownum.insertCell(0);
            var cell2 = rownum.insertCell(1);
            var cell3 = rownum.insertCell(2);
            var cell4 = rownum.insertCell(3);
            var cell5 = rownum.insertCell(4);
          
            cell1.innerHTML = "<span class='badge badge-danger'>New</span> " + toSummary['year'];
            cell2.innerHTML = Month;
            cell3.innerHTML = toSummary['total'] + " Out of " + toSummary['total'];
            cell4.innerHTML = "On-going";
            cell5.innerHTML = '<button class="btn btn-danger" onclick="getPlanDetails('+Month+')"><i class = "fa fa-pencil"></i></button>';
           
            $(".modal-new-preventive").modal("hide");
            $("#tbloverlay").hide();
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
            alert("[submitPreventive()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });
}
function getPlanDetails(planmonth, search) {

    var table = document.getElementById("tbl-plan-details");
    $('#tb-plan-details').empty();

    $.ajax({
        url: '/Preventive/GetPlanDetails',
        type: 'POST',
        data: JSON.stringify({
            PlanMonth: planmonth,
            Search: search
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#tblverlay").show();
        },
        success: function (Details) {
            var Monthyr = "";
            var Statuslbl = "";

            for (var i in Details) {

                var rownum = table.insertRow(1);

                var cell1 = rownum.insertCell(0);
                var cell2 = rownum.insertCell(1);
                var cell3 = rownum.insertCell(2);
                var cell4 = rownum.insertCell(3);
                var cell5 = rownum.insertCell(4);
                var cell6 = rownum.insertCell(5);

                var ID = "'" + Details[i].ID + "'";
                var PC         = "'" + Details[i].ComputerName + "'";
                var Equipment = "'" + Details[i].Equipment + "'";
                var Department = "'" + Details[i].Department + "'";
                var Incharge = "'" + Details[i].Incharge.split(',').join(' ').split('.').join(' ') + "'";

                if (Details[i].Status !== 'Completed')
                    Statuslbl = "<span class='badge badge-danger'>" + Details[i].Status + "</span> ";
                else
                    Statuslbl = "<span class='badge badge-success'>" + Details[i].Status + "</span> ";

                cell1.innerHTML = Details[i].ComputerName;
                cell2.innerHTML = Details[i].Equipment;
                cell3.innerHTML = Details[i].Department;
                cell4.innerHTML = Details[i].Incharge;
                cell5.innerHTML = Statuslbl;
                cell6.innerHTML = '<button class="btn btn-warning" onclick="showActual(' + PC + ','
                                                                                         + Equipment  + ','
                                                                                         + Department + ','
                                                                                         + ID + ','
                                                                                         + Incharge   + ')"><i class = "fa fa-folder-open-o"></i></button>';
                Monthyr = Details[i].PlannedMonth;
            }
            $("#txt-details-monthyr").val(Monthyr);
            $("#tblverlay").hide();
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
            alert("[submitPreventive()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });
}
function showActual(pc,equipment,department,id, incharge) {

    $("#lbl-maintenance-id").text(id);
    $("#txt-actual-pcname").val(pc);
    $("#txt-actual-type").val(equipment);
    $("#txt-actual-department").val(department);
    $("#txt-actual-incharge").val(incharge);

    $(".modal-actual").modal("show"); 
}

function submitDiagnosis() {
    var Count = $("#lbl-diagnose-count").text();
    var arrDiagonosis = [];
    var jsonDiagnosis = {};

    var actualdate = $("#txt-actual-date").val();
    var id = $("#lbl-maintenance-id").text();

    //approvals
    var preparedby = $("#drd-preparedby").val();
    var checkedby = $("#drd-checkedby").val();
    var approvedby = $("#drd-approvedby").val();

    for (var i = 1; i <= parseInt(Count); i++) {

        var diagnoseid = $("#lbl-services-id-" + i).text();
        var notes      = $("#txt-notes-" + i).val();
        var remarks    = $("#txt-remarks-" + i).val();
        var duration = $("#txt-duration-" + i).val();

        var jsonDiagnosis = {
            "maintennaceid": id,
            "servicesid": diagnoseid,
            "notes": notes,
            "remarks": remarks,
            "duration": duration
           };

        arrDiagonosis.push(JSON.stringify(jsonDiagnosis));
    }

    $.ajax({
        url: '/Preventive/SavePreventiceDiagnosis',
        type: 'POST',
        data: JSON.stringify({
            ActualTime: actualdate,
            Diagnosis:  arrDiagonosis,
            PreparedBy: preparedby,
            CheckedBy:  checkedby,
            ApprovedBy: approvedby
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
        },
        success: function () {
            var month = $("#txt-details-monthyr").val();
            getPlanDetails(month,'');
            $(".modal-actual").modal("hide");
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
            alert("[submitPreventive()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });

}

function displayChart() {
    var ctx = document.getElementById("preventive-chart");

    var ctx = document.getElementById("preventive-chart");
    var hwLineChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: _preventive_chart_month,
            datasets: [
                {
                    data: _preventive_chart_standard,
                    label: "Standard Time",
                    borderColor: "green",
                    // backgroundColor: "red",
                    fill: false
                },
                {
                    data: _preventive_chart_actual,
                    label: "Actual Time",
                    borderColor: "red",
                    fill: false
                },
            ]
        },
        options: {
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Month-Year'
                    },
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Preventive'
                    }
                }]
            }
        }
    });
}
function preventiveSearch() {
    var search = $("#txt-preventive-search").val();
    var month = $("#txt-details-monthyr").val();

    getPlanDetails(month, search);
}

function reloadDetails() {
    var month = $("#txt-details-monthyr").val();

    getPlanDetails(month, '');
}

//----------------- | R E P O R T S | ---------------------------------

function printToHardCopy() {
    var month = $("#txt-details-monthyr").val();

    var url = "/Preventive/ExportPreventive?PlanMonth=" + month;
    window.location.href = url;

}

