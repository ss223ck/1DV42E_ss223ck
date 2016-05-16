using Schema.Domain.DataModels;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema_Application.Models.Factory
{
    public interface ISchemaFactory
    {
        List<ActivitySummery> GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeryViewModelList);
    }
}
