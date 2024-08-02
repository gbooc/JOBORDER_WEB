var _count = "";
var _chartcount = "";

var _code_number = [];
var _minsofdowntime = [];
var _server_name = [];


window.onload = function () {
    var count = $("#server-count").val();
    var chartcount = $("#lbl-downtimechart-count").text();

    var server_name = [];
    var minsofdowntime = [];
    var downtimemins = 0;

    for (var i = 1; i < count; i++) {
        //Add server name and code number
        var tmp_server_name = $("#txt-server-name_" + i).text();
        var tmp_codenumber = $("#txt-codenumber_" + i).text();

        server_name.push(JSON.stringify(tmp_server_name));
        _code_number.push(JSON.stringify(tmp_codenumber));
        //To get and add downtime minutes
        for (var a = 1; a <= chartcount; a++) {
            var tmp_chrt_codenumber = $("#lbl-chart-codenumber-" + a).text();
            var tmp_downtime = $("#lbl-chart-downtimemins-" + a).text();

            if (tmp_codenumber.trim() == tmp_chrt_codenumber.trim()) {
                downtimemins = tmp_downtime;
            }

        }

        if (downtimemins > 0) {
            minsofdowntime.push(downtimemins);
        } else {
            minsofdowntime.push(0);
        }
        downtimemins = 0;
    }

    //copy details searchChartByMonth() function
    _count = count;
    _chartcount = chartcount;
    _server_name = server_name;
    _minsofdowntime = minsofdowntime;
   // ----------------- C H A R T S ---------------------------
    displayChart();
}
function displayChart() {
    var ctx = document.getElementById("serverChart");

    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: _server_name,
            datasets: [
                {
                    backgroundColor: "red",
                    label: "Server Downtime",
                    data: _minsofdowntime
                }
            ]
        },
        options: {
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Server'
                    },
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Downtime',
                        events: ['click']
                    }
                }]
            },
            'onClick': function (evt, item) {
                var activePoints = hwBarchart.getElementsAtEvent(evt);
                if (activePoints[0]) {
                    var chartData = activePoints[0]['_chart'].config.data;
                    var idx = activePoints[0]['_index'];

                    var label = chartData.labels[idx];
                    var result = label.substring(1, label.length - 1);
                    viewDowntimeDetails(result);
                }
            }
        }
    });
}
function searchChartByMonth() {
    var date = $("#downtime-date").val();
    _minsofdowntime =[];

    $.ajax({
        url: '/Server/SearchMonthlyDowntime',
        type: 'POST',
        data: JSON.stringify(
            {
                Date: date
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (result) {
            var tmpdowntimemins = 0;
            for (x = 0; x < _count-1; x++) {
                for (var i in result) {

                    if (result[i].CodeNumber == _code_number[x].split('"').join('').trim())
                        tmpdowntimemins = result[i].DowntimeMinutes;
                }
                if (tmpdowntimemins > 0)
                    _minsofdowntime.push(tmpdowntimemins);
                else
                    _minsofdowntime.push(0);

                tmpdowntimemins = 0;
            }
            loadDowntimeDetails(date);
            displayChart();
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
            alert("Oops, something went wrong.\n Error: " + msg);
        }
    });
}

function addDowntTimeCause(count) {
    $("#txt-downtime-cause-id").val(count);
    $(".bs-example-modal-downtimecause").modal("show");
}

function saveDowntime() {

    var datestarted = $("#txt-server-date-started").val();
    var timestarted = $("#txt-server-time-started").val();
    var dateended = $("#txt-server-date-ended").val();
    var timeended = $("#txt-server-time-ended").val();
    var codenumber = $("#txt-server-name").val();
    var downtime_reason = $("#txt-downtime-reason").val();

    var causeid = 0;
    var isAllValid = true;
    //trappings [START]
    if ($("#txt-server-name").val() == "" || $("#txt-server-name").val() == "No matches found") {
        $("#txt-server-name").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-server-name").removeClass("is-invalid");
        $("#txt-server-name").addClass("is-valid");
    }

    if ($("#txt-server-date-started").val() == "") {
        $("#txt-server-date-started").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-server-date-started").removeClass("is-invalid");
        $("#txt-server-date-started").addClass("is-valid");
    }
    if ($("#txt-server-time-started").val() == "") {
        $("#txt-server-time-started").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-server-time-started").removeClass("is-invalid");
        $("#txt-server-time-started").addClass("is-valid");
    }

    if ($("#txt-server-date-ended").val() == "") {
        $("#txt-server-date-ended").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-server-date-ended").removeClass("is-invalid");
        $("#txt-server-date-ended").addClass("is-valid");
    }
    if ($("#txt-server-time-ended").val() == "") {
        $("#txt-server-time-ended").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-server-time-ended").removeClass("is-invalid");
        $("#txt-server-time-ended").addClass("is-valid");
    }
    if ($("#txt-downtime-reason").val() == "") {
        $("#txt-downtime-reason").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-downtime-reason").removeClass("is-invalid");
        $("#txt-downtime-reason").addClass("is-valid");
    }

    //END

    if (isAllValid) {
        for (var i = 1; i <= $("#txt-Cause-Count").val(); i++) {

            if ($("#cb-cause-" + i).is(':checked')) {
                causeid = $("#cb-cause-" + i).val();
            }
        }

        if (causeid == 0) {
            alert("You must add downtime cause");
        } else {

            $("#tblverlay").show();
            $(".bs-example-modal-downtimecause").modal("hide");

            $.ajax({
                url: '/Server/SaveServerDowntime',
                type: 'POST',
                data: JSON.stringify(
                    {
                        CauseID: causeid,
                        CodeNumber: codenumber,
                        DateStarted: datestarted,
                        TimeStarted: timestarted,
                        DateEnded: dateended,
                        TimeEnded: timeended,
                        CauseDetails: downtime_reason
                    }),
                dataType: "json",
                traditional: true,
                contentType: 'application/json;',
                cache: false,
                success: function (result) {
                    if (result == "Success") {

                        setTimeout(function () {
                            $("#tblverlay").hide();
                            $(".bs-example-modal-confirmation").modal("show");
                        }, 1500);
                    }
                },
            });
        }
    }
}

function viewDowntimeDetails(servername) {
   
    var date_filter = $("#downtime-date").val();
    $("#lbl-server-name").text(servername);

    $.ajax({
        url: '/Server/GetServerDowntimeDetails',
        type: 'POST',
        data: JSON.stringify(
            {
                ServernName: servername,
                DateFilter: date_filter
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#tblverlay").show();
        },
        success: function (result) {
            var table = document.getElementById("tbl-downtime-details");
            $('#tb').empty();

            var count = result.length;
            for (var i in result) {

                var rownum = table.insertRow(1);

                var cell1 = rownum.insertCell(0);
                var cell2 = rownum.insertCell(1);
                var cell3 = rownum.insertCell(2);
                var cell4 = rownum.insertCell(3);
                var cell5 = rownum.insertCell(4);
                var cell6 = rownum.insertCell(5);
                var cell7 = rownum.insertCell(6);

                cell1.innerHTML = count;
                cell2.innerHTML = servername;
                cell3.innerHTML = result[i].DateStarted;
                cell4.innerHTML = result[i].DateEnded;
                cell5.innerHTML = result[i].DowntimeMinutes;
                cell6.innerHTML = result[i].DowntimeCause;
                cell7.innerHTML = result[i].DowntimeCauseDetails;

                count--;
            }
            setTimeout(function () {
                $("#tblverlay").hide();
            }, 800);
           
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
            alert("Oops, something went wrong.\n Error: " + msg);
        }
    });
}

function loadDowntimeDetails(date) {

    $.ajax({
        url: '/Server/ReloadServerDowntime',
        type: 'POST',
        data: JSON.stringify(
            {
                DateMonth: date
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#tblverlay").show();
        },
        success: function (result) {
            var table = document.getElementById("tbl-downtime-details");
            $('#tb').empty();

            var count = result.length;
            for (var i in result) {

                var rownum = table.insertRow(1);

                var cell1 = rownum.insertCell(0);
                var cell2 = rownum.insertCell(1);
                var cell3 = rownum.insertCell(2);
                var cell4 = rownum.insertCell(3);
                var cell5 = rownum.insertCell(4);
                var cell6 = rownum.insertCell(5);
                var cell7 = rownum.insertCell(6);

                cell1.innerHTML = count;
                cell2.innerHTML = result[i].DowntimeName;
                cell3.innerHTML = result[i].DateStarted;
                cell4.innerHTML = result[i].DateEnded;
                cell5.innerHTML = result[i].DowntimeMinutes;
                cell6.innerHTML = result[i].DowntimeCause;
                cell7.innerHTML = result[i].DowntimeCauseDetails;

                count--;
            }
            setTimeout(function () {
                $("#tblverlay").hide();
            }, 800);

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
            alert("Oops, something went wrong.\n Error: " + msg);
        }
    });
}