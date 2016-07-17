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
        private List<List<ActivitySummeryViewModel>> _activitySummeries;

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

        public List<List<ActivitySummeryViewModel>> ActivitySummeries 
        {
            get { return _activitySummeries ?? (_activitySummeries = new List<List<ActivitySummeryViewModel>>()); }
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

        public TempLogic(List<WeekDayViewModel> weekDays, List<WeekDayViewModel> userWeekDay)
        {
            if (weekDays.Count() != 7)
            {
                throw new Exception("weekDays in constructor for templogic got != 7 value");
            }
            WeekDays = weekDays;
        }
        
        public List<WeekDayViewModel> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {

            for (int i = 0; i < randomizeActivitySummeriesViewModel.Count; i++)
            {
                ActivitySummeries.Add(new List<ActivitySummeryViewModel>());
                for (int j = 0; j < randomizeActivitySummeriesViewModel[i].ActivityTimesCountsInWeek; j++)
                {
                    ActivitySummeries.Last().Add(CreateActivity(randomizeActivitySummeriesViewModel[i]));
                }
            }
            for (int id = 0; id < WeekDays.Count(); id++)
            {
                foreach (List<ActivitySummeryViewModel> summery in ActivitySummeries)
                {
                    WeekDays[id].ActivitiySummeries = WeekDays[id].ActivitiySummeries.Concat(summery.FindAll(x => (x.WeekDayId - 1) == id)).ToList();
                }
            }
            
            return WeekDays;
        }

        private ActivitySummeryViewModel CreateActivity(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {

            ActivitySummeryViewModel activitySummeryViewModel = new ActivitySummeryViewModel()
            {
                ActivityId = randomActivitySummeryViewModel.ActivityId,
                ActivitySummeryId = randomActivitySummeryViewModel.ActivityId,
                //Change userID
                //Userid = 1,
                Name = randomActivitySummeryViewModel.ActivityName,
                Description = randomActivitySummeryViewModel.Description
            };
            activitySummeryViewModel.WeekDayId = CreateActivityWeekDayId(activitySummeryViewModel, randomActivitySummeryViewModel);
            CreateActivityTimeSpan(activitySummeryViewModel, randomActivitySummeryViewModel);


            return activitySummeryViewModel;
        }

        private int CreateActivityWeekDayId(ActivitySummeryViewModel activitySummeryViewModel, RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {
            int weekDayId;
            do
            {
                weekDayId = 0;
                if (randomActivitySummeryViewModel.WeekDayId != null)
                {
                    if (randomActivitySummeryViewModel.ActivityTimesCountsInWeek < randomActivitySummeryViewModel.WeekDayId.Count())
                    {
                        weekDayId = randomActivitySummeryViewModel.WeekDayId[RandomNumber(0, randomActivitySummeryViewModel.WeekDayId.Count())];
                    }
                    else
                    {
                        foreach (int id in randomActivitySummeryViewModel.WeekDayId)
                        {
                            if (!ActivitySummeries.Last().FindAll(x => x.ActivityId == activitySummeryViewModel.ActivityId).
                                           Exists(x => x.WeekDayId == id) && weekDayId == 0)
                            {
                                weekDayId = id;
                            }
                        }
                    }
                }
                if(weekDayId == 0)
                {
                    weekDayId = RandomNumber(1, 8);
                }
            } while (ActivitySummeries.Last().FindAll(x => x.ActivityId == activitySummeryViewModel.ActivityId).
                                       Exists(x => x.WeekDayId == weekDayId));
            return weekDayId;
        }

        private void CreateActivityTimeSpan(ActivitySummeryViewModel activitySummeryViewModel, RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {
            bool isActivityTimeSpanNotGood = false;
            do
            {
                int activityStartTime = RandomNumber(6, 22);
                activitySummeryViewModel.StartTime = new TimeSpan(activityStartTime, 0, 0);
                activitySummeryViewModel.EndTime = new TimeSpan(activityStartTime + randomActivitySummeryViewModel.ActivityTime, 0, 0);

                foreach (var activity in ActivitySummeries.Last().FindAll(x => x.WeekDayId == activitySummeryViewModel.WeekDayId))
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