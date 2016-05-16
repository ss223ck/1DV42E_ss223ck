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
        var activitySummeriesIndexCounter = htmlActivityElementsHolder.childElementCount;

        GetInformation.gatherRandomizeActivityView(activityId, activitySummeriesIndexCounter).done(function (response) {
            jQuery("#activity-summeries-holder").html(htmlActivityElementsHolder.innerHTML + response);
            var checkboxHolder = document.getElementById("[" + activitySummeriesIndexCounter + "]dayOfWeek-holder");

            //Need checkboxes for the activity to se which days that person is/not willing to do that activity
            GetInformation.gatherWeekDaysCheckboxes(activitySummeriesIndexCounter).done(function (responseCheckboxes) {
                checkboxHolder.innerHTML = checkboxHolder.innerHTML + responseCheckboxes;
            }).error(function (error) {
                var i = 0;
            });
        }).error(function (error) {
            console.log(error);
        });
    }
}
window.onload = application.init;