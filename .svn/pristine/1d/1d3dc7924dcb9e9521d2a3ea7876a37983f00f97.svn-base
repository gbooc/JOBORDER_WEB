var _deptname = [];
var _jocounts = [];

var _closedapc = [];
var _closedcount = [];

var _delayedapc = [];
var _delayedcount = [];

var _ontimeapc = [];
var _ontimecount = [];

window.onload = function () {

    var count = $("#txt-dept-count").val();
    var completedcount = $("#txt-closed-count").val();
    var delayedcount = $("#txt-delayed-count").val();
    var ontimecount = $("#txt-ontime-count").val();

    //All department jo data
    for (var i = 1; i < count; i++) {

        var deptname = $("#lbl-deptname-" + i).text();
        var jocount = $("#lbl-jocount-" + i).text();

        _deptname.push(deptname);
        _jocounts.push(jocount);
    }

    //All completed jo data
    for (var i = 1; i < completedcount; i++) {

        var apc = $("#lbl-closed-apc-" + i).text();
        var jocount = $("#lbl-closed-count-" + i).text();

        _closedapc.push(apc);
        _closedcount.push(jocount);
    }

    //All delayed jo data
    for (var i = 1; i < delayedcount; i++) {

        var apc = $("#lbl-delayed-apc-" + i).text();
        var jocount = $("#lbl-delayed-count-" + i).text();

        _delayedapc.push(apc);
        _delayedcount.push(jocount);
    }

    //All on time jo data
    for (var i = 1; i < ontimecount; i++) {

        var apc = $("#lbl-ontime-apc-" + i).text();
        var jocount = $("#lbl-ontime-count-" + i).text();

        _ontimeapc.push(apc);
        _ontimecount.push(jocount);
    }

    displayChart();
};


function displayChart() {

    //graph by departmen
    var ctx = document.getElementById("by-dept-graph").getContext('2d');
    var hwLineChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: _deptname,
            datasets: [{
                backgroundColor: [
                    "#557c5c",
                    "#8e2b4e",                  
                    "#000dff",                  
                    "#f88379",                  
                    "#ff2255",                  
                    "#b14949",
                    "#fd3535",
                    "#245c8d"
                ],
                data: _jocounts
            }]
        }

    });

    //Graph all closed jo
    var ctx = document.getElementById("closed-jo-graph");
    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: _closedapc,
            datasets: [
                {
                    backgroundColor: "#81d8d0",
                    label: "All Closed Job Order",
                    data: _closedcount
                }
            ]
        },
        options: {
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
                        labelString: 'Count',
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

    //graph all delayed jo
    var ctx = document.getElementById("closed-delayed-graph");
    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: _delayedapc,
            datasets: [
                {
                    backgroundColor: "#fd3535",
                    label: "All Delayed Job Order",
                    data: _delayedcount
                }
            ]
        },
        options: {
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
                        labelString: 'Count',
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

    //graph all ontime jo
    var ctx = document.getElementById("closed-ontime-graph");
    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: _ontimeapc,
            datasets: [
                {
                    backgroundColor: "#000dff",
                    label: "All On-Time Job Order",
                    data: _ontimecount
                }
            ]
        },
        options: {
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
                        labelString: 'Count',
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