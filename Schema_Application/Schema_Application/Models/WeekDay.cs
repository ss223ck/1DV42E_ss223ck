using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Schema_Application.Models
{
    [MetadataType(typeof(WeekDay_Metadata))]
    public partial class WeekDay
    {

        private class WeekDay_Metadata
        {
            public int WeekDayID { get; set; }
            public string Day { get; set; }
        }
    }
}