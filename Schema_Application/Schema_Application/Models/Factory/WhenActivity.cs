using Schema.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Factory
{
    public class WhenActivity : IWhenActivity
    {
        public List<TimeSpan> GetTimeSpan(TimeSpan startTime, Activity activity)
        {
            throw new NotImplementedException();
        }
    }
}