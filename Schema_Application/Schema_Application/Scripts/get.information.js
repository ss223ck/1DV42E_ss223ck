"use strict"

var application = {
    init: function () {
        application.gatherActivities();
        application.gatherWeekDays();
    },
    gatherActivities: function(){
        $.ajax({
            url: "/Schema/GetActivities/"
        }).done(function (response) {
            jQuery("#activity-holder").html(response);  
        }).error(function (error) {

        });
    },
    gatherWeekDays: function(){
        $.ajax({
            url: "/Schema/GetWeekDays/"
        }).done(function (response) {
            jQuery("#dayOfWeek-holder").html(response);  
        }).error(function (error) {

        });
    }
}
window.onload = application.init;