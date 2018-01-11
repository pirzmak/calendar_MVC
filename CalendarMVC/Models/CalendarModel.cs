using CalendarMVC;
using CalendarMVC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Calendar.Model
{
    public class CalendarModel : ICalendar
    {
        public IStorage storage { get; set; }
        public List<Day> AllDays { get; set; }
        public List<Day> Days { get; set; }
        public Person Login { get; set; }
        
        public CalendarModel(IStorage sotrage)
        {
            storage = sotrage;
        }

        public void logUser(String user)
        {
            try
            {
                Login = storage.getPerson(user);
            }
            catch (Exception)
            {
                Login = new Person();
            }
        }

        private List<Day> getDays(List<Appointment> appointments)
        {
            List<Day> days = new List<Day>();

            foreach (Appointment e in appointments)
            {
                if (days.Exists(d => e.AppointmentDate.Date.Equals(d.Date)))
                    days.Find(d => e.AppointmentDate.Date.Equals(d.Date)).AppointmentsList.Add(e);
                else
                {
                    Day newDay = new Day(e.AppointmentDate);
                    newDay.AddEvent(e);
                    days.Add(newDay);
                }
            }

            return days;
        }

        public List<Day> LoadEvents(DateTime from, DateTime to, string user)
        {
            logUser(user);
            AllDays = getDays(storage.getAppointments(Login));
            AllDays.ForEach(d => d.AppointmentsList = d.AppointmentsList.OrderBy(e => e.StartTime).ToList());
            List<Day> filteredList = AllDays.Where(d => d.Date.Date.CompareTo(from.Date) >= 0 && d.Date.Date.CompareTo(to.Date) <= 0).ToList();
            Days = filteredList;


            return Days;
        }        

        public Appointment GetAppointment(string id)
        {
            return storage.getAppointmentById(id);
        }
        
        public void DeleteAppointment(Appointment e)
        {
            try
            {
                storage.deleteAppointment(e);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAppointment(Appointment e) 
        {
            try
            {
                storage.updateAppointment(e);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddAppointment(Appointment e, Person user)
        {
            try
            {
                storage.createAppointment(e.Title, e.Description, e.AppointmentDate, e.StartTime, e.EndTime, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Person> getAllPersons()
        {
            return storage.getPersons();
        }

        public List<Person> getPersonsFromAppointment(Appointment a)
        {
            return storage.getPersonsFromIdsList(a.Attendances.Select(at => at.PersonID).ToList());
        }

    }
}