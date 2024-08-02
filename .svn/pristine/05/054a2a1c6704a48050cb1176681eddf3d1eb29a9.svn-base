
var _Field = "";
var _JobNo = "";
var _totalOngoing = 0;
var _transactiontype = "";

$(function () {
    $("#txtDateTarget").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});


window.onload = function () {

    loadApcJo();
    loadAllDepartment();
    latestAvailableAPC();

    if ($("#lblAvailable").text() == "Yes")
        $("#rdEnable").prop("checked", true);
    else
        $("#rdDisable").prop("checked", true);
    
}

var currentValue = "";

function JOAvailability(myRadio) {

    currentValue = myRadio.value;

    $.ajax({
        url: '/JobOrders/UpdateJOCreationAvailability',
        type: 'POST',
        data: JSON.stringify(
            {
                Value: currentValue,
                NewMessage: $("#txtNewMessage").val(),
                TempAccess: $("#txtTempEmpName").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Message) {

            if (Message !== "")
                alert(Message);
        },
    });
}


function UpdateMessage() {

    var CurrentStatus = "";

    if ($("#rdEnable").checked)
        CurrentStatus = "Yes";
    else
        CurrentStatus = "No";

    $.ajax({
        url: '/JobOrders/UpdateJOCreationAvailability',
        type: 'POST',
        data: JSON.stringify(
            {
                Value: CurrentStatus,
                NewMessage: $("#txtNewMessage").val(),
                TempAccess: $("#txtTempEmpName").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        onBegin: function () {
            $("#dvPleaseWait").show();
            $("#dvPleaseWait1").show();
        },
        success: function (Message) {

            if (Message !== "")
                alert(Message);
            else {
                $("#alertSave").show();
                $("#alertSave1").show();
            }
        },
    });

}
function latestAvailableAPC() {

    $.ajax({
        url: '/JobOrders/LatestAvailableAPC',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (APC) {

            var textcolor = "";
            for (var i in APC) {

                if (APC[i].Datetarget == "Available ")
                    textcolor = "badge bg-success w3-text-white";
                else
                    textcolor = "badge bg-info w3-text-white";

                $(".list-group").append($('<li class="list-group-item list-group-item-action" data-toggle="tooltip" data-placement="TOP" title="' + APC[i].ApcName + '" style="padding: 2px 1px 2px 0px;"></li>')
                    .html("<img src='/APCImages/" + APC[i].Assignee + "' class='rounded-circle' width='40'/>" +
                        "<span class='float-right" + textcolor + "'>" + APC[i].Datetarget + "</span>"));
            }
        },
    });
}

function loadApcJo() {

    var devname = [];
    var achieve = [];
    var unachieve = [];
    var onqueue = [];

    $.ajax({
        url: '/JobOrders/APCOngoingJo',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (OngoingJO) {

            for (var i in OngoingJO) {

                devname.push(OngoingJO[i].Developer);
                achieve.push(OngoingJO[i].DevTotal);
                unachieve.push(OngoingJO[i].DueOngoing);
                onqueue.push(OngoingJO[i].Assessment);
            }


            var ctx = document.getElementById("graph-ongoing-jo");

            var allachievemnts = {
                label: 'Assigned JO',
                data: achieve,
                backgroundColor: 'RGBA(0, 227, 210 ,.8)',
                borderWidth: 1,
            };

            var allunachieve = {
                label: 'Due JO',
                data: unachieve,
                backgroundColor: 'RGBA(255, 0, 0 ,.7)',
                borderWidth: 1,
            };
            var allonqueue = {
                label: 'Under Assessment',
                data: onqueue,
                backgroundColor: 'RGBA(241, 241, 5,.7)',
                borderWidth: 1,
            };

            var alljo = {
                labels: devname,
                datasets: [allachievemnts, allunachieve, allonqueue]
            };

            var hwBarchart = new Chart(ctx, {
                type: 'bar',
                data: alljo,
                options: {
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 1,
                            categoryPercentage: 0.6
                        }],
                        yAxes: [{
                            // beginAtZero: true,
                            stepSize: 1,
                            ticks: {
                                precision: 0
                            }
                        }]
                    },
                    'onClick': function (evt, item) {
                        var activePoints = hwBarchart.getElementsAtEvent(evt);
                        if (activePoints[0]) {
                            var chartData = activePoints[0]['_chart'].config.data;
                            var idx = activePoints[0]['_index'];

                            var label = chartData.labels[idx];
                            showAssigneeJo(label);
                        }
                    },
                    responsive: true,
                }
            });


        },
    });

    devname = [];
    achieve = [];
    unachieve = [];
}

