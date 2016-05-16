using Schema.Domain.DataModels;
using Schema.Domain.Repositories;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.BLL
{
    public class ConvertationService : IConvertationService
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        public List<WeekDayViewModel> GetWeekDayViewModels(int userID)
        {
            //change _schemaRepository.GetUserSpecificWeekDayActivities(1); to the userID
            //Picks out schedule for specific user
            IEnumerable<WeekDay> userWeekDays = _schemaRepository.GetUserSpecificWeekDayActivities(1);
            List<WeekDayViewModel> viewModels = new List<WeekDayViewModel>(7);
            foreach (WeekDay model in userWeekDays)
            {
                viewModels.Add(new WeekDayViewModel
                {
                    Day = model.Day,
                    ActivitiySummeries = model.ActivitySummeries.Select(item => new ActivitySummeryViewModel()
                    {
                        ActivitySummeryId = item.ActivitySummeryId,
                        ActivityId = item.ActivityId,
                        WeekDayId = item.WeekDayId,
                        Name = item.Activity.ActivityName,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Description = item.ActivityDescription
                    }).ToList()
                });
            }
            return viewModels;
        }
        public ActivitySummeryViewModel GetActivityViewModel(int id)
        {
            var activity = _schemaRepository.GetSpecificActivitySummery(id);
            if (activity != null)
            {
                var activityView = new ActivitySummeryViewModel()
                {
                    ActivitySummeryId = activity.ActivitySummeryId,
                    ActivityId = activity.ActivityId,
                    WeekDayId = activity.WeekDayId,
                    Name = activity.Activity.ActivityName,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    Description = activity.ActivityDescription
                };
                return activityView;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        public void CreateActivitySummery(ActivitySummeryViewModel activitySummeryViewModel)
        {
            try
            {
                ActivitySummery activitySummery = new ActivitySummery()
                    {
                        ActivityId = activitySummeryViewModel.ActivityId,
                        WeekDayId = activitySummeryViewModel.WeekDayId,
                        //ADD tempdata userId
                        UserId = 1,
                        StartTime = activitySummeryViewModel.StartTime,
                        EndTime = activitySummeryViewModel.EndTime,
                        ActivityDescription = activitySummeryViewModel.Description,
                        Activity = _schemaRepository.GetSpecificActivity(activitySummeryViewModel.ActivityId),
                        WeekDay = _schemaRepository.GetSpecificWeekDay(activitySummeryViewModel.WeekDayId)
                    };
                _schemaRepository.CreateActivitySummery(activitySummery);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong when trying to save the activity summery");
            }
        }
    }
}