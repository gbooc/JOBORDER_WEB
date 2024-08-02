var ccname = [];
var ccount = [];
var percent = [];

var totalchart = null;
var percentagechart = null;

$(function () {
    $("#txt-datestart").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#txt-dateend").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    searchMonthReport();
});

function searchMonthReport() {

    var from = $("#txt-datestart").val();
    var end  = $("#txt-dateend").val();

    ccname = [];
    ccount = [];
    percent = [];

    $.ajax({
        url: '/JobOrders/GetMonthyReport',
        type: 'POST',
        data: JSON.stringify(
            {
                From: from,
                To: end
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {

            for (var i in Result) {

                ccname.push(Result[i].cc_description);
                ccount.push(Result[i].total);
            }
            loadChart();
        },
    });

    $.ajax({
        url: '/JobOrders/GetPieMonthlyReport',
        type: 'POST',
        data: JSON.stringify(
            {
                From: from,
                To: end
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Graph) {

            var totalmins = 0;
            for (var i in Graph) {
                totalmins = Graph[i].overalltotal;

                var percentage = parseFloat((parseInt(Graph[i].total) / totalmins) * 100);
                percent.push(Math.round(percentage,2));
            }
            loadChart();
        },
    });

    $.ajax({
        url: '/JobOrders/GetExportedJO',
        type: 'POST',
        data: JSON.stringify(
            {
                DateFrom: from,
                DateTo: end
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Report) {
            $("#tb-jo-details > tr").remove();

            for (var i in Report) {

                var row = $("<tr>");
                row.append($("<td>" + Report[i].jo_no + "</td>"))
                    .append($("<td>" + Report[i].department + "</td>"))
                    .append($("<td>" + Report[i].costcenter + "</td>"))
                    .append($("<td>" + Report[i].apc + "</td>"))
                    .append($("<td style='width: 120px;'>" + Report[i].hrs_mins + "</td>"))
                    .append($("<td style='width: 80px;'><a href = '/JobOrders/JobOrderDetails?JoNumber=" + Report[i].jo_no + "#AllJo' class = 'btn btn-outline-warning'><i class = 'fa fa-pencil-square'></i></a></td>"));

                $("#tbl-jo-details tbody").append(row);
            }

        },
    });

}

function loadChart() {

    var ctx = document.getElementById("cc-chart");
  
    totalchart = new Chart(ctx, {
        type: 'horizontalBar',
        data: {
            labels: ccname,
            datasets: [
                {
                    backgroundColor: "red",
                    label: "Cost Center",
                    data: ccount
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
            }
        },
        animateScale: true,

    });

    var ctx1 = document.getElementById("cc-percentage");
    percentagechart = new Chart(ctx1, {
        type: 'pie',
        data: {
            labels: ccname,
            datasets: [{
                backgroundColor: [
                    "#2ecc71",
                    "#3498db",
                    "#95a5a6",
                    "#9b59b6",
                    "#f1c40f",
                    "#e74c3c",
                    "#34495e",
                    "##7E5109",
                    "#17202A",
                    "#D6EAF8",
                    "#EDBB99",
                    "#512E5F",
                    "##C0392B"
                ],
                data: percent
            }]
        },
        animateScale: true

    });
}
function exportToExcel() {

    var Department = $("#txtUser").val();
    var ID = $("#txt-EmpID").val();
    if (Department.trim() !== "ASIA PACIFIC CENTER" && ID.trim() != "g101756")
        alert("Access denied, please contact APC Department");
    else {
        var txtdatefrom = $("#txt-datestart").val();
        var txtdateto   = $("#txt-dateend").val();

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