function loadAllDepartment() {

    var deptname = [];
    var jocount = [];
    var joDelayed = [];

    $.ajax({
        url: '/JobOrders/DepartmentOnGoingJO',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Department) {

            for (var i in Department) {

                deptname.push(Department[i].DeptName);
                jocount.push(Department[i].Count);
                joDelayed.push(Department[i].DelayedJo);
            }

            var ctx = document.getElementById("graph-dept-ongoingjo");

            var allachievemnts = {
                label: 'On-Going JO',
                data: jocount,
                backgroundColor: 'RGBA(12, 186, 227, .7)',
                borderWidth: 1,
            };

            var allunachieve = {
                label: 'Due JO',
                data: joDelayed,
                backgroundColor: 'red',
                borderWidth: 0,
                backgroundColor: 'RGBA(205, 3, 3,.7)',
                borderWidth: 1,
            };

            var alljo = {
                labels: deptname,
                datasets: [allachievemnts, allunachieve]
            };

            var hwBarchart = new Chart(ctx, {
                type: 'bar',
                data: alljo,
                options: {
                    tooltips: {
                        mode: 'label',
                        bodySpacing: 10,
                        cornerRadius: 0,
                        titleMarginBottom: 15,
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 1,
                            categoryPercentage: 0.6,
                            ticks: {
                                precision: 0
                            }
                        }],
                        yAxes: [{
                            // beginAtZero: true,
                            stepSize: 1,

                        }],

                    },
                    'onClick': function (evt, item) {
                        var activePoints = hwBarchart.getElementsAtEvent(evt);
                        if (activePoints[0]) {
                            var chartData = activePoints[0]['_chart'].config.data;
                            var idx = activePoints[0]['_index'];

                            var label = chartData.labels[idx];
                            showAllDeptJo(label);
                        }
                    },
                    responsive: true,
                }
            });

        },
    });

    deptname = [];
    jocount = [];
    joDelayed = [];
}

