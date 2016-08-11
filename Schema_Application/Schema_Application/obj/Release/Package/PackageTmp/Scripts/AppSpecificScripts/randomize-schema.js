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
            jQuery("#error-message").removeClass("display-none");
        });
    },
    renderActivitySummery: function () {
        var dropdown = document.getElementById("activityDropdown");
        var htmlActivityElementsHolder = document.getElementById("activity-summeries-holder");
        var activityId = dropdown.options[dropdown.selectedIndex].value;
        var activitySummeriesIndexCounter = htmlActivityElementsHolder.childElementCount;

        GetInformation.gatherRandomizeActivityView(activityId, activitySummeriesIndexCounter).done(function (response) {
            

            //Need checkboxes for the activity to se which days that person is/not willing to do that activity
            GetInformation.gatherWeekDaysCheckboxes(activitySummeriesIndexCounter).success(function (responseCheckboxes) {
                var responseActivityHolder = document.createElement("div");
                responseActivityHolder.innerHTML = response;
                htmlActivityElementsHolder.appendChild(responseActivityHolder);
                var checkboxHolder = document.getElementById("[" + activitySummeriesIndexCounter + "]dayOfWeek-holder");
                checkboxHolder.innerHTML = checkboxHolder.innerHTML + responseCheckboxes;
                document.getElementById("exitbar-holder-" + activitySummeriesIndexCounter).addEventListener("click", application.removeRandomizeActivitySummery);
            }).error(function (error) {
                jQuery("#error-message").html("Something went wrong when trying to get the checkboxes");
                jQuery("#error-message").removeClass("display-none");
            });
        }).error( function (error) {
            jQuery("#error-message").html("Something went wrong when trying to randomize the schema");
            jQuery("#error-message").removeClass("display-none");
        });
    },
    gatherSchema: function () {
        GetInformation.gatherSchema().done(function (response) {
            jQuery("#schema-container").html(response);
        }).error( function (error) {
            jQuery("#error-message").html("Something went wrong when trying to get the schema");
            jQuery("#error-message").removeClass("display-none");
        });
    },
    removeRandomizeActivitySummery: function (e) {
        var myDiv = e.currentTarget.parentNode.parentNode.parentNode.parentNode;
        myDiv.parentNode.removeChild(myDiv);
        var activityHolder = document.getElementById("activity-summeries-holder");

        //Changing the id so the array sent to server has the right index
        for(var i = 0; i < activityHolder.childElementCount; i++){
            var formNode = activityHolder.childNodes[i + 1].childNodes[0].childNodes[1];

            formNode.childNodes[1].childNodes[1].id = "exitbar-holder-" + i;

            formNode.childNodes[5].id = "[" + i + "]activityID-holder";

            formNode.childNodes[5].childNodes[1].name = "[" + i + "]ActivityId";

            formNode.childNodes[7].id = "[" + i + "]timesAWeek-holder.form-group.randomize-group";

            formNode.childNodes[7].childNodes[3].name = "[" + i + "].ActivityTimesCountsInWeek";

            formNode.childNodes[9].id = "[" + i + "]dayOfWeek-holder.form-group.randomize-group";

            formNode.childNodes[9].childNodes[7].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[12].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[17].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[22].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[27].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[32].name = "[" + i + "].WeekDayId";

            formNode.childNodes[9].childNodes[37].name = "[" + i + "].WeekDayId";

            formNode.childNodes[11].childNodes[3].name = "[" + i + "].ActivityTime";

            formNode.childNodes[13].childNodes[3].id = "[" + i + "].Description";

            formNode.childNodes[13].childNodes[3].name = "[" + i + "].Description";
        }
    }
}
window.onload = application.init;