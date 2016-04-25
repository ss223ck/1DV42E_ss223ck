using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema_Application.Models.Repository
{
    interface ISchemaRepository : IDisposable
    {
        IEnumerable<Activity> GetAllActivities();
        Activity GetSpecificActivity(int id);
        void CreateActivity(Activity activity);
        void UpdateActivity(Activity activity);
        void DeleteActivity(int id);
        IEnumerable<WeekDay> GetAllWeekDays();
        
        void Save();
    }
}
