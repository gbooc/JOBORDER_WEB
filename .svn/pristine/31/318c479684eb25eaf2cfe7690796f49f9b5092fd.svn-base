var _count = "";
var _chartcount = "";

var _code_number = [];
var _minsofdowntime = [];
var _network_name = [];


window.onload = function () {
    var count = $("#network-count").val();
    var chartcount = $("#lbl-downtimechart-count").text();

    var network_name = [];
    var minsofdowntime = [];
    var downtimemins = 0;

    for (var i = 1; i < count; i++) {
        //Add server name and code number
        var tmp_codenumber = $("#txt-codenumber-" + i).text();
        
        _code_number.push(JSON.stringify(tmp_codenumber));
        //To get and add downtime minutes
        for (var a = 1; a <= chartcount; a++) {
            var tmp_chrt_codenumber = $("#lbl-chart-codenumber-" + a).text();
            var tmp_downtime = $("#lbl-chart-downtimemins-" + a).text();

            if (tmp_codenumber.trim() == tmp_chrt_codenumber.trim())
                downtimemins = tmp_downtime;
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
    _network_name = _code_number;
    console.log(network_name);
    _minsofdowntime = minsofdowntime;
    // ----------------- C H A R T S ---------------------------
   displayChart();
}

function displayChart() {
    var ctx = document.getElementById("networkChart");

    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: _network_name,
            datasets: [
                {
                    backgroundColor: "red",
                    label: "Network Downtime",
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
                        labelString: 'Switch'
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

function saveDowntime() {

    var datestarted = $("#txt-network-date-started").val();
    var timestarted = $("#txt-network-time-started").val();
    var dateended = $("#txt-network-date-ended").val();
    var timeended = $("#txt-network-time-ended").val();
    var codenumber = $("#txt-switch-name").val();
    var downtime_reason = $("#txt-downtime-reason").val();
    var causeid = 0;

    var isAllValid = true;

    //trappings [START]
    if ($("#txt-switch-name").val() == "" || $("#txt-switch-name").val() == "No matches found") {
        $("#txt-switch-name").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-switch-name").removeClass("is-invalid");
        $("#txt-switch-name").addClass("is-valid");
    }

    if ($("#txt-network-date-started").val() == "") {
        $("#txt-network-date-started").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-network-date-started").removeClass("is-invalid");
        $("#txt-network-date-started").addClass("is-valid");
    }
    if ($("#txt-network-time-started").val() == "") {
        $("#txt-network-time-started").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-network-time-started").removeClass("is-invalid");
        $("#txt-network-time-started").addClass("is-valid");
    }

    if ($("#txt-network-date-ended").val() == "") {
        $("#txt-network-date-ended").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-network-date-ended").removeClass("is-invalid");
        $("#txt-network-date-ended").addClass("is-valid");
    }
    if ($("#txt-network-time-ended").val() == "") {
        $("#txt-network-time-ended").addClass("is-invalid");
        isAllValid = false;
    } else {
        $("#txt-network-time-ended").removeClass("is-invalid");
        $("#txt-network-time-ended").addClass("is-valid");
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
                url: '/Network/SaveNetworkDowntime',
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
function addDowntTimeCause(count) {

    $("#txt-downtime-cause-id").val(count);
    $(".bs-example-modal-downtimecause").modal("show");
}

function viewDowntimeDetails(network) {

    $("#lbl-server-name").text(network);
    var date_filter = $("#downtime-date").val();

    $.ajax({
        url: '/Network/GetNetworkDowntimeDetails',
        type: 'POST',
        data: JSON.stringify(
            {
                NetworkName: network,
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
                var cell8 = rownum.insertCell(7);

                cell1.innerHTML = count;
                cell2.innerHTML = network;
                cell3.innerHTML = result[i].Location;
                cell4.innerHTML = result[i].DateStarted;
                cell5.innerHTML = result[i].DateEnded;
                cell6.innerHTML = result[i].DowntimeMinutes;
                cell7.innerHTML = result[i].DowntimeCause;
                cell8.innerHTML = result[i].DowntimeCauseDetails;

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
   // $(".bs-example-modal-downtimedetails").modal("show");
}

function searchChartByMonth() {
    var date = $("#downtime-date").val();
    _minsofdowntime = [];

    $.ajax({
        url: '/Network/SearchDowntime',
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
            for (x = 0; x < _count - 1; x++) {
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

function loadDowntimeDetails(date) {

    $.ajax({
        url: '/Network/ReloadNetworkDowntime',
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