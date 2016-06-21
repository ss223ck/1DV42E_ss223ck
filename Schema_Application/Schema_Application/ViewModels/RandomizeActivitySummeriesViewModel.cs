using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class RandomizeActivitySummeriesViewModel
    {
        public int ActivityId { get; set; }
        public int[] WeekDayId { get; set; }

        //How many times in the week the person wants to do the activity
        public int ActivityTimesCountsInWeek { get; set; }
        public string ActivityName { get; set; }

        //Reference for List to keep track of whick index different values belongs to. 
        //Reason behind this is i have got several model-classes in the view so i can't bind one modelclass to the view
        public int CountIndex { get; set; }

        [Required]
        public string Description { get; set; }
        public int ActivityTime { get; set; }

        [Display(Name="Do you want to work out before studies/work")]
        public bool WhenWorkout { get; set; }

    }
}