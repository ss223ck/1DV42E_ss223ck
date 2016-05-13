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
            url: "/Schema/GetWeekDaysRadioButtons/"
        });
    },
    gatherRandomizeActivityView: function (id, counter) {
        return $.ajax({
            url: "/Schema/RandomizeActivitySummery/" + id + "?counter=" + counter
        });
    }
}