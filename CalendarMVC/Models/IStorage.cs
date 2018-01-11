using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarMVC.Models
{
    public interface IStorage
    {
        List<Person> getPersons();
        List<Person> getPersonsFromIdsList(List<Guid> personsId);
        Person getPerson(string login);
        List<Appointment> getAppointments(Person login);
        Appointment getAppointmentById(string id);
        Guid createAppointment(string title, string description, DateTime date, TimeSpan startDate, TimeSpan endDate, Person user);
        void updateAppointment(Appointment st);
        void deleteAppointment(Appointment st);
    }
}
