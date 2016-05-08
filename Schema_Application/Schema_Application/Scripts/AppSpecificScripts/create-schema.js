"use strict"

var application = {
    init: function () {
        application.gatherActivities();
        application.gatherWeekDays();
    },
    gatherActivities: function () {
        GetInformation.gatherActivities().done(function (response) {
            jQuery("#activity-holder").html(response);
        });
    },
    gatherWeekDays: function () {
        GetInformation.gatherWeekDays().done(function (response) {
            jQuery("#dayOfWeek-holder").html(response);
        });
        
    }
}
window.onload = application.init;