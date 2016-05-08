"use strict"

var GetInformation = {
    gatherActivities: function () {
        return $.ajax({
            url: "/Schema/GetActivities/"
        });
    },
    gatherWeekDays: function () {
        return $.ajax({
            url: "/Schema/GetWeekDays/"
        });
    }
}