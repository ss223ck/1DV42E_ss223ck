using Schema.Domain.DataModels;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Factory
{
    public class SchemaGenerator
    {
        private List<WeekDayViewModel> _weekDays;
        private List<List<ActivitySummeryViewModel>> _activitySummeries;
        private List<WeekDayViewModel> _userWeekDay;

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
        public SchemaGenerator(List<WeekDayViewModel> weekDays)
        {
            if(weekDays.Count() != 7)
            {
                throw new Exception("weekDays in constructor for templogic got != 7 value");
            }
            WeekDays = weekDays;
        }

        public SchemaGenerator(List<WeekDayViewModel> weekDays, List<WeekDayViewModel> userWeekDay)
        {
            if (weekDays.Count() != 7)
            {
                throw new Exception("weekDays in constructor for templogic got != 7 value");
            }
            _userWeekDay = userWeekDay;
            WeekDays = weekDays;
        }
        
        public List<WeekDayViewModel> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel, string userId)
        {
            for (int i = 0; i < randomizeActivitySummeriesViewModel.Count; i++)
            {
                ActivitySummeries.Add(new List<ActivitySummeryViewModel>());
                for (int j = 0; j < randomizeActivitySummeriesViewModel[i].ActivityTimesCountsInWeek; j++)
                {
                    if (randomizeActivitySummeriesViewModel[i].ActivityId == 2)
                    {
                        CreateActivitiesForStudy(randomizeActivitySummeriesViewModel[i], userId);
                    }
                    else
                    {
                        ActivitySummeries.Last().Add(CreateActivity(randomizeActivitySummeriesViewModel[i], userId));
                    }
                    
                }
            }
            for (int id = 0; id < WeekDays.Count(); id++)
            {
                foreach (List<ActivitySummeryViewModel> summery in ActivitySummeries)
                {
                    _userWeekDay[id].ActivitiySummeries = _userWeekDay[id].ActivitiySummeries.Concat(summery.FindAll(x => (x.WeekDayId - 1) == id)).ToList();
                }
            }

            return _userWeekDay;
        }

        private ActivitySummeryViewModel CreateActivity(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel, string userId)
        {
            ActivitySummeryViewModel activitySummeryViewModel = InitiateActivitySummeryStandardProperties(randomActivitySummeryViewModel, userId);

            activitySummeryViewModel.WeekDayId = CreateActivityWeekDayId(activitySummeryViewModel, randomActivitySummeryViewModel);
            CreateActivityTimeSpan(activitySummeryViewModel, randomActivitySummeryViewModel);


            return activitySummeryViewModel;
        }

        private ActivitySummeryViewModel InitiateActivitySummeryStandardProperties(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel, string userId)
        {
            ActivitySummeryViewModel activitySummeryViewModel = new ActivitySummeryViewModel()
            {
                ActivityId = randomActivitySummeryViewModel.ActivityId,
                ActivitySummeryId = 0,
                UserId = userId,
                Name = randomActivitySummeryViewModel.ActivityName,
                Description = randomActivitySummeryViewModel.Description
            };
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

                var allActivitySummeries = ActivitySummeries.Concat(_userWeekDay.Select(x => x.ActivitiySummeries));

                foreach (var activityList in allActivitySummeries)
                {
                    foreach (var activity in activityList.FindAll(x => x.WeekDayId == activitySummeryViewModel.WeekDayId))
                    {
                        //Check if the new activity starts after previous activity

                        if (activitySummeryViewModel.StartTime < activitySummeryViewModel.EndTime )
                        {
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
                        else
                        {
                            isActivityTimeSpanNotGood = true;
                        }
                    }
                }
            } while (isActivityTimeSpanNotGood);
        }

        private void CreateActivitiesForStudy(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel, string userId)
        {
            ActivitySummeryViewModel activitySummeryViewModel = InitiateActivitySummeryStandardProperties(randomActivitySummeryViewModel, userId);
            activitySummeryViewModel.WeekDayId = CreateActivityWeekDayId(activitySummeryViewModel, randomActivitySummeryViewModel);

            List<ActivitySummeryViewModel> newlist = new List<ActivitySummeryViewModel>(100) { new ActivitySummeryViewModel()};
            List<ActivitySummeryViewModel> specificDayList = new List<ActivitySummeryViewModel>(100);

            ActivitySummeries.ForEach(delegate(List<ActivitySummeryViewModel> activityList)
            {
                newlist.AddRange(activityList);
            });

            _userWeekDay.ForEach(delegate(WeekDayViewModel weekDay)
            {
                newlist.AddRange(weekDay.ActivitiySummeries);
            });

            specificDayList = newlist.Where(x => x.WeekDayId == activitySummeryViewModel.WeekDayId).OrderBy(y => y.StartTime).ToList();         

            if (specificDayList.Count == 0 || specificDayList[0].StartTime >= new TimeSpan(9, 20, 0))
            {
                activitySummeryViewModel.StartTime = new TimeSpan(8, 0, 0);
                activitySummeryViewModel.EndTime = activitySummeryViewModel.StartTime + new TimeSpan(1, 0, 0);
            }
            else
            {
                bool isActivityNotAdded = false;
                for(int i = 0; i < specificDayList.Count(); i++)
                {
                    if(isActivityNotAdded && specificDayList.Count() == 1 || i == (specificDayList.Count() - 1))
                    {
                        activitySummeryViewModel.StartTime = specificDayList[i].EndTime;
                        activitySummeryViewModel.EndTime = activitySummeryViewModel.StartTime + new TimeSpan(1, 0, 0);
                        isActivityNotAdded = true;
                    }
                    else if (isActivityNotAdded && (specificDayList[i + 1].StartTime - specificDayList[i].EndTime) >= new TimeSpan(1, 20, 0))
                    {
                        activitySummeryViewModel.StartTime = specificDayList[i].EndTime;
                        activitySummeryViewModel.EndTime = activitySummeryViewModel.StartTime + new TimeSpan(1, 0, 0);
                        isActivityNotAdded = true;
                    }
                }
            }
            ActivitySummeryViewModel firstBreak = CreateBreak(activitySummeryViewModel.EndTime, activitySummeryViewModel.WeekDayId, userId);

            _activitySummeries.Last().Add(activitySummeryViewModel);
            _activitySummeries.Last().Add(firstBreak);

            specificDayList.Add(activitySummeryViewModel);
            specificDayList.Add(firstBreak);
            specificDayList = specificDayList.OrderBy(y => y.StartTime).ToList();

            for (int i = 1; i < randomActivitySummeryViewModel.ActivityTime; i++)
            {
                ActivitySummeryViewModel activitySummeryViewModelNew = InitiateActivitySummeryStandardProperties(randomActivitySummeryViewModel, userId);
                bool isActivityNotAdded = true;

                activitySummeryViewModelNew.WeekDayId = activitySummeryViewModel.WeekDayId;
                
                for (int j = 0; j < specificDayList.Count(); j++)
                {
                    if (isActivityNotAdded && j == specificDayList.Count() - 1 || isActivityNotAdded && (specificDayList[j + 1].StartTime - specificDayList[j].EndTime) >= new TimeSpan(1, 20, 0))
                    {
                        activitySummeryViewModelNew.StartTime = specificDayList[j].EndTime;
                        activitySummeryViewModelNew.EndTime = activitySummeryViewModelNew.StartTime + new TimeSpan(1, 0, 0);
                        _activitySummeries.Last().Add(activitySummeryViewModelNew);

                        ActivitySummeryViewModel secondBreak = CreateBreak(activitySummeryViewModelNew.EndTime, activitySummeryViewModel.WeekDayId, userId);
                        _activitySummeries.Last().Add(secondBreak);

                        specificDayList.Add(activitySummeryViewModelNew);
                        specificDayList.Add(secondBreak);
                        specificDayList = specificDayList.OrderBy(y => y.StartTime).ToList();
                        isActivityNotAdded = false;
                    }
                }
            }
        }

        private ActivitySummeryViewModel CreateBreak(TimeSpan breakStartTime, int weekDayId, string userId)
        {
            ActivitySummeryViewModel breakInActivitySummery = new ActivitySummeryViewModel()
            {
                ActivityId = 9,
                ActivitySummeryId = 0,
                UserId = userId,
                Name = "Break",
                Description = "Time to take a break",
                StartTime = breakStartTime,
                EndTime = breakStartTime + new TimeSpan(0, 20, 0),
                WeekDayId = weekDayId
            };
            return breakInActivitySummery;
        }

    }
}