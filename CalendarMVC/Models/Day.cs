using CalendarMVC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Calendar.Model
{
    public class Day 
    {
        public DateTime Date { get; set; }
        public List<Appointment> AppointmentsList { get; set; }

        public Day(DateTime date)
        {
            this.Date = date;
            AppointmentsList = new List<Appointment>();
            AppointmentsList = new List<Appointment>(AppointmentsList.OrderBy(e => e.StartTime).ToList());            
        }      
        

        public String DateWeekNumber
        {
            get
            {
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                return "Week " + weekNum.ToString();
            }
        }

        public String DateString
        {
            get
            {
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                return "Week " + weekNum.ToString();
            }
        }


        public void AddEvent(Appointment e)
        {
            AppointmentsList.Add(e);
            AppointmentsList = new List<Appointment>(AppointmentsList.OrderBy(ev => ev.StartTime).ToList());
        }

        public void EditEvent(Appointment e)
        {
            foreach (Appointment ev in AppointmentsList)
                if (ev.AppointmentID.Equals(e.AppointmentID))
                    ev.copy(e);
            AppointmentsList = new List<Appointment>(AppointmentsList.OrderBy(ev => ev.StartTime).ToList());
        }

        public void DeleteEvent(Appointment e)
        {
            AppointmentsList.Remove(e);
            AppointmentsList = new List<Appointment>(AppointmentsList.OrderBy(ev => ev.StartTime).ToList());
        }
        
    }
}
