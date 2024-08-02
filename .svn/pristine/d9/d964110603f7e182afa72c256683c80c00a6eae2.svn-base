$(function () {
    $("#txt-datestart").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#txt-dateend").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});

function getCanteen() {

    var empid = $("#txt-idnum").val();
    var from = $("#txt-datestart").val();
    var to = $("#txt-dateend").val();

    $.ajax({
        url: '/Home/GetTransaction',
        type: 'POST',
        data: JSON.stringify(
            {
                EmpID: empid,
                From: from,
                To: to
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        beforeSend: function () {
            $("#tblverlay").show();
        },
        success: function (Result) {
            $('#tb-transaction').empty();
            var total = 0;
            for (var i in Result) {

                var row = $("<tr>");

                row.append($("<td>" + Result[i].EmpID + "</td>"))
                row.append($("<td>" + Result[i].Date + "</td>"))
                    .append($("<td>" + Result[i].Amount + "</td>"));

                $("#tbl-transaction tbody").append(row);

                total += parseInt(Result[i].Amount);
            }
            $("#lbl-total").text(total);
            $("#tblverlay").hide();
        }
    });
}