using Schema_Application.Models.Repository;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schema_Application.Controllers
{
    public class SchemaController : Controller
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        public ActionResult Index()
        {
            var models = _schemaRepository.GetAllWeekDays();
            List<WeekDayViewModel> viewModels = new List<WeekDayViewModel>(100);
            foreach(var model in models)
            {
                viewModels.Add(new WeekDayViewModel{
                    Day = model.Day,
                    Activities = model.Activities.Select(item => new ActivityViewModel(){
                        ActivityID = item.ActivityID,
                        Name = item.ActivityName,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate,
                        Description = item.ActivityDescription
                    }).ToList()
                });
            }
            viewModels.TrimExcess();
            return View("Index", viewModels);
        }

        public ActionResult Detail(int? id)
        {
            if(id.HasValue)
            {
                var activity = _schemaRepository.GetSpecificActivity(id.GetValueOrDefault());
                if(activity != null)
                {
                    var activityView = new ActivityViewModel(){
                        ActivityID = activity.ActivityID,
                        Name = activity.ActivityName,
                        StartTime = activity.StartDate,
                        EndTime = activity.EndDate,
                        Description = activity.ActivityDescription
                    };
                    return View("Details", activityView);
                }
                else
                {
                    TempData["ErrorMessage"] = "The activity doesn't exist";
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to show a specific activity, try again!";
                return RedirectToAction("Index");
            }
            
        }
    }
}