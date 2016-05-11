using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class RandomizeActivitySummeriesViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int[] WeekDayId { get; set; }
        public int CountIndex { get; set; }
        public string Description { get; set; }
        public int ActivityTime { get; set; }

    }
}