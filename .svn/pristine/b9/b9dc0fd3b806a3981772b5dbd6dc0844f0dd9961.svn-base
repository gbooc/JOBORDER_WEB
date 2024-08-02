
var _approval = "";
var _jonumber = "";
var _accountcount = 0;
window.onload = function () {

    var devname = [];
    var achieve= [];
    var unachieve= [];

    var count = $("#txtDevCount").val();
    
    for (var i = 1; i < count; i++) {

        var tmpDevname = $("#lblDevname_" + i).text();
        var tmpunachieved = $("#lblExceedJo_" + i).text();
        var tmpAchieved = $("#lblAchievedJo_" + i).text();

        achieve.push(tmpAchieved);
        unachieve.push(tmpunachieved);

        devname.push(tmpDevname);

    }
    // ----------------- C H A R T S ---------------------------

    var ctx = document.getElementById("exceedJoChart");

    var allachievemnts = {
        label: 'Achieve',
        data: achieve,
        backgroundColor: 'green',
        borderWidth: 1,
        pointRadius: 0,
        fill: false
    };

    var allunachieve = {
        label: 'Unachieve',
        data: unachieve,
        backgroundColor: 'red',
        borderWidth: 1,
        pointRadius: 0,
        fill: false
       
    };

    var alljo = {
        labels: devname,
        datasets: [allachievemnts, allunachieve]
    };


    var hwBarchart = new Chart(ctx, {
        type: 'bar',
        data: alljo,
        options: {
            responsive: true,
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
                }]
            }
        }
    });
}

function displayConfirmation(approval,jonumber, count) {
    _approval = approval.value;
    _jonumber = jonumber;
    _accountcount = count;

    $(".md-confirmation").modal("show");
}

function submitApproval() {

    $.ajax({
        url: '/JobOrders/JOApproval',
        type: 'POST',
        data: JSON.stringify({
            IsApprove: _approval == "approve" ? 1 : 2,
            JONumber: _jonumber,
            ApprovalType: $("#txtAccount_" + _accountcount).val(),
            Source: "From System"
        }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $(".spinner-border").show();
        },
        success: function (Result) {

            $("#dvConfirmation").hide();
            $("#dvFooter").hide();
            $("#dvSuccess").show("slow");
            $(".spinner-border").hide();
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