function showAssigneeJo(assignee) {

    $("#tbloverlay").show();
    $.ajax({
        url: '/JobOrders/SearchAssigneeJOs',
        type: 'POST',
        data: JSON.stringify(
            {
                Assignee: assignee,
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (JO) {

            $('#tb-assignee-jos').empty();

            for (var i in JO) {

                var progressbar = JO[i].JoProgress + "%";
                var row = JO[i].IsDue == 1 ? $("<tr class='bg-danger w3-text-white'>") : $("<tr class='w3-hover-blue-gray'>");

                row.append($("<td>" + assignee + "</td>"))
                    .append($("<td>" + JO[i].Details + "</td>"))
                    .append($("<td>" + JO[i].Department + "</td>"))
                    .append($("<td>" + JO[i].Datetarget + "</td>"))
                    .append($("<td>" + JO[i].ReasonOfDelay + "</td>"))
                    .append($("<td>" + progressbar + "</td> "))
                    .append($("<td>" + JO[i].DateProgress + "</td>"))
                    .append($("<td><a href='/JobOrders/JobOrderDetails?JoNumber=" + JO[i].JobNo + "#AllJo' class='w3-btn w3-black'><i class='fa fa-pencil'></i></a></td>"));

                $("#tbl-assignee-jos tbody").append(row);
            }
        },
    });
    $("#tbloverlay").hide();
}

function showAllDeptJo(department) {

    $.ajax({
        url: '/JobOrders/JoByDeptDetails',
        type: 'POST',
        data: JSON.stringify(
            {
                DeptName: department,
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {
            $('#tb-assignee-jos').empty();

            for (var i in Result) {

                var progressbar = Result[i].JoProgress + "%";
                var row = $("<tr class='w3-hover-blue-gray'>");

                //append($("<td style='width:150px;'>" + JO[i].JobNo + "</td>"))

                row.append($("<td>" + Result[i].Assignee + "</td>"))
                    .append($("<td>" + Result[i].Details + "</td>"))
                    .append($("<td>" + Result[i].Department + "</td>"))
                    .append($("<td>" + Result[i].Datetarget + "</td>"))
                    .append($("<td>" + Result[i].ReasonOfDelay + "</td>"))
                    .append($("<td>" + progressbar + "</td>"))
                    .append($("<td>" + Result[i].DateProgress + "</td>"))
                    .append($("<td><a href='/JobOrders/JobOrderDetails?JoNumber=" + Result[i].JobNo + "#AllJo' class='w3-btn w3-black'><i class='fa fa-pencil'></i></a></td>"));

                $("#tbl-assignee-jos tbody").append(row);
            }
        },
    });
}

function updateAssignedJO() {

    $.ajax({
        url: '/JobOrders/UpdateUnassignedJO',
        type: 'POST',
        data: JSON.stringify(
            {
                JobNo: $("#txtJONum").val(),
                Assignee: $("#txtAssignee").val(),
                Datetarget: $("#txtDateTarget").val(),
                Message: "",
                JOType: $("#txtSoftware").val() !== "" ? "8" : $("#ddHardwares").val(),
                CodeNumber: $("#txtSoftware").val() !== "" ? $("#txtSoftware").val() : "hw",
                StatusID: $("#txtStatus").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $(".md-confirmation").modal("hide");
            $("#alrtAssigning").show();
        },
        success: function (Result) {

            $("#alrtAssigned").show();
            $("#alrtAssigning").fadeOut();
            if (Result)
                reloadPage();
            else
                alert("Something went wrong.");
        },
        complete: function () {
            $("#txtSearchJONum").val("");
            $("#txtSearchJONum").focus();
        }
    });
}

function OpenJODetails(JobNo) {

    $.ajax({
        url: '/JobOrders/SearchUnassignedJO',
        type: 'POST',
        data: JSON.stringify(
            {
                JobNo: JobNo,
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (JO) {

            $(".assignjo").modal("show");

            for (var i in JO) {

                $("#txtJONum").val(JO[i].JobNo);
                $("#txtRequestorName").val(JO[i].Requestor);
                $("#txtDatefiled").val(JO[i].DateFiled);
                $("#txtDetails").val(JO[i].Details);
            }
        },
        //    .append($("<td><input type='text' class='form-control' id='txt-date-target' onchange='assignJo(" + ID + ", 2)'/></td>"));
        complete: function () {
            $("#tblverlay").hide();
        }
    });

    $.ajax({
        url: '/JobOrders/AllHardwareCategory',
        type: 'POST',
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (data) {

            for (var i in data) {

                var slctCategory = document.getElementById("ddHardwares");
                //Populate the details to select option
                var option = document.createElement("option");
                option.text = data[i].details;
                option.value = data[i].type_id;
                slctCategory.add(option);
            }
        },
        complete: function () {
            $("#tblverlay").hide();
        }
    });
}

function SearchSelectedJO() {

    $.ajax({
        url: '/JobOrders/SearchUnassignedJO',
        type: 'POST',
        data: JSON.stringify(
            {
                JobNo: $("#txtJobOrder").val(),
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (JO) {

            $('#tbongoingjo').empty();
            var row = $("<tr class='w3-hover-blue-gray'>");

            for (var i in JO) {

                row.append($("<td>" + JO[i].JobNo + "</td>"))
                    .append($("<td>" + JO[i].Details + "</td>"))
                    .append($("<td>" + JO[i].DateFiled + "</td>"))
                    .append($("<td>" + JO[i].Requestor + "</td>"))
                    .append($('<td><a class="btn btn-dark" onclick="OpenJODetails(\'' + JO[i].JobNo + '\')"><i class="fa fa-pencil"></i></a></td>'));


                $("#tblongoingjo tbody").append(row);
            }
        },
        complete: function () {
            $("#tblverlay").hide();
        }
    });
}

function apcNames() {
    var options = "";
    var count = $("#lbl-apc-count").text();

    for (var i = 1; i <= count; i++) {

        options += "<option value = '" + $('#lbl-apc-' + i).text() + "'>" + $("#lbl-apc-" + i).text() + "</option>";
    }
    return options;
}

function exportToExcel() {

    var url = "/JobOrders/UnassignedToExcel";
    window.location.href = url;

}



var isForDeffered = true;
function showDefferedJo() {

    if (isForDeffered == true) {
        $("#tbl-unassigned-jo").hide();
        $("#tbl-deffered-jo").show();

        //Button
        $("#btnDisplay").removeClass("btn-danger");
        $("#btnDisplay").addClass("btn-info");

        document.getElementById('btnDisplay').innerHTML = '<i class="fa fa-thumbs-o-up fa-2x"></i> For Approval JO';

        isForDeffered = false;
    } else {

        $("#tbl-unassigned-jo").show();
        $("#tbl-deffered-jo").hide();

        $("#btnDisplay").removeClass("btn-info");
        $("#btnDisplay").addClass("btn-danger");

        document.getElementById('btnDisplay').innerHTML = '<i class="fa fa-hand-stop-o fa-2x"></i> Deferred JO';

        isForDeffered = true;
    }
}

function saveNewMessage() {

    var MyDiv2 = document.getElementById('txtMessage');
    MyDiv2.innerHTML = MyDiv2.innerHTML;

    $.ajax({
        url: '/JobOrders/InsertMessages',
        type: 'POST',
        data: JSON.stringify(
            {
                Message: MyDiv2.innerHTML
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (Result) {

            if (Result == "")
                populateNewMessage();
        },
        complete: function () {
            $("#tblverlay").hide();
        }
    });

}

function populateNewMessage() {

    var select = document.getElementById('slctMessages');
    var row = $("<tr>");

    //populate to table
    row.append($("<td style='width:80px;'>" + (parseInt($("#lblMessageID").text()) + 1) + "</td>"))
        .append($("<td style='flaot:left;'>" + $("#txtMessage").val() + "</td>"));
    $("#tblOnHoldMessages tbody").append(row);

    //populate to select dropdown list
    var opt = document.createElement('option');
    opt.value = $("#txtMessage").val();
    opt.innerHTML = $("#txtMessage").val();
    select.appendChild(opt);

    $("#alrIsHold").fadeIn("slow");
}

function displayAddMessage() {
    $(".mdNewMessage").modal("show");
}
