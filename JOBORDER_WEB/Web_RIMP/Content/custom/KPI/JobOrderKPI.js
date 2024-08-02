//Data
var _graph_completejo = [];

var _ontime = [];
var _delayed = [];
var _unassigned = [];
var _ongoing = [];
var _hw_avg_standartime = [];
var _hw_avg_actualtime = [];
var _satisfactory_rate = [];
var _apc_name = [];
var _alljo = [];
//Month
var _graph_monthjo = [];
var _graph_assigned_monthjo = [];
var _all_year = [];
var _all_year_details = [];
var allemails = 0;
var alldesktop = 0;
var allnetwork = 0;
var allserver = 0;
var allothers = 0;
var alltel = 0;
var software = 0;

window.onload = function () {

    var today = new Date();
    var currentyr = today.getFullYear();

    completedJO(currentyr);
  //  joPearYear();
    KPIHardwarePercentage();
    satisfactoryRate();
    joPerYearDetails();
    allApcJo();
    resoureGraph();
    downtimeChart();
}
function completedJO(year) {

    $.ajax({
        url: '/JobOrders/GetCompletedJO',
        type: 'POST',
        data: JSON.stringify(
            {
                Year: year
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Result) {

            for (var i in Result) {
                _graph_monthjo.push(Result[i].ClosedJOMonth);
                _graph_completejo.push(Result[i].ClosedJOCount);

                allemails += Result[i].CloseEmail;
                alldesktop += Result[i].ClosedDesktop;
                allnetwork += Result[i].ClosedNetwork;
                allserver += Result[i].CloseServer;
                allothers += Result[i].CloseOthers;
                alltel += Result[i].CloseTel;
                software += Result[i].CloseSoftware;

            }
            $("#lbl-ClosedEmail").text(allemails);
            $("#lbl-ClosedDesktop").text(alldesktop);
            $("#lbl-ClosedNetwork").text(allnetwork);
            $("#lbl-ClosedServer").text(allserver);
            $("#lbl-ClosedOthers").text(allothers);
            $("#lbl-ClosedTel").text(alltel);
            $("#lbl-ClosedSoftware").text(software);

            //Chart
            var ctx = document.getElementById("completedChart");
            var hwLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: _graph_monthjo,
                    datasets: [
                        {
                            data: _graph_completejo,
                            label: "Completed JO",
                            borderColor: "#212F3C",
                            backgroundColor: "RGBA(33, 47, 60,.5)",
                            fill: true,
                            borderWidth: 3,
                        },
                    ]
                },
                options: {
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,

                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Month'
                            },
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Total'
                            }
                        }]
                    },
                    animateScale: true,
                    plugins: {
                        filler: {
                            propagate: true
                        }
                    },
                    responsive: true,
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,
                    },
                    animation: {
                        duration: 3000,
                    }
                }
            });
        }
    });

    _graph_completejo = [];
    _graph_monthjo = [];

    allemails = 0;
    alldesktop = 0;
    allnetwork = 0;
    allserver = 0;
    allothers = 0;
    alltel = 0;
    software = 0;

}


//function joPearYear() {

//    //$.ajax({
//    //    url: '/JobOrders/KPIPerYear',
//    //    type: 'POST',
//    //    dataType: "json",
//    //    traditional: true,
//    //    contentType: 'application/json;',
//    //    cache: false,
//    //    beforeSend: function () {
//    //        // $("#tblverlay").show();
//    //    },
//    //    success: function (Result) {

//    //        for (var i in Result) {
//    //            _all_year.push(Result[i].Year);
//    //            _all_count_jo_yr.push(Result[i].YearCount);

//    //        }

//    //        //Chart
//    //        var ctx = document.getElementById("alljobyearChart");
//    //        var hwBarchart = new Chart(ctx, {
//    //            type: 'bar',
//    //            data: {
//    //                labels: _all_year,
//    //                datasets: [
//    //                    {
//    //                        backgroundColor: "RGBA(	0, 95, 81,.4)",
//    //                        label: "JO Created By Year",
//    //                        borderWidth: 1,
//    //                        borderColor: "#005f51",
//    //                        data: _all_count_jo_yr
//    //                    }
//    //                ]
//    //            },
//    //            options: {
//    //                scales: {
//    //                    xAxes: [{
//    //                        display: true,
//    //                        scaleLabel: {
//    //                            display: true,
//    //                            labelString: 'Year'
//    //                        },
//    //                    }],
//    //                    yAxes: [{
//    //                        display: true,
//    //                        scaleLabel: {
//    //                            display: true,
//    //                            labelString: 'Total JO',
//    //                        }
//    //                    }]
//    //                }
//    //            }
//    //        });

//    //    }
//    //});

//    //_all_year = [];
//    //_all_count_jo_yr = [];


//}

