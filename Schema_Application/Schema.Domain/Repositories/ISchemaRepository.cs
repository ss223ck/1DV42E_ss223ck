using Schema.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Domain.Repositories
{
    public interface ISchemaRepository : IDisposable
    {
        IEnumerable<ActivitySummery> GetAllActivitySummeries(string userId);
        IEnumerable<Activity> GetAllActivities();
        Activity GetSpecificActivity(int id);
        ActivitySummery GetSpecificActivitySummery(int id);
        WeekDay GetSpecificWeekDay(int id);
        IEnumerable<WeekDay> GetUserSpecificWeekDayActivities(string userId);
        void CreateActivity(Activity activity);
        void CreateActivitySummery(ActivitySummery activitySummery);
        void UpdateActivitySummery(ActivitySummery activitySummery);
        void DeleteActivity(int id);
        void DeleteActivitySummery(int id);
        IEnumerable<WeekDay> GetAllWeekDays();
        void Save();
    }
}
