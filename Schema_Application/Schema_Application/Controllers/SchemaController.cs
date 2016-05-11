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

        #region Index
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

        #endregion

        #region Details
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

        #endregion

        #region CreateSchema
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
                                    ActivityDescription = activitySummeryVM.Description,
                                    Activity = _schemaRepository.GetSpecificActivity(activitySummeryVM.ActivityId),
                                    WeekDay = _schemaRepository.GetSpecificWeekDay(activitySummeryVM.WeekDayId)
                                };
                    _schemaRepository.CreateActivitySummery(activitySummery);

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

        #endregion

        #region RandomizeSchema
        public ActionResult RandomizeSchema()
        {
            var weekDays = _schemaRepository.GetAllWeekDays();
            RandomizeSchemaViewModel randomizeSchema = new RandomizeSchemaViewModel();


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


            randomizeSchema.WeekDays = weekDaysViewModels.AsEnumerable();
            return View("RandomizeSchema", randomizeSchema);
        }
        public ActionResult RandomizeSchema2()
        {
            List<RandomizeActivitySummeriesViewModel> activi = new List<RandomizeActivitySummeriesViewModel> (7);
            activi.Add(new RandomizeActivitySummeriesViewModel
            {
                ActivityId = 2,
                ActivityName = "träning",
                ActivityTime = 2,
                Description = "gymma",
                WeekDayId = new int[1]
            });
            activi.Add(new RandomizeActivitySummeriesViewModel
            {
                ActivityId = 3,
                ActivityName = "Plugg",
                ActivityTime = 2,
                Description = "c#",
                WeekDayId = new int[1]
            });
            activi.TrimExcess();
            return View("RandomizeSchema2", activi);
        }

        [HttpPost]
        public ActionResult RandomizeSchema2(List<RandomizeActivitySummeriesViewModel> list)
        {
            var i = 0;
            i = i - 1;
            return View();
        }

        #endregion

        #region partialviews
        public ActionResult RandomizeActivitySummery(int? id, int counter)
        {
            if(id.HasValue)
            {
                try
                {
                    var activity = _schemaRepository.GetSpecificActivity((int)id);
                    List<RandomizeActivitySummeriesViewModel> activitySummeryViewModel = new List<RandomizeActivitySummeriesViewModel>(1);
                    activitySummeryViewModel.Add(new RandomizeActivitySummeriesViewModel()
                    {
                        ActivityId = activity.ActivityId,
                        ActivityName = activity.ActivityName,
                        CountIndex = counter
                    });
                    

                    return PartialView("_RandomizeActivitySummery", activitySummeryViewModel);
                }
                catch (Exception)
                {

                }
            }

            return PartialView("_RandomizeActivitySummery");
        }

        public ActionResult GetActivities()
        {
            IEnumerable<Activity> activities = _schemaRepository.GetAllActivities();
            List<ActivityViewModel> activitiesViewModels = new List<ActivityViewModel>(50);

            foreach (var activity in activities)
            {
                activitiesViewModels.Add(new ActivityViewModel()
                {
                    ActivityId = activity.ActivityId,
                    Name = activity.ActivityName
                });
            }
            activitiesViewModels.TrimExcess();
            return PartialView("_ActivityDropdown", activitiesViewModels.AsEnumerable());
        }
        public ActionResult GetWeekDaysCheckboxes()
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

        
        public ActionResult WeekDaysRadioButtons()
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
            return PartialView("_WeekDaysRadioButtons", weekDaysViewModels.AsEnumerable());
        }

        #endregion
    }
}