function joPerYearDetails() {
    $.ajax({
        url: '/JobOrders/KPIPerYearDetails',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Result) {

            for (var i in Result) {
                _all_year_details.push(Result[i].Year);

                _ontime.push(Result[i].OnTime);
                _ongoing.push(Result[i].Ongoing);
                _unassigned.push(Result[i].Unassigned);
                _delayed.push(Result[i].Delayed);

            }
            //Chart
            // ------------ DELAYED CHART / COMPLETED / UNACHIEVE-----------------

            var graphdata = {
                labels: _all_year_details,
                datasets: [
                    {
                        label: 'Completed',
                        yAxisID: "y-axis-1",
                        data: _ontime,
                        backgroundColor: 'RGBA(82, 190, 128 ,.4)',
                        borderWidth: 1,
                        borderColor: "#52BE80"
                    },
                    {
                        label: 'Delayed',
                        yAxisID: "y-axis-1",
                        data: _delayed,
                        backgroundColor: 'RGBA(118, 68, 138,.6)',
                        borderWidth: 1,
                        borderColor: "#76448A",
                    },
                    {
                        label: 'On-going',
                        yAxisID: "y-axis-1",
                        data: _ongoing,
                        backgroundColor: 'RGBA(231, 76, 60,.6)',
                        borderWidth: 1,
                        borderColor: "red",
                    },
                    {
                        label: 'Unassigned',
                        yAxisID: "y-axis-1",
                        data: _unassigned,
                        backgroundColor: 'RGBA(95, 0, 14,.6)',
                        borderWidth: 1,
                        borderColor: "#5f000e",
                    },
                ]
            };

            //Chart
            var ctx = document.getElementById("alljobyearChart");

            var swLineChart = new Chart(ctx, {
                type: 'bar',
                data: graphdata,
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: "All JO Created Process By Year"
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            stacked: true,
                            barPercentage: 0.6
                        }],
                        yAxes: [{
                            type: "linear", // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                            stacked: true,
                            display: true,
                            position: "left",
                            id: "y-axis-1",
                            ticks: {
                                beginAtZero: true,
                                suggestedMin: 0,
                                suggestedMax: 10,
                                min: 0
                            }
                        }, {
                            type: "linear", // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                            display: false,
                            id: "y-axis-2",
                            ticks: {
                                beginAtZero: true,
                                suggestedMin: 0,
                                suggestedMax: 10,
                                min: 0
                            }
                        }],
                    },
                    animation: {
                        duration: 3000,
                    }
                }
            });

        }
    });

    _all_year_details = [];
    _all_count_jo_yr_details = [];

}

function KPIHardwarePercentage() {

    $.ajax({
        url: '/JobOrders/HardwarePercentage',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Result) {

            var goalpercentage = 0;
            for (var i in Result) {
                _hw_avg_standartime.push(Result[i].AvgStandardTime);
                _hw_avg_actualtime.push(Result[i].AvgActualTime);

                goalpercentage = Result[i].GoalPercentage;
            }
            // --------------------------- AVG RESOLUTION --------------

            var ctx = document.getElementById("resolutionChart").getContext('2d');

            var hwLineChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ["Avg Standard Time", "Avg Actual Time"],
                    datasets: [{
                        backgroundColor: [
                            "#0B5345",
                            "#239B56"
                        ],
                        data: [_hw_avg_standartime, _hw_avg_actualtime],
                    }]
                },
            });

            $("#dv-progress").attr("aria-valuenow", goalpercentage).css("width", goalpercentage + "%");
            $("#lbl-progress").text(goalpercentage + "%");
        }
    });
}
function satisfactoryRate() {


    $.ajax({
        url: '/JobOrders/SatisfactoryRate',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Result) {

            var goalpercentage = 0;
            for (var i in Result) {
                _satisfactory_rate.push(Result[i].Satisfied5);
                _satisfactory_rate.push(Result[i].Satisfied4);
                _satisfactory_rate.push(Result[i].Neutral);
                _satisfactory_rate.push(Result[i].Dissatisfied2);
                _satisfactory_rate.push(Result[i].Dissatisfied1);
            }
            console.log(_satisfactory_rate);
            //----------- SATISFACTORY CHART -------------------------
            var ctx = document.getElementById("satisfactoryChart");

            totalchart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: ["Very Satistied", "Somewhat Satisfied", "Neutral", "Somewhat Dissatisfied", "Very Dissastified"],
                    datasets: [
                        {
                            label: "Total Ratings",
                            data: _satisfactory_rate,
                            backgroundColor: 'RGBA(230, 126, 34 ,.6)',
                            borderWidth: 1,
                            borderColor: "#A04000",
                        }
                    ]
                },
                options: {
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Cost Center'
                            },
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Total Minutes',
                                events: ['click']
                            }
                        }]
                    },
                    animation: {
                        duration: 4000,
                    }
                },
                animateScale: true
            });
        }
    });

}

