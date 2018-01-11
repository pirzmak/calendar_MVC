using Calendar.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarMVC.Controllers
{
    public class HomeController : Controller
    {       

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IStateManager<string> stateManager = new SessionStateManager<string>();

        public void setStateManager(IStateManager<string> manager)
        {
            stateManager = manager;
        }

        const int NUMDAYSINWEEK = 7;

        public DateTime ActualDay { get; set; }

        public ICalendar CalendarModel { get; set; }

        public Day[][] Days { get; set; }
      

        private void loadData()
        {
            if (stateManager.load("login") != null)
            {
                CalendarModel.LoadEvents(ActualDay, ActualDay.AddDays(4 * NUMDAYSINWEEK), stateManager.load("login"));                
            } else
            {
                CalendarModel.Days = new List<Day>(); 
            }
            Days = LoadEvents();
        }

        private Day[][] LoadEvents()
        {
            Day[][] newDL = new Day[4][];

            DateTime day = ActualDay;

            for (int x = 0; x < newDL.Length; x += 1)
            {
                newDL[x] = new Day[NUMDAYSINWEEK];
                for (int y = 0; y < NUMDAYSINWEEK; y += 1)
                {
                    if (CalendarModel.Days.Any(d => d.Date.Date.Equals(day.Date.Date)))
                        newDL[x][y] = CalendarModel.Days.Single(d => d.Date.Date.Equals(day.Date.Date));
                    else
                        newDL[x][y] = new Day(day);

                    day = day.AddDays(1);
                }
            }

            return newDL;
        }

        private DateTime GetFirstDayOfWeek(DateTime date)
        {
            return date.AddDays(-((int)date.DayOfWeek - 1));
        }

        public ActionResult Index(string id)
        {
            var tmpDate = DateTime.Now;

            ActualDay = GetFirstDayOfWeek(DateTime.Now.AddDays(Int32.Parse(id)));

            CalendarModel = new CalendarModel(new Storage());

            try
            {
                CalendarModel.logUser(stateManager.load("login"));
                log.Info("Calendar Started");
            } catch (Exception) {
               
            }

            loadData();

            ViewBag.Days = Days;
            ViewBag.Date = Int32.Parse(id);

            return View();
        }        

    }
}