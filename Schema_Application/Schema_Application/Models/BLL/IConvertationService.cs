using Schema.Domain.DataModels;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema_Application.Models.BLL
{
    public interface IConvertationService
    {
        List<WeekDayViewModel> GetWeekDayViewModels(int userID);

        ActivitySummeryViewModel GetActivityViewModel(int id);

        void CreateActivitySummery(ActivitySummeryViewModel activitySummeryViewModel);
    }
}
