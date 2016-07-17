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

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Net;
using System.Data;

namespace Schema_Application.Controllers
{
    public class SchemaController : Controller
    {
        ISchemaRepository _schemaRepository = new SchemaRepository();
        IConvertService _convertService = new ConvertService();

        #region Index
        public ActionResult Index()
        {
            //User.Identity.IsAuthenticated;
            if (User.Identity.IsAuthenticated)
            {
                return View("Index", _convertService.GetWeekDayViewModels(User.Identity.GetUserId())); 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #endregion

        #region Details
        public ActionResult Detail(int? id)
        {
            if (User.Identity.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #endregion

        #region Edit

        public ActionResult Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
        #endregion
        #region DeleteSchema
        public ActionResult Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id.HasValue)
                {
                    try
                    {
                        return View("Delete", _convertService.GetActivityViewModel((int)id));
                    }
                    catch (Exception)
                    {
                        TempData["ErrorMessage"] = "Something went wrong when getting the specific activity";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong when getting the specific activity, try again!";
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                try
                {
                    _schemaRepository.DeleteActivitySummery((int)id);
                    _schemaRepository.Save();
                    TempData["successMessage"] = "You deleted the activity";
                    return RedirectToAction("index");
                }
                catch (DataException e)
                {
                    TempData["errorMessage"] = "Something went wrong when the activity was sopposed to be deleted, try again!";
                    return RedirectToAction("index");
                } 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #endregion
        #region CreateSchema
        public ActionResult CreateSchema()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(); 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchema(ActivitySummeryViewModel activitySummeryViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        _convertService.CreateActivitySummery(activitySummeryViewModel, User.Identity.GetUserId());

                        return PartialView("_ShowSchema", _convertService.GetWeekDayViewModels(User.Identity.GetUserId()));
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
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #endregion

        #region RandomizeSchema
        public ActionResult RandomizeSchema()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("RandomizeSchema"); 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RandomizeSchema(List<WeekDayViewModel> WeekDaysViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                var weekDays = (List<WeekDayViewModel>)TempData["RandomizedSchema"];
                if (weekDays != null)
                {
                    _convertService.CreateNewGeneratedSchema(weekDays, User.Identity.GetUserId());
                }
                else
                {

                }
                TempData["SuccessMessage"] = "The schema is saved";
                return Redirect("Index"); 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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

        public ActionResult GetActivitySummeries(string id)
        {
            try
            {
                return PartialView("_ShowSchema", _convertService.GetWeekDayViewModels(id)); 
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateSchema(List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {
            var returnvalues = _convertService.GenerateSchema(randomizeActivitySummeriesViewModel, User.Identity.GetUserId());
            TempData["RandomizedSchema"] = returnvalues;

            return PartialView("_ShowSchema", returnvalues);
        }

        #endregion
    }
}