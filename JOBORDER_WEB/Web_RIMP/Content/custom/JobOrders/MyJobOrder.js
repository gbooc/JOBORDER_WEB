var _hw_name = [];
var _hw_count = [];

$(function () {

    $("#txt-myjo-search").keyup(function (e) {
        if (e.which == 13) {
            myjoSearch('search', '');
        }
    });
});


window.onload = function () {

    var count = $("#lbl-hw-total-count").text();

    for (var i = 1; i <= count; i++) {

        var hwname = $("#lbl-hw-name_" + i).text();
        var hwcount = $("#lbl-hw-type_" + i).text();

        _hw_name.push(hwname);
        _hw_count.push(hwcount);
    }

    displayChart();
};



function displayChart() {

    var ctx = document.getElementById("my-hw-chart").getContext('2d');
    var hwLineChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: _hw_name,
            datasets: [{
                backgroundColor: [
                    "#2ecc71",
                    "#3498db",
                    "#95a5a6",
                    "#9b59b6",
                    "#f1c40f",
                    "#e74c3c",
                    "#34495e"
                ],
                data: _hw_count,
                borderWidth: 1,
                hoverBackgroundColor: "rgba(232,105,90,0.8)",
                hoverBorderColor: "orange",
                scaleStepWidth: 1
            }]
        },
        options: {
            legend: {
                labels: {
                    fontSize: 15
                }
            },
        }
    });
}

