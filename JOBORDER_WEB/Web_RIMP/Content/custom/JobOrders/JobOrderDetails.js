﻿var _NewStatus = 0;
var _ProceedStatus = false;

$(function () {
    $("#txtDateRendered").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#txtDateIssue").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});


$(document).ready(function () {

    //status load
    switch ($("#lblStatusID").text()) {

        case "Created":
            $("#liCreated").addClass("active");
            _NewStatus = 1;
            break;
        case "Under Assessment":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("active");

            _NewStatus = 2;
            break;
        case "Pending In-charge Assignment":

            _NewStatus = 3;

           
            break;
        case "Request In-charge Assigned":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("active");

            _NewStatus = 4;

            //Hardware joborder -- no need the other processes
            if ($("#lblTypeID").text() !== "8") {
                _NewStatus = 12;               
            }
            break;
        case "On Hold-Lacking Request Documents":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("active");

            _NewStatus = 5;

            $('#txtDiagnosis').attr("required");
            $('#txtActionTaken').attr("required");

            break;
        case "For Review":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("completed");
            $("#liForReview").addClass("active");

            $("#txtDiagnosis").attr('required', 'true'); 
            _NewStatus = 6;

            break;
        case "Rejected":
            _NewStatus = 7;        
            break;
        case "For Proposal":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("completed");
            $("#liForReview").addClass("completed");
            $("#liForProposal").addClass("active");

            _NewStatus = 8;

            break;
        case "For Development":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("completed");
            $("#liForReview").addClass("completed");
            $("#liForProposal").addClass("completed");
            $("#liForDevelopment").addClass("active");

            _NewStatus = 10;

            break;
        case "Testing":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("completed");
            $("#liForReview").addClass("completed");
            $("#liForProposal").addClass("completed");
            $("#liForDevelopment").addClass("completed");
            $("#liTesting").addClass("active");

            _NewStatus = 11;

            break;
        case "Closed":
            $("#liCreated").addClass("completed");
            $("#liAssessment").addClass("completed");
            $("#liAssigned").addClass("completed");
            $("#liDocuments").addClass("completed");
            $("#liForReview").addClass("completed");
            $("#liForProposal").addClass("completed");
            $("#liForDevelopment").addClass("completed");
            $("#liTesting").addClass("completed");
            $("#liCompleted").addClass("completed");

            _NewStatus = 12;

            break;
    }

    //Enable button after created status, effective only to apc personnel
    if ($("#lblLoginDept").text().trim() == "ASIA PACIFIC CENTER" && $("#lblStatusID").text().trim() !== "Created") {
        $("#btnNext").removeClass("disabled");
    }
});


$(document).ready(function () {

    $(".next").click(function () {
        if ($(".step-wrapper li:last-child").hasClass('completed')) {
            alert("completed");
            return
        }
        _ProceedStatus = true;
        $(".changestatus").modal("show");

        $(".step-wrapper li.active").addClass("completed").removeClass("active").next('li').addClass("active");
    });

    $(".previous").click(function () {
        if ($(".step-wrapper li:first-child").hasClass('active')) {
            alert("Already on first step");
            return
        }
        $(".step-wrapper li.active").removeClass("active completed").prev('li').addClass("active").removeClass('completed');

        if ($(".step-wrapper li:last-child").hasClass('completed')) {
            $(".step-wrapper li:last-child").removeClass('completed').addClass('active')
        }

        _ProceedStatus = false;
        $(".changestatus").modal("show");
    });
});


function changeStatus() {

    if (_ProceedStatus) {

        if (_NewStatus == 6 || _NewStatus == 8 || _NewStatus == 2) // overlap rejected/for revision status
            _NewStatus++;

        _NewStatus++;
    }
    else
        _NewStatus--

    updateJobOrder();
    $(".changestatus").modal("hide");

}

function updateJobOrder() {

   
    //Update application
    $.ajax({
        url: '/JobOrders/UpdateJobOrderDetails',
        type: 'POST',
        data: JSON.stringify(
            {
                JobNoID: $("#lblJobNoID").text(),
                DetailsID: $("#lblDetailsID").text(),
                Details: $("#txtJODetails").val(),
                Purpose: $("#txtJOPurpose").val(),
                NotedBy: $("#notedbyID").val(),
                ApprovedBy: $("#approvedByID").val(),
                StatusID: _NewStatus,
                Progress: parseInt($("#txtProgressRate").val()),
                Diagnosis: $("#txtDiagnosis").val(),
                ActionTaken: $("#txtActionTaken").val(),
                ReasonOfDelay: $("#txtReasonOfDelay").val(),
                ActualTime: $("#lblActualTime").text() == "" ? 0 : parseInt($("#lblActualTime").text()),
                DocumentMeetings: $("#txtMeetingsRemarks").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function () {
            $(".bs-example-modal-confirmation").modal("show");
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

function printJobOrder() {
    //Variables
    var jonumber = $("#lblJobNo").text();
    var datefiled = $("#lblDateFiled").text();
    var category = $("#lblJOType").text();
    var categorytype = "";// $("#txtcategoryid").val();
    var standardtime = $("#lblStandardTime").text();
    var requestor = $("#lblRequestor").text();
    var department = $("#lblDepartment").text();
    var workdetails = $("#txtJODetails").val();
    var purpose = $("#txtJOPurpose").val();
    var notedby = $("#notedbyID").val();
    var approvedby = $("#approvedByID").val();
    var apcapproval = "BALUNDO, ALFRED C.";
    var reqlocal = "Requestor Local #: " + $("#txtlocalnum").val();
    var costcenter = $("#lblCostCenter").text();
    var apcincharge = $("#lblAssignee").text();// $("#slctIncharge").val() != 'Unassigned' ? $("#slctIncharge").val() : '';

    var doc = new jsPDF('', '', 'a4');
    doc.setFontSize(12);

    var img = new Image()
    img.src = '/Barcodes/' + jonumber + '.png';

    doc.setFontType("bold");
    doc.text(5, 10, 'APC Job Order Print Form');
    doc.text(5, 15, 'RICOH Imaging Products (Philippines) Corporation');
    doc.text(5, 20, '4th St., Block C4, MEZ1 Lapu-Lapu City 6015');
    doc.text(5, 25, 'Tel (6332) 3400505 / 495-4747 Fax (6332) 3400500 501');

    doc.setFontType("normal");

    //Font properties For specific text
    doc.setFontSize(17);
    doc.setFont("times");
    doc.setFontType("bold");
    doc.text(75, 35, 'APC JOB ORDER FORM');
    //END

    //Font properties for all
    doc.setFontSize(12);
    doc.setFontType("normal");
    doc.setTextColor(0, 0, 0);

    doc.text(85, 40, 'ASIA PACIFIC CENTER');

    //Job Order No.
    doc.setFontType("bold");
    doc.text(5, 48, 'Job Order No.:');
    doc.text(35, 48, jonumber);
    doc.line(35, 49, 80, 49);
    //Date filed
    doc.setFontType("bold");
    doc.text(125, 48, 'Date Filed:');
    doc.setFontType("normal");
    doc.text(154, 48, datefiled);

    //Category
    doc.setFontType("bold");
    doc.text(5, 53, 'Category:');
    doc.setFontType("normal");
    doc.text(35, 53, category);

    //Categoy type
    doc.setFontType("bold");
    doc.text(125, 53, 'Type:');
    doc.setFontType("normal");
    var category = doc.splitTextToSize(categorytype, 70);
    doc.text(155, 53, category);

    //Code number
    doc.setFontType("bold");
    doc.text(5, 58, 'Standard time:');
    doc.setFontType("normal");
    doc.text(15, 53, standardtime);

    //Standard time
    doc.setFontType("bold");
    doc.text(125, 58, 'Cost Center:');
    doc.setFontType("normal");
    doc.text(155, 58, costcenter);

    //Requestor
    doc.setFontType("bold");
    doc.text(5, 65, 'Requestor:');
    doc.text(35, 65, requestor);
    doc.line(35, 67, 80, 67);

    //Department
    doc.setFontType("bold");
    doc.text(125, 65, 'Department');
    doc.setFontType("normal");
    doc.text(155, 65, department);
    //Work details

    doc.setFontSize(13);
    doc.setFontType("bold");
    doc.text(5, 72, 'Work Details:(Requestor)');
    doc.setFontSize(12);
    doc.setFontType("normal");
    var wrapdetails = doc.splitTextToSize(workdetails, 180);
    doc.text(5, 77, wrapdetails);

    //Purpose
    doc.setFontSize(13);
    doc.setFontType("bold");
    doc.text(5, 150, 'Purpose:(Requestor)');
    doc.setFontSize(12);
    doc.setFontType("normal");
    var wrappurpose = doc.splitTextToSize(purpose, 180);
    doc.text(5, 155, wrappurpose);

    //Diagnosis of the problem
    doc.setFontSize(13);
    doc.setFontType("bold");
    doc.text(5, 170, 'Diagnosis of the problem:(APC Department)');
    doc.setFontSize(12);
    doc.setFontType("normal");
    doc.text(5, 155, '');

    //Action taken
    doc.setFontSize(13);
    doc.setFontType("bold");
    doc.text(5, 200, 'Action Taken:(APC Department)');
    doc.setFontSize(12);
    doc.setFontType("normal");
    doc.text(5, 200, '');

    doc.setFontSize(13);
    doc.text(5, 230, '* To be filled up by requesting department:');
    doc.setTextColor(0, 0, 0);
    //Requested by
    doc.setFontType("bold");
    doc.text(5, 235, 'Requested By:');
    doc.text(85, 235, 'Noted By:');
    doc.text(155, 235, 'Approved By:');

    //Requested by
    doc.setFontType("normal");
    doc.text(5, 255, requestor);
    doc.line(55, 256, 5, 256);

    //Noted by
    doc.text(85, 255, notedby);
    doc.line(135, 256, 85, 256);

    //Approved by
    doc.text(155, 255, approvedby);
    doc.line(210, 256, 155, 256);

    doc.setFontSize(10);
    doc.text(5, 260, reqlocal);
    doc.setFontSize(13);
    doc.setTextColor(0, 0, 0);
    doc.setFontType("bold");
    doc.text(5, 267, 'Noted By:');
    doc.text(85, 267, 'Approved By:');
    doc.text(155, 267, 'In-Charge:');

    //Noted by
    doc.setFontType("normal");
    doc.text(5, 282, apcapproval);
    doc.line(55, 283, 5, 283);

    //Approved by
    doc.text(85, 282, apcapproval);
    doc.setFontType("bold");
    doc.line(135, 283, 85, 283);

    //In-charge
    doc.setFontType("normal");
    doc.text(155, 282, apcincharge);
    doc.setFontType("bold");
    doc.line(210, 283, 155, 283);
    //doc.line(155, 283, 55, 283);

    doc.setFontType("bold");
    doc.text(155, 295, '7001-H-0012 Form1 rev0');

    img.onload = function () {

        doc.addImage(img, 'png', 180, 5, 20, 20);

        var filename = jonumber + ".pdf";
        doc.save(filename);
        doc.save(filename);

    };

}

$(document).on('click', '.number-spinner button', function () {
    var btn = $(this),
        oldValue = btn.closest('.number-spinner').find('input').val().trim(),
        newVal = 0;

    if (btn.attr('data-dir') == 'up') {
        newVal = parseInt(oldValue) + 1;
    } else {
        if (oldValue > 1) {
            newVal = parseInt(oldValue) - 1;
        } else {
            newVal = 1;
        }
    }
    btn.closest('.number-spinner').find('input').val(newVal);
    $('.progress-bar').css('width', newVal + '%').attr('aria-valuenow', newVal);
    $('#lbl-progress').text(newVal + "%");

    updateJobOrder();
});

function AddTimeAllocation() {

    //populate time allocation to actual time
    var timeallocation = parseInt($("#txtTimeRendered").val()) + parseInt($("#lblActualTime").text());
    $("#lblActualTime").text(timeallocation);

    $.ajax({
        url: '/JobOrders/AddTimeAllocation',
        type: 'POST',
        data: JSON.stringify(
            {
                DetailsID: $("#lblDetailsID").text(),
                DateRendered: $("#txtDateRendered").val(),
                MinRendered: $("#txtTimeRendered").val(),
                Remarks: $("#txtRemarks").val()
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function () {
            //Update joborder actual  time
            updateJobOrder();
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
            alert("[changeCategory()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });
}

function addIssue(type) {

    alert("Successfully change status.");

    $.ajax({
        url: '/JobOrders/AddIssue',
        type: 'POST',
        data: JSON.stringify(
            {
                DetailsID: $("#lblDetailsID").text(),
                Date: $("#txtDateIssue").val(),
                Issue: $("#txtIssueDetails").val(),
                Status: type == "Add" ? "Ongoing" : $("#drdStatus").val(),
                TransType: type
            }),
        dataType: "json",
        traditional: true,
        contentType: 'application/json;',
        cache: false,
        success: function (msg) {

            if (msg !== "")
                alert(msg);

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
            alert("[changeCategory()] \n Oops, something went wrong.\n Error: " + msg);
        }
    });        
}

