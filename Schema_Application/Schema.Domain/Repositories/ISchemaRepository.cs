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
        IEnumerable<Activity> GetAllActivities();
        Activity GetSpecificActivity(int id);
        ActivitySummery GetSpecificActivitySummery(int id);
        void CreateActivity(Activity activity);
        void UpdateActivity(Activity activity);
        void DeleteActivity(int id);
        IEnumerable<WeekDay> GetAllWeekDays();

        void Save();
    }
}
