using Schema_Application.Models;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schema.Domain.DataModels;
using Schema.Domain.Repositories;

namespace Schema_Application.Controllers
{
    public class SchemaController : Controller
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        
        public ActionResult Index()
        {
            //change _schemaRepository.GetUserSpecificWeekDayActivities(1); to the users id
            //Picks out schedule for specific user
            var models = _schemaRepository.GetUserSpecificWeekDayActivities(1);
            List<WeekDayViewModel> viewModels = new List<WeekDayViewModel>(7);
            foreach (var model in models)
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
            return View("Index", viewModels);
        }

        public ActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                var activity = _schemaRepository.GetSpecificActivitySummery(id.GetValueOrDefault());
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

        public ActionResult CreateSchema()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchema(ActivitySummeryViewModel activitySummeryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ActivitySummery activitySummery = new ActivitySummery()
                                {
                                    ActivityId = activitySummeryVM.ActivityId,
                                    WeekDayId = activitySummeryVM.WeekDayId,
                                    //ADD tempdata userId
                                    UserId = 1,
                                    StartTime = activitySummeryVM.StartTime,
                                    EndTime = activitySummeryVM.EndTime,
                                    ActivityDescription = activitySummeryVM.Description
                                };
                    _schemaRepository.CreateActivitySummery(activitySummery);

                    var models = _schemaRepository.GetAllWeekDays();
                    List<WeekDayViewModel> viewModels = new List<WeekDayViewModel>(7);
                    foreach (var model in models)
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
                    return PartialView("_ShowSchema", viewModels); 
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }
            //Change this to show error message
            return PartialView("_ShowSchema"); 
        }

        public ActionResult GetActivities()
        {
            IEnumerable<Activity> activities = _schemaRepository.GetAllActivities();
            List<ActivityViewModel> activitiesViewModels = new List<ActivityViewModel>(50);

            foreach(var activity in activities)
            {
                activitiesViewModels.Add(new ActivityViewModel(){
                    ActivityId = activity.ActivityId,
                    Name = activity.ActivityName
                });
            }
            activitiesViewModels.TrimExcess();
            return PartialView("_ActivityDropdown", activitiesViewModels.AsEnumerable());
        }
        public ActionResult GetWeekDays()
        {
            IEnumerable<WeekDay> weekDays = _schemaRepository.GetAllWeekDays();
            List<WeekDayViewModel> weekDaysViewModels = new List<WeekDayViewModel>(50);

            foreach (var weekDay in weekDays)
            {
                weekDaysViewModels.Add(new WeekDayViewModel()
                {
                    WeekDayId = weekDay.WeekDayId,
                    Day = weekDay.Day
                });
            }
            weekDaysViewModels.TrimExcess();
            return PartialView("_WeekDaysCheckBoxes", weekDaysViewModels.AsEnumerable());
        }
    }
}