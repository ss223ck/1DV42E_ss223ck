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
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return View("Index", _convertService.GetWeekDayViewModels(User.Identity.GetUserId()));
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        #endregion

        #region Details
        public ActionResult Detail(int? id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id.HasValue)
                    {
                        try
                        {
                            if (_convertService.CheckIfActivitySummeryIdBelongsToUser(User.Identity.GetUserId(), (int)id))
                            {
                                return View("Details", _convertService.GetActivityViewModel((int)id));
                            }
                            else
                            {
                                TempData["errorMessage"] = "You tried to access a activity that you don't own";
                                return RedirectToAction("index");
                            }
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        #endregion

        #region Edit

        public ActionResult Edit(int? id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id.HasValue)
                    {
                        try
                        {

                            if (_convertService.CheckIfActivitySummeryIdBelongsToUser(User.Identity.GetUserId(), (int)id))
                            {
                                return View("Edit", _convertService.GetActivityViewModel((int)id));
                            }
                            else
                            {
                                TempData["errorMessage"] = "You tried to access a activity that you don't own";
                                return RedirectToAction("index");
                            }
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
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivitySummeryId, ActivityId, WeekDayId, Name, Description, StartTime, EndTime")]ActivitySummeryViewModel activitySummeryViewModel)
        {
            
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (activitySummeryViewModel != null)
                    {
                        try
                        {

                            if (_convertService.CheckIfActivitySummeryIdBelongsToUser(User.Identity.GetUserId(), activitySummeryViewModel.ActivitySummeryId))
                            {
                                activitySummeryViewModel.UserId = User.Identity.GetUserId();
                                _convertService.UpdateActivitySummery(activitySummeryViewModel);
                                TempData["successMessage"] = "You changed the activity";
                                return RedirectToAction("index");
                            }
                            else
                            {
                                TempData["errorMessage"] = "You tried to change userId which is not allowed";
                                return RedirectToAction("index");
                            }
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
            else
            {
                return View("Edit");
            }

        }

        #endregion
        #region DeleteSchema
        public ActionResult Delete(int? id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id.HasValue)
                    {
                        try
                        {

                            if (_convertService.CheckIfActivitySummeryIdBelongsToUser(User.Identity.GetUserId(), (int)id))
                            {
                                return View("Delete", _convertService.GetActivityViewModel((int)id));
                            }
                            else
                            {
                                TempData["errorMessage"] = "You tried to access a activity that you don't own";
                                return RedirectToAction("index");
                            }
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        if (_convertService.CheckIfActivitySummeryIdBelongsToUser(User.Identity.GetUserId(), (int)id))
                        {
                            _schemaRepository.DeleteActivitySummery((int)id);
                            _schemaRepository.Save();
                            TempData["successMessage"] = "You deleted the activity";
                            return RedirectToAction("index");
                        }
                        else
                        {
                            TempData["errorMessage"] = "You tried to access a activity that you don't own";
                            return RedirectToAction("index");
                        }
                    }
                    catch (Exception)
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        #endregion
        #region CreateSchema
        public ActionResult CreateSchema()
        {
            try
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchema([Bind(Include = "ActivityId, WeekDayId, Name, Description, StartTime, EndTime")]ActivitySummeryViewModel activitySummeryViewModel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            activitySummeryViewModel.UserId = User.Identity.GetUserId();
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        #endregion

        #region RandomizeSchema
        public ActionResult RandomizeSchema()
        {
            try
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
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("RandomizeSchema")]
        [ValidateAntiForgeryToken]
        public ActionResult RandomizeSchema(int? id)
        {
            try
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
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        #region partialviews
        public ActionResult RandomizeActivitySummery(int? id, int counter)
        {
            try
            {
                if (id.HasValue)
                {
                    return PartialView("_RandomizeActivitySummery", _convertService.GetRandomizeActivitySummeryViewModel((int)id, counter));
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult GetActivitySummeries()
        {
            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    return PartialView("_ShowSchema", _convertService.GetWeekDayViewModels(User.Identity.GetUserId())); 
                }
                else
                {
                    throw new Exception("User is not logged in");
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get det activities";
                return View("Error");
            }
        }

        public ActionResult GetActivities()
        {
            try
            {
                return PartialView("_ActivityDropdown", _convertService.GetActivityViewModels());
            } catch(Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get det activities";
                return View("Error");
            }
        }
        public ActionResult GetWeekDaysCheckboxes(int? counterID)
        {
            
            try
            {
                if (counterID.HasValue)
                {
                    return PartialView("_WeekDaysCheckBoxes", _convertService.GetWeekDayViewModelsForPartial((int)counterID).AsEnumerable()); 
                }
                else
                {
                    return View("Error");
                }
            } catch(Exception)
            {
                TempData["ErrorMessage"] = "Something went wrong when trying to get the checkboxes";
                return View("Error");
            }
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
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateSchema([Bind(Include = "ActivityId, WeekDayId, ActivityTimesCountsInWeek, ActivityName, CountIndex, Description, ActivityTime")]List<RandomizeActivitySummeriesViewModel> randomizeActivitySummeriesViewModel)
        {
            try
            {
                if (ModelState.IsValid && randomizeActivitySummeriesViewModel != null)
                {
                    var returnvalues = _convertService.GenerateSchema(randomizeActivitySummeriesViewModel, User.Identity.GetUserId());
                    TempData["RandomizedSchema"] = returnvalues;

                    return PartialView("_ShowSchema", returnvalues); 
                }
                else
                {
                    return View("_ErrorModelStateIsNotValid");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        #endregion
    }
}