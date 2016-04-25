using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class ActivityViewModel
    {
        public int ActivityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}