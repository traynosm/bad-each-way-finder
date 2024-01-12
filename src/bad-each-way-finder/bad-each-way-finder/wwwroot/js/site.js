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

    let refreshRate = 15;
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
                let parser = new DOMParser();
                let parsedHtml = parser.parseFromString(result, 'text/html');
                let live_propositions = parsedHtml.getElementById("live-propositions").innerHTML;
                let account_propositions = parsedHtml.getElementById("account-propositions").innerHTML;
                let raised_propositions = parsedHtml.getElementById("raised-propositions").innerHTML;
                let status_message = parsedHtml.getElementById("StatusMessage").value;
                let updated_time = parsedHtml.getElementById("updated-time").innerHTML;

                $("#live-propositions").html('');
                $("#account-propositions").html('');
                $("#raised-propositions").html('');
                $("#StatusMessage").val('');
                $("#updated-time").html('');


                $("#live-propositions").html(live_propositions);
                $("#account-propositions").html(account_propositions);
                $("#raised-propositions").html(raised_propositions);
                $("#StatusMessage").val(status_message);
                $("#updated-time").html(updated_time);

                $("#last-updated-time").fadeOut(80).fadeIn(80).fadeOut(80).fadeIn(80);

                let msg = $('#StatusMessage').val();
                if (msg.length > 0) {
                    swal.fire(msg);
                }
            }
        });
    }

    $(".proposition-submit").on('click', function (e) {
        e.preventDefault();
        var data = $(this).parents('form:first').serialize();
        propositionAdded(data);
    });

    function propositionAdded(data) {
        $.post({
            url: "?handler=AddAccountProposition",
            data: data,
            success: function (result) {
                $("#account-propositions").html('');
                $("#account-propositions").html(result);
            }
        });
    };

    $(".proposition-remove").on('click', function (e) {
        e.preventDefault();
        var data = $(this).parents('form:first').serialize();
        propositionRemoved(data);
    });

    function propositionRemoved(data) {
        $.post({
            url: "?handler=RemoveAccountProposition",
            data: data,
            success: function (result) {
                $("#account-propositions").html('');
                $("#account-propositions").html(result);
            }
        });
    };

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(
        function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
});
