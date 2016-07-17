using Schema.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Domain.Repositories
{
    public class SchemaRepository : ISchemaRepository
    {
        private SchemaApplicationEntities _schemaApplicationEntities = new SchemaApplicationEntities();
        private bool disposed = false;
        public IEnumerable<Activity> GetAllActivities()
        {
            return _schemaApplicationEntities.Activities.AsQueryable();
        }
        public IEnumerable<WeekDay> GetAllWeekDays()
        {
            return _schemaApplicationEntities.WeekDays.AsQueryable();
        }

        public IEnumerable<ActivitySummery> GetAllActivitySummeries(string userId)
        {
            return _schemaApplicationEntities.ActivitySummeries.Where(x => x.UserId == userId).AsQueryable();
        }
        public Activity GetSpecificActivity(int id)
        {
            return _schemaApplicationEntities.Activities.Find(id);
        }
        public ActivitySummery GetSpecificActivitySummery(int id)
        {
            return _schemaApplicationEntities.ActivitySummeries.Find(id);
        }

        public WeekDay GetSpecificWeekDay(int id)
        {
            return _schemaApplicationEntities.WeekDays.Find(id);
        }
        public IEnumerable<WeekDay> GetUserSpecificWeekDayActivities(string userId)
        {
            IEnumerable<WeekDay> weekDays = _schemaApplicationEntities.WeekDays.AsQueryable();

            foreach (WeekDay day in weekDays)
            {
                day.ActivitySummeries = day.ActivitySummeries.Where(a => a.UserId == userId).ToList();
            }
            
            return weekDays;
        }
        public void CreateActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void CreateActivitySummery(ActivitySummery activitySummery)
        {
            _schemaApplicationEntities.ActivitySummeries.Add(activitySummery);
        }

        public void UpdateActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void DeleteActivity(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteActivitySummery(int id)
        {
            ActivitySummery activitySummery = _schemaApplicationEntities.ActivitySummeries.Find(id);
            _schemaApplicationEntities.ActivitySummeries.Remove(activitySummery);
        }

        #region Save And Dipose
        public void Save()
        {
            _schemaApplicationEntities.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _schemaApplicationEntities.Dispose();
                }
            }
            this.disposed = true;
        }
        #endregion
    }
}