function allApcJo() {

    $.ajax({
        url: '/JobOrders/AllApcJo',
        type: 'POST',
        /*data: JSON.stringify(
            {
                Year: year
            }),*/
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Result) {

            $("#tb-jotype > tr").remove();

            for (var i in Result) {
                _apc_name.push(Result[i].APC);
                _alljo.push(Result[i].TotalJO);

                var row = $("<tr>");

                row.append($("<td style='width: 55%;'data-bs-toggle='tooltip' data-bs-placement='top' title='" + Result[i].APC + "'><img src='" + Result[i].EmpID +
                    "' style='max-width:80px;max-height:80px;' class='rounded-circle'><h6>" + Result[i].APC + "</h6></td>"))
                    .append($("<td><h5><span class='badge badge-pill badge-info rounded-circle'>" + Result[i].HW + "</span></h5></td>"))
                    .append($("<td><h5><span class='badge badge-pill badge-dark'>" + Result[i].SW + "</span></h5></td>"))

                $("#tbl-jotype tbody").append(row);
            }

            //Chart
            var ctx = document.getElementById("jobyAssigneeChart");
            var hwLineChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: _apc_name,
                    datasets: [
                        {
                            data: _alljo,
                            label: "APC Personnel JO",
                            borderColor: "#154360",
                            borderWidth: 2,
                            backgroundColor: "RGBA(21, 67,96,.6)",
                            fill: true
                        },
                    ]
                },
                options: {
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'APC'
                            },
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Total'
                            }
                        }]
                    },
                    animateScale: true,
                    plugins: {
                        filler: {
                            propagate: true
                        }
                    },
                    responsive: true,
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,
                    },
                    animation: {
                        duration: 6000,
                    }
                }
            });
        }
    });

}

function resoureGraph() {

    var apc = [];
    var utilization_JO = [];
    var utilization_report = [];

    $.ajax({
        url: '/JobOrders/ResourceUtilization',
        type: 'POST',
        /*data: JSON.stringify(
            {
                Year: year
            }),*/
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            // $("#tblverlay").show();
        },
        success: function (Utilization) {

            for (var i in Utilization) {

                var joutilization = 0;
                var reportsutilization = 0;

                apc.push(Utilization[i].APC);

                if (Utilization[i].JOCount > 0) {

                    joutilization = (((Utilization[i].JOCount) * (Utilization[i].JOActualTime / Utilization[i].JOCount))/12420)*100
                }
                if (Utilization[i].ReportCount > 0) {
                    reportsutilization = (((Utilization[i].ReportCount) * (Utilization[i].ReportActualTime / Utilization[i].ReportCount)) / 12420) * 100
                }

                utilization_JO.push(parseInt(joutilization));
                utilization_report.push(parseInt(reportsutilization));
            }

            var graphdata = {
                labels: apc,
                datasets: [
                    {
                        label: 'Job Order',
                        yAxisID: "y-axis-1",
                        data: utilization_JO,
                        backgroundColor: 'RGBA(102, 0, 0,.7)',
                        borderWidth: 1,
                        borderColor: "#660000"
                    },
                    {
                        label: 'Daily Report',
                        yAxisID: "y-axis-1",
                        data: utilization_report,
                        backgroundColor: 'RGBA(207, 17, 17 ,.8)',
                        borderWidth: 1,
                        borderColor: "#7C0101",
                    }
                ]
            };

            //Chart
            var ctx = document.getElementById("resourceChart");

            var swLineChart = new Chart(ctx, {
                type: 'bar',
                data: graphdata,
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: "APC Resource Utilization JO And Daily Report"
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            stacked: true,
                            barPercentage: 0.6
                        }],
                        yAxes: [{
                            type: "linear", // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                            stacked: true,
                            display: true,
                            position: "left",
                            id: "y-axis-1",
                            ticks: {
                                beginAtZero: true,
                                suggestedMin: 0,
                                suggestedMax: 10,
                                min: 0
                            }
                        }, {
                            type: "linear", // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                            display: false,
                            id: "y-axis-2",
                            ticks: {
                                beginAtZero: true,
                                suggestedMin: 0,
                                suggestedMax: 10,
                                min: 0
                            }
                        }],
                    }
                }
            });
          
        }
    });
 
}


function downtimeChart() {

    var ctx = document.getElementById("downtimeChart");
    var hwLineChart = new Chart(ctx, {
        type: 'bar',
        data: {
            datasets: [
                {
                    data: [0, 0, 0, 0, 0, 0, 1411, 10,1193,1272,0,0],
                    label: "Total Downtime",
                    borderColor: "#7C0606",
                    borderWidth: 2,
                    backgroundColor: "RGBA(255, 14, 14 ,.4)",
                    fill: true
                },
                {
                    label: 'Target Resolution',
                    data: [1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200   ],
                    type: 'line',
                    // this dataset is drawn on top
                    order: 2,
                    borderColor: "#0024C6",
                    borderWidth: 3,
                    fill: false
                }
            ],
            labels: ["JAN", "FEB", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUG", "SEPT", "OCT", "NOV", "DEC"],

        },
        options: {
            tooltips: {
                mode: 'label',
                bodySpacing: 10,
                cornerRadius: 0,
                titleMarginBottom: 15,
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Monthly Downtime Report'
                    },
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Total'
                    }
                }]
            },
            animateScale: true,
            plugins: {
                filler: {
                    propagate: true
                }
            },
            responsive: true,
            tooltips: {
                mode: 'label',
                bodySpacing: 10,
                cornerRadius: 0,
                titleMarginBottom: 15,
            },
            animation: {
                duration: 6000,
            }
        }
    });

}