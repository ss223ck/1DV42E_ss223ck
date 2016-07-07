using Schema_Application.Models;
using Schema_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schema.Domain.DataModels;
using Schema.Domain.Repositories;
using Schema_Application.Models.Services;

namespace Schema_Application.Controllers
{
    public class SchemaController : Controller
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        IConvertService _convertService = new ConvertService();

        #region Index
        public ActionResult Index()
        {
            /*TempData["userID"] = 1;
            var userID = TempData["userID"];*/
            int userID = 1;
            return View("Index", _convertService.GetWeekDayViewModels(userID));
        }

        #endregion

        #region Details
        public ActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return View("Details", _convertService.GetActivityViewModel((int)id));
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

        #region Edit

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return View("Edit", _convertService.GetActivityViewModel((int)id));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when trying to edit the specific activity";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to edit a specific activity, try again!";
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
                    _convertService.CreateActivitySummery(activitySummeryViewModel);

                    return PartialView("_ShowSchema", _convertService.GetWeekDayViewModels(userID)); 
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
        public ActionResult RandomizeSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {
            var returnvalues = _convertService.GenerateSchema(randomizeActivitySummeriesViewModel);
            TempData["RandomizedSchema"] = returnvalues;
            
            return PartialView("_ShowSchema", returnvalues);
        }
        
        #endregion

        #region partialviews
        public ActionResult RandomizeActivitySummery(int? id, int counter)
        {
            if(id.HasValue)
            {
                try
                {
                    return PartialView("_RandomizeActivitySummery", _convertService.GetRandomizeActivitySummeryViewModel((int)id, counter));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when geting the activity summery";
                    return RedirectToAction("Index");
                }
            }
            return PartialView("_RandomizeActivitySummery", new RandomizeActivitySummeriesViewModel(){ActivityId = id.Value});
        }

        public ActionResult GetActivitySummeries(int? id)
        {
            try
            {
                return PartialView("_ShowSchema", _convertService.GetWeekDayViewModels((int)id)); 
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get det activities";
            }
            return null;
        }

        public ActionResult GetActivities()
        {
            try
            {
                return PartialView("_ActivityDropdown", _convertService.GetActivityViewModels());
            } catch(Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get det activities";
            }
            return null;
        }
        public ActionResult GetWeekDaysCheckboxes(int? counterID)
        {
            if(counterID.HasValue)
            {
                try
                {
                    return PartialView("_WeekDaysCheckBoxes", _convertService.GetWeekDayViewModelsForPartial((int)counterID).AsEnumerable());
                } catch(Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when trying to get the checkboxes";
                }
            }
            return null;
        }
        public ActionResult GetWeekDaysRadioButtons()
        {
            try
            {
                return PartialView("_WeekDaysRadioButtons", _convertService.GetWeekDayViewModelsForPartial(null).AsEnumerable());
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get the checkboxes";
            }
            return null;
        }

        public ActionResult SaveGeneratedSchema()
        {
            var weekDays = (List<WeekDayViewModel>)TempData["RandomizedSchema"];
            if (weekDays != null)
            {
                foreach (WeekDayViewModel weekDay in weekDays)
                {
                    foreach(ActivitySummeryViewModel activitySummery in weekDay.ActivitiySummeries)
                    {
                        
                    }
                }
            }
            return null;
        }

        #endregion
    }
}