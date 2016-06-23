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
        });
    },
    gatherWeekDays: function () {
        GetInformation.gatherWeekDaysRadiobuttons().done(function (response) {
            jQuery("#dayOfWeek-holder").html(response);
        });
    },
    gatherSchema: function () {
        //Change the 1 to id
        GetInformation.gatherSchema(1).done(function (response) {
            jQuery("#schemaActivities").html(response);
        });
    }
}
window.onload = CreateSchema.init;