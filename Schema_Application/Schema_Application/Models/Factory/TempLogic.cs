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
        public List<WeekDayViewModel> WeekDays 
        {
            get { return _weekDays;}
            set { _weekDays = value;} 
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
            List<ActivitySummeryViewModel> activitySummeries = new List<ActivitySummeryViewModel>();

            //Do something for every activity
            foreach (RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel in randomizeActivitySummeriesViewModel)
            {
                //create one activity
                /*if (randomActivitySummeryViewModel.ActivityTimesCountsInWeek == 0 || 
                    randomActivitySummeryViewModel.ActivityTimesCountsInWeek == 1)
                {
                    //create one activity
                    activitySummeries.Add(this.CreateActivity(randomActivitySummeryViewModel));
                }
                else
                {
                    
                }*/
                for (int i = 0; i < randomActivitySummeryViewModel.ActivityTimesCountsInWeek; i++)
                {
                    //create many activities
                    activitySummeries.Add(this.CreateActivity(randomActivitySummeryViewModel));
                }
            }
            //Returned values
            for (int id = 0; id < WeekDays.Count(); id++)
            {
                this.MapWeekDays(WeekDays[id], activitySummeries.FindAll(x => x.WeekDayId == id));
            }
            return WeekDays;
        }

        private ActivitySummeryViewModel CreateActivity(RandomizeActivitySummeriesViewModel randomActivitySummeryViewModel)
        {
            Random random = new Random();
            ActivitySummeryViewModel activitySummeryViewModel = new ActivitySummeryViewModel()
            {
                ActivityId = randomActivitySummeryViewModel.ActivityId,
                ActivitySummeryId = randomActivitySummeryViewModel.ActivityId,
                //Change userID
                Userid = 1,
                Name = randomActivitySummeryViewModel.ActivityName,
                Description = randomActivitySummeryViewModel.Description
            };
            int activityStartTime = random.Next(0, 22);
            int randomWeekDay = random.Next(1, 7);
            activitySummeryViewModel.StartTime = new TimeSpan(activityStartTime, 0, 0);
            activitySummeryViewModel.EndTime = new TimeSpan(activityStartTime + randomActivitySummeryViewModel.ActivityTime, 0, 0);
            activitySummeryViewModel.WeekDayId = randomWeekDay;
            return activitySummeryViewModel;
        }

        private void MapWeekDays(WeekDayViewModel weekday, List<ActivitySummeryViewModel> activitySummeries)
        {
            weekday.ActivitiySummeries = activitySummeries;
        }
    }
}