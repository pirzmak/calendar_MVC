using Calendar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarMVC.Controllers
{
    public class LoginController : Controller
    {
        protected IStateManager<string> stateManager = new SessionStateManager<string>();
        protected ICalendar CalendarModel { get; set; }

        public void setStateManager(IStateManager<string> manager)
        {
            stateManager = manager;
        }

        public ActionResult Login(FormCollection collection)
        {
            var login = Request.Form["Login"];
            CalendarModel = new CalendarModel(new Storage());
            stateManager.save("login", Request.Form["Login"]);
            return RedirectToAction("Index", "Home");
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
    }
}