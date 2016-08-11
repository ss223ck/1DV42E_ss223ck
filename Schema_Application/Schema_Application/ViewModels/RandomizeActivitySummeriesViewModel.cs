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

        [Required]
        public int[] WeekDayId { get; set; }

        //How many times in the week the person wants to do the activity
        [Required]
        [Range(1, 7, ErrorMessage = "You can do a activity of max 7 and minimum 1 time a week")]
        [Display(Name = "Choose how many days you want to do this activity")]
        public int ActivityTimesCountsInWeek { get; set; }
        public string ActivityName { get; set; }

        //Reference for List to keep track of whick index different values belongs to. 
        //Reason behind this is i have got several model-classes in the view so i can't bind one modelclass to the view
        public int CountIndex { get; set; }

        [Required]
        [Display(Name = "Describe your activity with max of 50 chars")]
        [StringLength(50, ErrorMessage = "The description can max be 50 chars")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "For how long do you want to do this activity")]
        [Range(1, 24, ErrorMessage = "You can do a activity of max 7 and minimum 1 time a week")]
        public int ActivityTime { get; set; }

    }
}