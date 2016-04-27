using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class SchemaCombinedViewModels
    {
        public IEnumerable<ActivityViewModel> Activities { get; set; }
        public IEnumerable<ActivitySummeryViewModel> ActivitySummeries { get; set; }
        public IEnumerable<WeekDayViewModel> WeekDays { get; set; }
    }
}