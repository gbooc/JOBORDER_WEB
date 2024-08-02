$(document).ready(function () {

    $(function () {
        $('#dateneeded').datepicker();
    });

    (function ($) {
       // $("#tblverlay").fadeIn("slow");
        setTimeout(function () {
            $("#tblverlay").fadeOut("slow");
            $("#tblContainer").fadeIn("slow");
        }, 2500);

    }(jQuery));

    var hash = window.location.hash;

    switch (hash) {

        //All Users
        case "#myJo": $("#navMyJo").addClass("link-active"); break;
        case "#unassignedJo": $("#navUnassignedJO").addClass("link-active"); break;
        case "#AllJo": $("#navAllJo").addClass("link-active"); break;
        case "#NewJo": $("#navNewJo").addClass("link-active"); break;
        case "#dashboard": $("#navDashboard").addClass("link-active"); break;
        //End

        //** APC ONLY

        //Downtime
        case "#server":
            $("#navServer").addClass("link-active");
            $('#dv-downtime').show(); break;
        case "#network":
            $("#navNetwork").addClass("link-active");
            $('#dv-downtime').show(); break;
        case "#telephone":
            $("#navTel").addClass("link-active");
            $('#dv-downtime').show(); break;

        //Maintenance
        case "#preventive":
            $("#NavMPreventive").addClass("link-active");
            $('#dv-Maintenance').show(); break;
        case "#Maintenance":
            $("#navMMaintenance").addClass("link-active");
            $('#dv-Maintenance').show(); break;

        //Monitoring
        case "#mServer":
            $("#navMServer").addClass("link-active");
            $('#dv-monitoring').show(); break;
        case "#mTel":
            $("#navMTel").addClass("link-active");
            $('#dv-monitoring').show(); break;

        //Reports
        case "#rJO":
            $("#navKPIJO").addClass("link-active");
            $('#dv-reports').show(); break;
        case "#rDailyReport":
            $("#navReport").addClass("link-active");
            $('#dv-reports').show(); break;


    }
});

function reloadPage() {
    location.reload();
}

function getDateFormat(format, customdate) {

    var dateformat = "";

    var date = new Date();
    var month = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];

    if (customdate == "today") {

        switch (format) {
            case 'YMD':
                dateformat = date.getFullYear() + "-" + month[date.getMonth()] + "-" + date.getDate() + " " +
                    date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                break;
            case 'MDY':
                dateformat = month[date.getMonth()] + "-" + date.getDate() + "-" + date.getFullYear() + " " +
                    date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                break;
            case 'DMY':
                dateformat = date.getDate() + "-" + month[date.getMonth()] + "-" + date.getFullYear() + " " +
                    date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                break;
            case 'Date':
                dateformat = date.getDate() + "-" + month[date.getMonth()] + "-" + date.getFullYear()
                break;
            case 'time':
                dateformat = " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
            case 'Y/M/D':
                dateformat = date.getFullYear() + "/" + month[date.getMonth()] + "/" + date.getDate() + " " +
                    date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                break;
        }
    } else if (customdate == "customwithtime") {
        var date = new Date(format); // override
        dateformat = date.getFullYear() + "-" + month[date.getMonth()] + "-" + date.getDate() + " " +
            date.getHours() + ":" + date.getMinutes() + ":00.000";
    }
    else if (customdate == "dateonly") { // yyyy-mm-dd
        var today = "";
        var date = new Date(format); // override

        if (date.getDate() <= 9)
            today = "0" + date.getDate();
        else
            today = date.getDate();

        dateformat = date.getFullYear() + "-" + month[date.getMonth()] + "-" + today;
    }
    else if (customdate == "nohrs") { // MM-dd-yyyy
        var date = new Date(format); // override
        dateformat = month[date.getMonth()] + "-" + date.getDate() + "-" + date.getFullYear();
    } else if (customdate == "lastmonth") {

        var date = new Date(format);
        dateformat = date.getFullYear() + "-" + month[parseInt(date.getMonth() - 1)] + "-" + date.getDate();

    } else
        dateformat = customdate + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

    return dateformat;
}

