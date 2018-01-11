using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calendar.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalendarMVC.Models;

namespace CalendarMVC.Tests.Controllers
{
    public class StorageTest : IStorage
    {
        public StorageContext db { get; set; }

        public List<Appointment> Appointments { get; set; }
        public List<Attendance> Attendances { get; set; }
        public List<Person> People { get; set; }

        public StorageTest(StorageContext d)
        {
            db = d;
            Appointments = new List<Appointment>();
            Attendances = new List<Attendance>();
            People = new List<Person>();
        }

        public List<Person> getPersons()
        {
           return People.ToList();            
        }

        public Person getPerson(string login)
        {
            return new Person { UserID = login };
        }

        public List<Person> getPersonsFromIdsList(List<Guid> personsId)
        {
            return People.Where(p => personsId.Any(id => p.PersonID.Equals(id))).ToList();            
        }
        

        public List<Appointment> getAppointments(Person login)
        {
                List<Attendance> attendences;
                if (Attendances != null && login != null)
                    attendences = Attendances.Where(at => at.PersonID.Equals(login.PersonID)).ToList();
                else attendences = new List<Attendance>();
                var ids = attendences.Select(a => a.AppointmentID).ToList();
                var query = Appointments.Where(r => ids.Any(a => a.Equals(r.AppointmentID))).ToList();
                return query;            
        }

        public Appointment getAppointmentById(string id)
        {
          return Appointments.First(a => a.AppointmentID == new Guid(id));            
        }



        public Guid createAppointment(string title, string description, DateTime date, TimeSpan startDate, TimeSpan endDate, Person user)
        {
                var Appointment = new Appointment
                {
                    AppointmentID = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    AppointmentDate = date,
                    StartTime = startDate,
                    EndTime = endDate,
                    timestamp = Encoding.ASCII.GetBytes("\0\0")
                };

                Appointments.Add(Appointment);

                Attendances.Add(new Attendance() { AttendanceID = Guid.NewGuid(), PersonID = user.PersonID, AppointmentID = Appointment.AppointmentID });

            return Appointment.AppointmentID;
            
        }

        public void updateAppointment(Appointment st)
        {
           
                var original = Appointments.Find(a => a.AppointmentID == st.AppointmentID);
                if (original != null && Encoding.ASCII.GetString(original.timestamp) == Encoding.ASCII.GetString(st.timestamp))
                {
                    original.Title = st.Title;
                    original.AppointmentDate = st.AppointmentDate;
                    original.StartTime = st.StartTime;
                    original.EndTime = st.EndTime;
                    original.Description = st.Description;                   
                }
                else
                {
                    throw new Exception("Your appointment version is not actualy");
                }            
        }

        public void deleteAppointment(Appointment st)
        {
            
                var original = Appointments.Find(a => a.AppointmentID == st.AppointmentID);
                if (original != null && Encoding.ASCII.GetString(original.timestamp) == Encoding.ASCII.GetString(st.timestamp))
                {
                    var at = Attendances.First(a => a.AppointmentID == st.AppointmentID);
                    Attendances.Remove(at);                    
                }
                else
                {
                    throw new Exception("Your appointment version is not actualy");
                }
            
        }
    
}
}
