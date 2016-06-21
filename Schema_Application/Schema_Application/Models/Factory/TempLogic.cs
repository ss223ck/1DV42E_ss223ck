using Schema.Domain.DataModels;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Factory
{
    public class TempLogic
    {
        private List<WeekDayViewModel> _weekDays;
        private List<ActivitySummeryViewModel> _activitySummeries;

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public List<WeekDayViewModel> WeekDays 
        {
            get { return _weekDays;}
            set { _weekDays = value;} 
        }
        public List<ActivitySummeryViewModel> ActivitySummeries 
        {
            get { return _activitySummeries ?? (_activitySummeries = new List<ActivitySummeryViewModel>()); }
            set { _activitySummeries = value; }
        }
        public TempLogic(List<WeekDayViewModel> weekDays)
        {
            if(weekDays.Count() != 7)
            {
                throw new Exception("weekDays in constructor for templogic got != 7 value");
            }
            WeekDays = weekDays;
        }
        
        public List<WeekDayViewModel> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {
            foreach (RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel in randomizeActivitySummeriesViewModel)
            {
                for (int i = 0; i < randomActivitySummeryViewModel.ActivityTimesCountsInWeek; i++)
                {
                    ActivitySummeries.Add(CreateActivity(randomActivitySummeryViewModel));
                }
            }
            for (int id = 0; id < WeekDays.Count(); id++)
            {
                WeekDays[id].ActivitiySummeries = ActivitySummeries.FindAll(x => (x.WeekDayId - 1) == id);
            }
            return WeekDays;
        }

        private ActivitySummeryViewModel CreateActivity(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {
            int activityStartTime;

            ActivitySummeryViewModel activitySummeryViewModel = new ActivitySummeryViewModel()
            {
                ActivityId = randomActivitySummeryViewModel.ActivityId,
                ActivitySummeryId = randomActivitySummeryViewModel.ActivityId,
                //Change userID
                Userid = 1,
                Name = randomActivitySummeryViewModel.ActivityName,
                Description = randomActivitySummeryViewModel.Description
            };
            CreateActivityWeekDayId(randomActivitySummeryViewModel, activitySummeryViewModel);
            CreateActivityTimeSpan(activitySummeryViewModel, randomActivitySummeryViewModel);
            
            return activitySummeryViewModel;
        }

        private void CreateActivityWeekDayId(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel, ActivitySummeryViewModel activitySummeryViewModel)
        {
            do
            {
                if (randomActivitySummeryViewModel.WeekDayId == null)
                {
                    activitySummeryViewModel.WeekDayId = RandomNumber(1, 8);
                }
                else
                {
                    foreach (int dayId in randomActivitySummeryViewModel.WeekDayId)
                    {
                        if (!ActivitySummeries.FindAll(x => x.ActivityId == activitySummeryViewModel.ActivityId).
                                            Exists(x => x.WeekDayId == dayId))
                        {
                            activitySummeryViewModel.WeekDayId = dayId;
                        }
                    }
                }
            } while (ActivitySummeries.FindAll(x => x.ActivityId == activitySummeryViewModel.ActivityId).
                                       Exists(x => x.WeekDayId == activitySummeryViewModel.WeekDayId));
        }

        private void CreateActivityTimeSpan(ActivitySummeryViewModel activitySummeryViewModel, RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {
            bool isActivityTimeSpanNotGood = false;
            do
            {
                int activityStartTime = RandomNumber(6, 22);
                activitySummeryViewModel.StartTime = new TimeSpan(activityStartTime, 0, 0);
                activitySummeryViewModel.EndTime = new TimeSpan(activityStartTime + randomActivitySummeryViewModel.ActivityTime, 0, 0);

                foreach (var activity in ActivitySummeries.FindAll(x => x.WeekDayId == activitySummeryViewModel.WeekDayId))
                {
                    //Check if the new activity starts after previous activity
                    if (activitySummeryViewModel.StartTime >= activity.EndTime)
                    {
                        isActivityTimeSpanNotGood = false;
                    }
                    // Check if the new activity starts earlier and do not overlap the other acitivity starttime
                    else if (activitySummeryViewModel.StartTime < activity.StartTime && activitySummeryViewModel.EndTime <= activity.StartTime)
                    {
                        isActivityTimeSpanNotGood = false;
                    }
                    else
                    {
                        isActivityTimeSpanNotGood = true;
                    }
                }
            } while (isActivityTimeSpanNotGood);
        }
    }
}