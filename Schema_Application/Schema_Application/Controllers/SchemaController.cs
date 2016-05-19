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

        #region Edit

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return View("Edit", _convertationRepository.GetActivityViewModel((int)id));
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
        public ActionResult RandomizeSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {
            var returnvalues = _convertationRepository.GenerateSchema(randomizeActivitySummeriesViewModel);
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
                    return PartialView("_RandomizeActivitySummery", _convertationRepository.GetRandomizeActivitySummeryViewModel((int)id, counter));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Something went wrong when geting the activity summery";
                    return RedirectToAction("Index");
                }
            }
            return PartialView("_RandomizeActivitySummery");
        }
        public ActionResult GetActivities()
        {
            try
            {
                return PartialView("_ActivityDropdown", _convertationRepository.GetActivityViewModels());
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
                    return PartialView("_WeekDaysCheckBoxes", _convertationRepository.GetWeekDayViewModelsForPartial((int)counterID).AsEnumerable());
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
                return PartialView("_WeekDaysRadioButtons", _convertationRepository.GetWeekDayViewModelsForPartial(null).AsEnumerable());
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get the checkboxes";
            }
            return null;
        }

        #endregion
    }
}