"use strict"

var GetInformation = {
    gatherActivities: function () {
        return $.ajax({
            url: "/Schema/GetActivities/"
        });
    },
    gatherWeekDaysCheckboxes: function () {
        return $.ajax({
            url: "/Schema/GetWeekDaysCheckboxes/"
        });
    },
    gatherWeekDaysRadiobuttons: function () {
        return $.ajax({
            url: "/Schema/WeekDaysRadiobuttons/"
        });
    },
    gatherRandomizeActivityView: function (id) {
        return $.ajax({
            url: "/Schema/RandomizeActivitySummery/" + id
        });
    }
}