using CalendarMVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    public interface ICalendar
    {
        Person Login { get; set; }
        List<Day> AllDays { get; set; }
        List<Day> Days { get; set; }
        List<Day> LoadEvents(DateTime from, DateTime to, string user);
        void UpdateAppointment(Appointment e);
        void DeleteAppointment(Appointment e);
        void AddAppointment(Appointment e, Person user);
        Appointment GetAppointment(string id);
        List<Person> getPersonsFromAppointment(Appointment a);
        List<Person> getAllPersons();
        void logUser(string user);
    }
}
