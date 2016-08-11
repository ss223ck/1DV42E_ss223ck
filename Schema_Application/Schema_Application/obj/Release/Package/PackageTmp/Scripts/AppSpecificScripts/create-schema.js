"use strict"

var CreateSchema = {
    init: function () {
        CreateSchema.gatherActivities();
        CreateSchema.gatherWeekDays();
        CreateSchema.gatherSchema();
    },
    gatherActivities: function () {
        GetInformation.gatherActivities().done(function (response) {
            jQuery("#activity-holder").html(response);
        }).error(function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the activities");
            jQuery("#error-message").removeClass("display-none");
        });
    },
    gatherWeekDays: function () {
        GetInformation.gatherWeekDaysRadiobuttons().done(function (response) {
            jQuery("#dayOfWeek-holder").html(response);
        }).error(function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the radiobuttons");
            jQuery("#error-message").removeClass("display-none");
        });
    },
    gatherSchema: function () {
        GetInformation.gatherSchema().done(function (response) {
            jQuery("#schemaActivities").html(response);
        }).error(function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the schema");
            jQuery("#error-message").removeClass("display-none");
        });
    }
}
window.onload = CreateSchema.init;