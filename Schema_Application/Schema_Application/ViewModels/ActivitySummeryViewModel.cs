using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class ActivitySummeryViewModel
    {
        public int ActivitySummeryId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [Required]
        [Range(1, 7)]
        public int WeekDayId { get; set; }

        public string UserId { get; set; }

        [StringLength(50, ErrorMessage = "The description can max be 50 chars")]
        public string Name { get; set; }

        [Required(ErrorMessage="You have to write a description")]
        [StringLength(50, ErrorMessage = "The description can max be 50 chars")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You have assign a start time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "You have assign a end time")]
        public TimeSpan EndTime { get; set; }
    }
}