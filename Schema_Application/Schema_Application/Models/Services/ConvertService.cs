using Schema.Domain.DataModels;
using Schema.Domain.Repositories;
using Schema_Application.Models.Factory;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Services
{
    public class ConvertService : IConvertService
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        public List<WeekDayViewModel> GetWeekDayViewModels(string userID)
        {
            //change _schemaRepository.GetUserSpecificWeekDayActivities(1); to the userID
            //Picks out schedule for specific user
            IEnumerable<WeekDay> userWeekDays = _schemaRepository.GetUserSpecificWeekDayActivities(userID);
            List<WeekDayViewModel> viewModels = new List<WeekDayViewModel>(7);
            foreach (WeekDay model in userWeekDays)
            {
                viewModels.Add(new WeekDayViewModel
                {
                    Day = model.Day,
                    WeekDayId = model.WeekDayId,
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

        public List<WeekDayViewModel> GetWeekDayViewModelsForPartial(int? counterID)
        {
            IEnumerable<WeekDay> weekDays = _schemaRepository.GetAllWeekDays();
            List<WeekDayViewModel> weekDaysViewModels = new List<WeekDayViewModel>();

            foreach (var weekDay in weekDays)
            {
                weekDaysViewModels.Add(new WeekDayViewModel()
                {
                    WeekDayId = weekDay.WeekDayId,
                    Day = weekDay.Day,
                    Counter = counterID
                });
            }
            return weekDaysViewModels;
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
        public void CreateActivitySummery(ActivitySummeryViewModel activitySummeryViewModel, string userId)
        {
            try
            {
                ActivitySummery activitySummery = new ActivitySummery()
                {
                    ActivityId = activitySummeryViewModel.ActivityId,
                    WeekDayId = activitySummeryViewModel.WeekDayId,
                    UserId = userId,
                    StartTime = activitySummeryViewModel.StartTime,
                    EndTime = activitySummeryViewModel.EndTime,
                    ActivityDescription = activitySummeryViewModel.Description,
                    Activity = _schemaRepository.GetSpecificActivity(activitySummeryViewModel.ActivityId),
                    WeekDay = _schemaRepository.GetSpecificWeekDay(activitySummeryViewModel.WeekDayId)
                };
                _schemaRepository.CreateActivitySummery(activitySummery);
                _schemaRepository.Save();
                _schemaRepository.Dispose();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong when trying to save the activity summery");
            }
        }

        public void CreateNewGeneratedSchema(List<WeekDayViewModel> weekDays, string userId)
        {
            List<ActivitySummery> savedActivities = _schemaRepository.GetAllActivitySummeries(userId).ToList();
            foreach(ActivitySummery activitySummery in savedActivities)
            {
                _schemaRepository.DeleteActivitySummery(activitySummery.ActivitySummeryId);
            }

            foreach (WeekDayViewModel weekDay in weekDays)
            {
                foreach (ActivitySummeryViewModel activitySummeryViewModel in weekDay.ActivitiySummeries)
                {
                    try
                    {
                        ActivitySummery activitySummery = new ActivitySummery()
                        {
                            ActivityId = activitySummeryViewModel.ActivityId,
                            WeekDayId = activitySummeryViewModel.WeekDayId,
                            UserId = userId,
                            StartTime = activitySummeryViewModel.StartTime,
                            EndTime = activitySummeryViewModel.EndTime,
                            ActivityDescription = activitySummeryViewModel.Description,
                            Activity = _schemaRepository.GetSpecificActivity(activitySummeryViewModel.ActivityId),
                            WeekDay = _schemaRepository.GetSpecificWeekDay(activitySummeryViewModel.WeekDayId)
                        };
                        _schemaRepository.CreateActivitySummery(activitySummery);
                        _schemaRepository.Save();
                        _schemaRepository.Dispose();
                        _schemaRepository = new SchemaRepository();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Something went wrong when trying to save the activity summery");
                    }
                }
            }
        }

        #region Generate schema
        public List<WeekDayViewModel> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel, string userId)
        {
            //change null to right userId
            SchemaGenerator schemaGenerator = new SchemaGenerator(GetWeekDayViewModelsForPartial(null), GetWeekDayViewModels(userId));
            return schemaGenerator.GenerateSchema(randomizeActivitySummeriesViewModel, userId);
        }

        #endregion
        public RandomizeActivitySummeriesViewModel GetRandomizeActivitySummeryViewModel(int id, int counter)
        {
            try
            {
                var activity = _schemaRepository.GetSpecificActivity(id);
                RandomizeActivitySummeriesViewModel activitySummeryViewModel = new RandomizeActivitySummeriesViewModel()
                {
                    ActivityId = activity.ActivityId,
                    ActivityName = activity.ActivityName,
                    CountIndex = counter
                };

                return activitySummeryViewModel;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong when trying to get the activity summery");
            }
        }


        public List<ActivityViewModel> GetActivityViewModels()
        {
            try
            {
                IEnumerable<Activity> activities = _schemaRepository.GetAllActivities();
                List<ActivityViewModel> activitiesViewModels = new List<ActivityViewModel>();

                foreach (var activity in activities)
                {
                    activitiesViewModels.Add(new ActivityViewModel()
                    {
                        ActivityId = activity.ActivityId,
                        Name = activity.ActivityName
                    });
                }
                activitiesViewModels.TrimExcess();
                return activitiesViewModels;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong when trying to get activity view models");
            }
        }
    }
}