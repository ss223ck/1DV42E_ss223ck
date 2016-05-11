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
        var htmlHolder = document.getElementById("activity-summeries-holder").innerHTML;
        var activityId = dropdown.options[dropdown.selectedIndex].value;



        GetInformation.gatherRandomizeActivityView(activityId).done(function (response) {
            jQuery("#activity-summeries-holder").html(htmlHolder + response);
        }).error(function (error) {
            console.log(error);
        });
    }
}
window.onload = application.init;