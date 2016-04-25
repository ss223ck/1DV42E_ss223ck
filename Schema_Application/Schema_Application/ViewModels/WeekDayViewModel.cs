using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class WeekDayViewModel
    {
        private List<ActivityViewModel> _activities;
        public string Day{ get; set; }
        
        public List<ActivityViewModel> Activities
        {
            get { return _activities.OrderBy(a => a.StartTime).AsEnumerable().ToList(); }
            set { _activities = value; }
        }
    }
}