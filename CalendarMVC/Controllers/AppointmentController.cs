using Calendar.Model;
using CalendarMVC;
using CalendarMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class AppointmentController : Controller
    {
        public Appointment MyAppointment { get; set; }

        public ICalendar CalendarModel { get; set; }   
        
        protected IStateManager<string> stateManager = new SessionStateManager<string>();
        public void setStateManager(IStateManager<string> manager)
        {
            stateManager = manager;
        }

        public void Delete(Appointment a)
        {
            try
            {
                CalendarModel.DeleteAppointment(a);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Save(Appointment a)
        {
            try
            {
                if (a.AppointmentID == Guid.Empty)
                {
                    CalendarModel.AddAppointment(a, CalendarModel.Login);
                }
                else
                {
                    CalendarModel.UpdateAppointment(a);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Index(Appointment a)
        {            
            CalendarModel = new CalendarModel(new Storage());
            CalendarModel.logUser(stateManager.load("login"));
            try
            {
                if (Request.Form["save"] != null)
                {
                    if (ModelState.IsValid == false)
                    {
                        return View(a);
                    }
                    Save(a);
                }
                else if (Request.Form["delete"] != null)
                {
                    Delete(a);
                }
                return RedirectToAction("Index", "Home");
            } 
            catch (Exception ex)
            {
                stateManager.save("error", ex.Message);
                return RedirectToAction("Index", "Appointment", new { id = a.AppointmentID });
            }
        }


        // GET: Appointment
        public ActionResult Index(string id)
        {
            CalendarModel = new CalendarModel(new Storage());
            CalendarModel.logUser(stateManager.load("login"));

            if(stateManager.load("Error") != null)
            {
                ViewBag.Error = true;
                ViewBag.msg = stateManager.load("Error") + " - " + "page refreshed.";
                stateManager.save("Error", null);
            }


            if (id != null && id != "new")
            {
                MyAppointment = CalendarModel.GetAppointment(id);
            }
            else
            {
                MyAppointment = new Appointment {
                    AppointmentID = Guid.Empty,
                    Title = "",
                    Description = "",
                    AppointmentDate = DateTime.Now,
                    StartTime = TimeSpan.Parse("10:00"),
                    EndTime = TimeSpan.Parse("12:00"),
                    timestamp = new byte[1]};
            }
            return View(MyAppointment);
        }
    }
}