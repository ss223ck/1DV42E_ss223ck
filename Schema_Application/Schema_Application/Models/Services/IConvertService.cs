using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema_Application.Models.Services
{
    public interface IConvertService
    {
        List<WeekDayViewModel> GetWeekDayViewModels(int userID);
        List<WeekDayViewModel> GetWeekDayViewModelsForPartial(int? counterID);
        ActivitySummeryViewModel GetActivityViewModel(int id);

        RandomizeActivitySummeriesViewModel GetRandomizeActivitySummeryViewModel(int id, int counter);

        List<ActivityViewModel> GetActivityViewModels();

        void CreateActivitySummery(ActivitySummeryViewModel activitySummeryViewModel);

        void CreateNewGeneratedSchema(List<WeekDayViewModel> weekDays, int userId);
        List<WeekDayViewModel> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel);
    }
}
