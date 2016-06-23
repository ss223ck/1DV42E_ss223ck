using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.ViewModels
{
    public class WeekDayViewModel
    {
        private List<ActivitySummeryViewModel> _activitySummeries;
        public int WeekDayId { get; set; }
        public string Day{ get; set; }
        public int? Counter { get; set; }
        public List<ActivitySummeryViewModel> ActivitiySummeries
        {
            get 
            {
                if(_activitySummeries == null)
                {
                    _activitySummeries = new List<ActivitySummeryViewModel>();
                }
                return _activitySummeries.OrderBy(a => a.StartTime).AsEnumerable().ToList(); 
            }
            set { _activitySummeries = value; }
        }
    }
}