"use strict"

var application = {
    init: function () {
        application.renderActivityDropdown();
        document.getElementById('generateNewActivity').addEventListener('click', application.renderActivitySummery);
    },
    renderActivityDropdown: function () {
        GetInformation.gatherActivities().done(function (response) {
            jQuery("#activity-holder").html(response);
        });
    },
    renderActivitySummery: function () {
        var dropdown = document.getElementById("activityDropdown");
        var htmlActivityElementsHolder = document.getElementById("activity-summeries-holder");
        var activityId = dropdown.options[dropdown.selectedIndex].value;

        //Get the acitivitysummery that will generate activitysummeries in the week
        GetInformation.gatherRandomizeActivityView(activityId, htmlActivityElementsHolder.childElementCount).done(function (response) {
            jQuery("#activity-summeries-holder").html(htmlActivityElementsHolder.innerHTML + response);

            //Need checkboxes for the activity to se which days that person is/not willing to do that activity
            GetInformation.gatherWeekDaysCheckboxes().done(function (response) {
                console.log();
            }).error(function (error) {
                console.log();
            });
        }).error(function (error) {
            console.log(error);
        });
    }
}
window.onload = application.init;