$(document).ready(function () {
    $(".select-race").on('click', function (e) {
        let meetingName = $(this).siblings()[0].innerText + ' ~ ';
        let raceTime = $(this).text();
        let raceMeetingTime = meetingName.concat(raceTime);
        getSelectedRace(raceMeetingTime);
    });

    function getSelectedRace(raceMeetingTime) {
        $.get({
            url: '?handler=SelectedRace&raceMeetingTime=' + raceMeetingTime,
            beforeSend: function () {
                $("#selected-race").html('');
                document.getElementsByClassName("loader")[0].style.display = "block";

            },
            success: function (result) {
                $("#selected-race").html(result);
                document.getElementsByClassName("loader")[0].style.display = "none";
            }
        });
    }

    let refreshRate = 5;
    let myBoolean = false;
    let intervalId;

    $("#refresh-auto-racing").on('click', function (e) {
        myBoolean = !myBoolean;

        if (myBoolean) {
            $("#refresh-auto-racing").removeClass('btn-danger');
            $("#refresh-auto-racing").addClass('btn-success');
            $("#refresh-auto-racing").text('Auto Refresh On');

            refreshPropositions();
            intervalId = setInterval(refreshPropositions, refreshRate * 1000);
        }
        else {
            $("#refresh-auto-racing").removeClass('btn-success');
            $("#refresh-auto-racing").addClass('btn-danger');
            $("#refresh-auto-racing").text('Auto Refresh Off');

            clearInterval(intervalId);
        }
    });


    function refreshPropositions() {
        $.get({
            url: '?handler=Propositions',
            beforeSend: function () {

            },
            success: function (result) {
                $("#propositions").html('');
                $("#propositions").html(result);
                $("#last-updated-time").fadeOut(80).fadeIn(80).fadeOut(80).fadeIn(80);
            }
        });
    }
});