function myjoSearch(process, system) {

    var search = "";
    var table = "";

    if (process == "search") {

        if (system == "")
            search = $("#txt-myjo-search").val();
        else
            search = system;

        $("#tb-myjo > tr").remove();
        table = "#tbl-myjo";

    } else { // software
        search = system;
        table = "#tbl-myjo-sw-details";
        $("#tb-myjo-sw-details > tr").remove();
    }

    $.ajax({
        url: '/JobOrderDetails/SearchMyJO',
        type: 'POST',
        data: JSON.stringify(
            {
                Search: search
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {



            for (var i in Result) {

                console.log("tset");
                
                var row = $("<tr>");
                var datetarget = '"' + Result[i].DateTarget + '"';
                var dateassigned = '"' + Result[i].DateAssigned + '"';

                var Status = Result[i].Status.trim() == "Closed" ? "<span class='w3-round-sm w3-deep-purple'> <i class='fa fa-check'></i> " + Result[i].Status + "</span>"
                    : "<span class='w3-round-sm  w3-cyan'><i class='fa fa-cogs'></i> " + Result[i].Status + "</span>";

                row.append($("<td style='text-align:center;'>" + Result[i].JoTypeDetails + "</td>"))
                    .append($("<td style='width:220px;'>" + Status + "</td>"))
                    .append($("<td style='text-align:right;'>" + Result[i].Assignee + "</td>"))
                  //  .append($("<td>" + Result[i].Datefiled + "</td>"))
                    .append($("<td style='text-align:right;'>" + Result[i].DateTarget + "</td>"))
                    .append($("<td style='text-align:right;'>" + Result[i].DateEnded + "</td>"))
                    .append($("<td style='width:30px;'><a href = '#'  onclick='loadGanttChart(" + Result[i].JODetailsID + "," + datetarget + "," + dateassigned + " )' class = 'btn btn-outline-warning'><i class = 'fa fa-calendar'></i></a></td>"))
                    .append($("<td style='width:200px;'><a href = '/JobOrders/JobOrderDetails?JoNumber=" + Result[i].JobNumber + "#AllJo' class = 'btn btn-outline-success'><i class = 'fa fa-pencil-square'></i></a></td>"));

                $(table + " tbody").append(row);
            }
        },
    });
}

function getAllMySWJO(type, system) {

    $("#tb-myjo > tr").remove();
    $("#tb-myjo-sw-details > tr").remove();

    //All software
    if (type == "Software") {
        $("#dv-myjo-hw").hide();
        $("#dv-myjo-sw").show();


    } else { // All hardware
        $("#dv-myjo-hw").show();
        $("#dv-myjo-sw").hide();

        myjoSearch('search', system);

    }
}

function loadGanttChart(detailsid,datetarget, dateassigned) {

   $(".bs-example-modal-new4").modal("show");

    _event = [];
    $.ajax({
        url: '/JobOrders/LoadGanttChart',
        type: 'POST',
        data: JSON.stringify(
            {
                DetailsID: detailsid
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {

            for (var i in Result) {

                title = Result[i].Function;

                _jsonEvent = {
                    title: Result[i].Function,
                    start: Result[i].DateStarted,
                    end: Result[i].DateStarted,
                    textColor: "white",
                    color: 'Green',
                    popup: {
                        title: Result[i].Function,
                        descri: Result[i].Function,
                        start: Result[i].DateStarted,
                        end: $("#txt-datetargetId").val()
                    }
                };

                _event.push(_jsonEvent);
            }
            _jsonEvent = {
                title: "JO Duration",
                start: dateassigned,
                end: datetarget,
                textColor: "white",
                color: 'purple',
                popup: {
                    title: "Date Target",
                    descri: "Target Of Completion",
                    start: dateassigned,
                    end: datetarget
                }
            };
            _event.push(_jsonEvent);

            LoadCalendar(JSON.stringify(_event));
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
}

//Display the calendar to div
function LoadCalendar(event) {

    document.getElementById("calendar").innerHTML = "";
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid', 'timeGrid'],
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        navLinks: true, // can click day/week names to navigate views
        selectable: true,
        selectMirror: true,
        select: function (args) {
            var userdept = $("#user-dept").text();

            if (userdept == "ASIA PACIFIC CENTER") {
                var date = new Date(args.start);
                var month = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
                var datestart = date.getFullYear() + "-" + month[date.getMonth()] + "-" + date.getDate();

                $("#txtganttchartStart").val(datestart);
                $(".bs-example-modal-new5").modal("show");
            }
        },
        editable: false,
        eventLimit: true,
        events: JSON.parse(event),
        eventMouseEnter: function (info) {

            var tis = info.el;
            var popup = info.event.extendedProps.popup;
            var tooltip = '<div class="tooltipevent" style="top:' + ($(tis).offset().top - 5) + 'px;left:' + ($(tis).offset().left + ($(tis).width()) / 2) + 'px"><div>' + popup.descri + '</div><div>Title: <u>' + popup.title + '</u><br/> Date Duration: <br/><u>' + popup.start + ' - ' + popup.end + '</u></div></div>';
            var $tooltip = $(tooltip).appendTo('body');

            $(tis).mouseover(function (e) {
                $(tis).css('z-index', 10000);
                $tooltip.fadeIn('500');
                $tooltip.fadeTo('10', 1.9);
            }).mousemove(function (e) {
                $tooltip.css('top', e.pageY + 10);
                $tooltip.css('left', e.pageX + 20);
            });
        },
        eventMouseLeave: function (info) {
            $(info.el).css('z-index', 1);
            $('.tooltipevent').remove();
        },
        contentHeight: 700
    });
    calendar.render();
}

function searchJODepartment() {

    $("#tb-ongoing-dept > tr").remove();


    var keyword = $("#depttxt-myjo-search").val();
    $.ajax({
        url: '/JobOrderDetails/SearchMyJoByDept',
        type: 'POST',
        data: JSON.stringify(
            {
                Keyword: keyword
            }),

        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {

            for (var i in Result) {

                var row = $("<tr>");

                var progressrate = Result[i].ProgressRate + "%"; 
                var datetarget = '"' + Result[i].DateTarget + '"';
                var dateassigned = '"' + Result[i].DateAssigned + '"';

                row.append($("<td>" + Result[i].JONumber + "</td>"))
                    .append($("<td>" + Result[i].Details + "</td>"))
                    //  .append($("<td>" + Result[i].Datefiled + "</td>"))
                    .append($("<td>" + Result[i].Requestor + "</td>"))
                    .append($("<td>" + Result[i].Assignee + "</td>"))
                    .append($("<td style='text-align:center;'>" + Result[i].Status == "Closed" ? "<span class='badge badge-success'>"
                        + Result[i].Status + "</span>" : "<span class='badge badge-danger'>"
                        + Result[i].Status + "</span>" + "</td>"))
                    .append($("<td>" +
                            "<div class='progress'>" +
                        "<div class= 'progress-bar progress-bar-striped bg-warning' role = 'progressbar' aria-valuenow='" + Result[i].ProgressRate+"'"+
                                              "aria-valuemin='0' aria-valuemax='100' style = 'width:"+progressrate+"'>"+
                        "<label id='lbl-progress'>" + progressrate+"</label>"+
                                 "</div>" +
                             "</div></td>"))
                    .append($("<td style='width:30px;'><a href = '#'  onclick='loadGanttChart(" + Result[i].DetailsID + "," + datetarget + "," + dateassigned + " )' class = 'btn btn-outline-warning'><i class = 'fa fa-calendar'></i></a></td>"))
                    .append($("<td style='width:30px;'><a href = '/JobOrders/JobOrderDetails?JoNumber=" + Result[i].JobNumber + "#AllJo' class = 'btn btn-outline-warning'><i class = 'fa fa-pencil-square'></i></a></td>"));

                $("#tbl-ongoing-dept tbody").append(row);
            }
        },
    });
}