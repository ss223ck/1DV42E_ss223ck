using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schema_Application.Models
{
    [MetadataType(typeof(Activity_Metadata))]
    public partial class Activity
    {
       
        private class Activity_Metadata
        {
            public int ActivityID{ get; set; }
            public int WeekDayID { get; set; }
            public TimeSpan StartDate { get; set; }
            public TimeSpan EndDate { get; set; }
            public string ActivityDescription { get; set; }
            public string ActivityName { get; set; }
        }
    }
}