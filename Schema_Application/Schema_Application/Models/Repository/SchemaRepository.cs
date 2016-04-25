using Schema_Application.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Repository
{
    public class SchemaRepository : ISchemaRepository
    {
        private SchemaApplicationEntities _schemaApplicationEntities = new SchemaApplicationEntities();
        private bool disposed = false;
        public IEnumerable<Activity> GetAllActivities()
        {
            return _schemaApplicationEntities.Activities.AsQueryable();
        }

        public Activity GetSpecificActivity(int id)
        {
            return _schemaApplicationEntities.Activities.Find(id);
        }

        public void CreateActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void UpdateActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void DeleteActivity(int id)
        {
            throw new NotImplementedException();
        }

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


        public IEnumerable<WeekDay> GetAllWeekDays()
        {
            return _schemaApplicationEntities.WeekDays.AsQueryable();
        }
    }
}