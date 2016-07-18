"use strict"

var application = {
    init: function () {
        application.renderActivityDropdown();
        application.gatherSchema();
        document.getElementById('generateNewActivity').addEventListener('click', application.renderActivitySummery);
    },
    renderActivityDropdown: function () {
        GetInformation.gatherActivities().done(function (response) {
            jQuery("#activity-holder").html(response);
        }).error(function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the activities");
        });
    },
    renderActivitySummery: function () {
        var dropdown = document.getElementById("activityDropdown");
        var htmlActivityElementsHolder = document.getElementById("activity-summeries-holder");
        var activityId = dropdown.options[dropdown.selectedIndex].value;
        var activitySummeriesIndexCounter = htmlActivityElementsHolder.childElementCount;

        GetInformation.gatherRandomizeActivityView(activityId, activitySummeriesIndexCounter).done(function (response) {
            

            //Need checkboxes for the activity to se which days that person is/not willing to do that activity
            GetInformation.gatherWeekDaysCheckboxes(activitySummeriesIndexCounter).done(function (responseCheckboxes) {
                var responseActivityHolder = document.createElement("div");
                responseActivityHolder.innerHTML = response;
                htmlActivityElementsHolder.appendChild(responseActivityHolder);
                var checkboxHolder = document.getElementById("[" + activitySummeriesIndexCounter + "]dayOfWeek-holder");
                checkboxHolder.innerHTML = checkboxHolder.innerHTML + responseCheckboxes;
            }, function (error) {
                jQuery("#error-message").html("Something went wrong when trying to get the checkboxes");
            });
        }, function (error) {
            jQuery("#error-message").html("Something went wrong when trying to randomize the schema");
        });
    },
    gatherSchema: function () {
        GetInformation.gatherSchema().done(function (response) {
            jQuery("#schema-container").html(response);
        }).error(function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the schema");
        });
    }
}
window.onload = application.init;