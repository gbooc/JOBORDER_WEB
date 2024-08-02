var _index = 2;

function linkJO(param) {

    var idcounter = parseInt(param.id.match(/\d+/), 10);
    var id = "slct-costcenter" + idcounter;

    var jobno = $("#" + param.id).val();
 
    if (jobno == "") {
        var element = document.getElementById(id);
        element.value = "0";
        return;
    }

    $.ajax({
        url: '/Reports/GetJODetails',
        type: 'POST',
        data: JSON.stringify({
            JobNo: jobno
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
        },
        success: function (JobOrder) {

            console.log(JobOrder);
            $("#tb-jo > tr").remove();

            var selectObj = document.getElementById(id);

            for (var i = 0; i < selectObj.options.length; i++) {
            
                var progressbar = JobOrder.ProgressRate + "%";

                if (selectObj.options[i].text == JobOrder.JO_Costcenter) {
                    selectObj.options[i].selected = true;
                    var row = $("<tr class='w3-hover-blue-gray'>");

                    row.append($("<td style='width:40%;'>" + JobOrder.WorkDetails + "</td>"))
                        .append($("<td>" + JobOrder.DateTarget + "</td>"))
                        .append($("<td>" + JobOrder.Requestor + "</td>"))
                        .append($("<td> <div class='progress'>"+
                                             "<div class= 'progress-bar progress-bar-striped' role = 'progressbar' aria-valuenow='" + progressbar + "' aria-valuemin='0' aria-valuemax='100' style = 'width:" + progressbar+";'>" +
                                                "<label id='lbl-progress'>" + progressbar +"</label>"+
                                             "</div>" +
                                       "</div></td>"))
                        .append($('<td><button class="btn btn-danger" onclick="viewDetails(\'' + jobno + '\')">Add Progress</button>' +
                                + '</td>'));

                    $("#tbl-jo tbody").append(row);

                    return;
                }
            }
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
function viewDetails(JO) {
    window.open("http://kpsweb2:1001/JobOrders/JobOrderDetails?JoNumber="+JO, "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=300,left=300,width=1700,height=700")

}
function submitReport() {

    var date = $("#txt-date").val();
    var day  = $("#txt-day").val();
    var hasEmpty = false;

    for (var i = 1; i <= _index; i++) {

        var timestart = $("#txt-timestart" + i).val();
        var timend = $("#txt-timeend" + i).val();
        var mins = $("#txt-minutes" + i).val();
        var activity = $("#txt-activity" + i).val();
        var jobno = $("#slct-jobno" + i).val();
        var costcenter = $("#slct-costcenter" + i).val();
        var company = $("#slct-company" + i).val();
        var empid = $("#txt-apcid").val();

        if (timestart == "") {
            $("#txt-timestart" + i).addClass("is-invalid");
            hasEmpty = true;
            console.log("timestart");
        }
        if (timend == "") {
            $("#txt-timeend" + i).addClass("is-invalid");
            hasEmpty = true;
        } if (mins == "") {
            $("#txt-minutes" + i).addClass("is-invalid");
            hasEmpty = true;

        } if (activity == "") {
            $("#txt-activity" + i).addClass("is-invalid");

            hasEmpty = true;
        } if (costcenter == "") {
            $("#slct-costcenter" + i).addClass("is-invalid");
            hasEmpty = true;
        }

        if (hasEmpty)
            break;

        //All fields is not empty, proceed to next process
        $.ajax({
            url: '/Reports/AddReport',
            type: 'POST',
            data: JSON.stringify({
                Empid: empid,
                Date: date,
                Day: day,
                TimeStart: timestart,
                TimeEnd: timend,
                Mins: mins,
                Activity: activity,
                JobNo: jobno,
                CostCenter: costcenter,
                Company: company
            }),
            dataType: "json",
            traditional: true,
            contentType: 'application/json;',
            cache: false,
            beforeSend: function () {
                $("#tblverlay").show();
            },
            success: function (Result) {

                if (Result != "") {
                    alert(Result);
                } else {
                    reloadPage();
                }
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
                alert("[submitReport()] \n Oops, something went wrong.\n Error: " + msg);
            }
        });

        hasEmpty = false;
    }
}

function convertHrsToMins(n) {
    var num = n;
    var hours = (num / 60);
    var rhours = Math.floor(hours);
    var minutes = (hours - rhours) * 60;
    var rminutes = Math.round(minutes);
    return rminutes;
}


function duplicate() {

    var original = document.getElementById('dv' + _index);
    var clone = original.cloneNode(true); // "deep" clone

    document.getElementById('txt-timestart2').id = 'txt-timestart' + (_index + 1);
    document.getElementById('txt-timeend2').id = 'txt-timeend' + (_index + 1);
    document.getElementById('txt-minutes2').id = 'txt-minutes' + (_index + 1);
    document.getElementById('txt-activity2').id = 'txt-activity' + (_index + 1);
    document.getElementById('slct-costcenter2').id = 'slct-costcenter' + (_index + 1);
    document.getElementById('slct-company2').id = 'slct-company' + (_index + 1);
    document.getElementById('slct-jobno2').id = 'slct-jobno' + (_index + 1);

    clone.id = "dv" + ++_index; // there can only be one element with an ID
    original.parentNode.appendChild(clone);

    $('.clockpicker').clockpicker({
        'default': 'now',
        vibrate: true,
        placement: "bottom",
        align: "left",
        twelvehour: false,
        autoclose: true
    });

}

function getminutes(param) {

    var idcounter = parseInt(param.id.match(/\d+/), 10);

    var hrstart = $("#txt-timestart" + idcounter).val();
    var hrend = $("#txt-timeend" + idcounter).val();

    var splitStart = hrstart.split(":");
    var splitEnd = hrend.split(":");

    var start = new Date(0, 0, 0, splitStart[0], splitStart[1], 0);
    var end = new Date(0, 0, 0, splitEnd[0], splitEnd[1], 0);


    var diffMs = (end - start);
    var diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
    var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes

    var totalhrs = (diffHrs * 60) + diffMins;

    $("#txt-minutes" + idcounter).val(totalhrs);
}
function exportAndSend() {

    var empid = $("#lbl-empid").text();
    $.ajax({
        url: '/Reports/SendEmailReport',
        type: 'POST',
        data: JSON.stringify({
            EmpID: empid
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#teOverlay").show();
        },
        success: function (Result) {
            $(".md-confirmation").modal("hide");

            if (Result != "") {
                alert(Result);
            } else {
                setTimeout(function () {
                    $("#teOverlay").hide();
                }, 2000);
            }
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
            alert("[submitReport()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });
}

function updateReport(param) {

    var timestart = $("#txt-timestart" + param).val();
    var timeend   = $("#txt-timeend" + param).val();
    var mins      = $("#txt-minutes" + param).val();
    var activity  = $("#txt-activity" + param).val();
    var costcenter = $("#slct-costcenter" + param).val();
    var company    = $("#slct-company" + param).val();

    $.ajax({
        url: '/Reports/UpdateDailyReport',
        type: 'POST',
        data: JSON.stringify({
            ID: param,
            TimeStart: timestart,
            TimeEnd: timeend,
            Mins: mins,
            Activity: activity,
            Costcenter: costcenter,
            Company: company
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {

            if (Result != "")
                alert(Result);

            $("#txt-timestart" + param).addClass("is-valid");
            $("#txt-timeend" + param).addClass("is-valid");
            $("#txt-minutes" + param).addClass("is-valid");
            $("#txt-activity" + param).addClass("is-valid");
            $("#slct-costcenter" + param).addClass("is-valid");
            $("#slct-company" + param).addClass("is-valid");
        }
    });
}

function resendEmail(empid, date) {

    $("#lbl-empid").text(empid);
    $("#lbl-datesent").text(date);

    $(".md-confirmation").modal("show");
}

function sendEmail() {

    $.ajax({
        url: '/Reports/ResendEmail',
        type: 'POST',
        data: JSON.stringify({
            EmpID: $("#lbl-empid").text(),
            DateSend: $("#lbl-datesent").text()
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#teOverlay").show();
        },
        success: function (Result) {
            $(".md-confirmation").modal("hide");
            alert("Message Sent.");
            
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
            alert("[submitReport()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });

}

$(document).ready(function () {
    $('.collapse.in').prev('.panel-heading').addClass('active');
    $('#accordion, #bs-collapse')
        .on('show.bs.collapse', function (a) {
            $(a.target).prev('.panel-heading').addClass('active');
        })
        .on('hide.bs.collapse', function (a) {
            $(a.target).prev('.panel-heading').removeClass('active');
        });
});