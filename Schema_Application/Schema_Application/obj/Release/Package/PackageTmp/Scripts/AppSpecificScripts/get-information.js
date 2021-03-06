﻿"use strict"

var GetInformation = {
    gatherActivities: function () {
        return $.ajax({
            url: "/Schema/GetActivities/"
        });
    },
    gatherWeekDaysCheckboxes: function (counter) {
        return $.ajax({
            url: "/Schema/GetWeekDaysCheckboxes/?counterID=" + counter
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
    },
    gatherSchema: function () {
        return $.ajax({
            url: "/Schema/GetActivitySummeries/"
        });
    },
    saveSchema: function () {
        return $.ajax({
            url: "/Schema/SaveGeneratedSchema/"
        });
    }
}