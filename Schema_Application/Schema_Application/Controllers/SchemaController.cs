using Schema_Application.Models;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schema.Domain.DataModels;
using Schema.Domain.Repositories;
using Schema_Application.Models.BLL;

namespace Schema_Application.Controllers
{
    public class SchemaController : Controller
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        IConvertationService _convertationRepository = new ConvertationService();

        #region Index
        public ActionResult Index()
        {
            /*TempData["userID"] = 1;
            var userID = TempData["userID"];*/
            int userID = 1;
            return View("Index", _convertationRepository.GetWeekDayViewModels(userID));
        }

        #endregion

        #region Details
        public ActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return View("Details", _convertationRepository.GetActivityViewModel((int)id));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when trying to show the specific activity";
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
        public ActionResult CreateSchema(ActivitySummeryViewModel activitySummeryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*var userID = TempData["userID"];*/
                    int userID = 1;
                    _convertationRepository.CreateActivitySummery(activitySummeryViewModel);

                    return PartialView("_ShowSchema", _convertationRepository.GetWeekDayViewModels(userID)); 
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when trying to create a schema";
                    return RedirectToAction("Index");
                }   
            }
            TempData["ErrorMessage"] = "Something is wrong with the information sent to the server. Please try again!";
            return PartialView("_CreateSchema"); 
        }

        #endregion

        #region RandomizeSchema
        public ActionResult RandomizeSchema()
        {
            return View("RandomizeSchema");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RandomizeSchema(List<RandomizeActivitySummeriesViewModel> list)
        {
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
                    RandomizeActivitySummeriesViewModel activitySummeryViewModel = new RandomizeActivitySummeriesViewModel()
                    {
                        ActivityId = activity.ActivityId,
                        ActivityName = activity.ActivityName,
                        CountIndex = counter
                    };                    

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
        public ActionResult GetWeekDaysCheckboxes(int? counterID)
        {
            IEnumerable<WeekDay> weekDays = _schemaRepository.GetAllWeekDays();
            List<WeekDayViewModel> weekDaysViewModels = new List<WeekDayViewModel>();

            foreach (var weekDay in weekDays)
            {
                weekDaysViewModels.Add(new WeekDayViewModel()
                {
                    WeekDayId = weekDay.WeekDayId,
                    Day = weekDay.Day,
                    Counter = counterID
                });
            }
            return PartialView("_WeekDaysCheckBoxes", weekDaysViewModels.AsEnumerable());
        }
        public ActionResult GetWeekDaysRadioButtons()
        {
            IEnumerable<WeekDay> weekDays = _schemaRepository.GetAllWeekDays();
            List<WeekDayViewModel> weekDaysViewModels = new List<WeekDayViewModel>();

            foreach (var weekDay in weekDays)
            {
                weekDaysViewModels.Add(new WeekDayViewModel()
                {
                    WeekDayId = weekDay.WeekDayId,
                    Day = weekDay.Day
                });
            }
            return PartialView("_WeekDaysRadioButtons", weekDaysViewModels.AsEnumerable());
        }

        #endregion
    }
}