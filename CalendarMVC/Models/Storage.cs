using CalendarMVC;
using CalendarMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    public class Storage: IStorage
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Person login;


        public List<Person> getPersons()
        {
            using (var db = new StorageContext())
            {
                return db.People.ToList();
            }
        }

        public List<Person> getPersonsFromIdsList(List<Guid> personsId)
        {
            try
            {
                using (var db = new StorageContext())
                    return db.People.Where(p => personsId.Any(id => p.PersonID.Equals(id))).ToList();
                
            }
            catch (Exception e)
            {
                log.Error("Error when get event persons");
                throw e;
            }
        }

        public Person getPerson(string login)
        {            
            if(login == null)
            {
                log.Error("Empty user");
                throw new Exception("Empty user");
            }
            try
            {
                using (var db = new StorageContext())
                {
                    if (db.People.Where(p => p.UserID.Equals(login)).ToList().Count == 0)
                    {
                        var newPerson = new Person {PersonID = Guid.NewGuid(), FirstName = "Adam", LastName = "Test", UserID = login };
                        this.login = db.People.Add(newPerson);
                        db.SaveChanges();
                        log.Info("Add new user: " + this.login.UserID + " with id: " + this.login.PersonID);
                    } else
                    {
                        this.login = db.People.Where(p => p.UserID.Equals(login)).First();
                    }

                    log.Info("User Loged: " + this.login.UserID);
                }
                return this.login;
            }
            catch (ArgumentNullException ANE)
            {
                log.Error("User " + login + "can't be load:" + ANE);
                throw ANE;
            }
        }

        public List<Appointment> getAppointments(Person login)
        {
            using (var db = new StorageContext())
            {
                List<Attendance> attendences;
                if (db.Attendances != null && login != null)
                    attendences = db.Attendances.Where(at => at.PersonID.Equals(login.PersonID)).ToList(); else attendences = new List<Attendance>();
                var ids = attendences.Select(a => a.AppointmentID).ToList();
                var query = db.Appointments.Where(r => ids.Any(a => a.Equals(r.AppointmentID))).ToList();
                return query;
            }           
        }

        public Appointment getAppointmentById(string id)
        {
           using (var db = new StorageContext())
            {
                return db.Appointments.First(a => a.AppointmentID == new Guid(id));
            }
        }
        


        public Guid createAppointment(string title, string description, DateTime date, TimeSpan startDate, TimeSpan endDate, Person user)
        {
            using (var db = new StorageContext())
            {
                var Appointment = new Appointment
                {
                    AppointmentID = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    AppointmentDate = date,
                    StartTime = startDate,
                    EndTime = endDate
                };                

                var id = db.Appointments.Add(Appointment);

                db.Attendances.Add(new Attendance() { AttendanceID = Guid.NewGuid(), PersonID = user.PersonID, AppointmentID = id.AppointmentID });
                try
                {
                    db.SaveChanges();
                    log.Info("Create new appointment: " + id.AppointmentID);
                    return Appointment.AppointmentID;
                }
                catch (Exception E)
                {
                    log.Error("Message did not create becouse of: " + E);
                    throw E;
                }
            }
        }

        public void updateAppointment(Appointment st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Appointments.Find(st.AppointmentID);
                if (original != null && Encoding.ASCII.GetString(original.timestamp) == Encoding.ASCII.GetString(st.timestamp))
                {
                    original.Title = st.Title;
                    original.AppointmentDate = st.AppointmentDate;
                    original.StartTime = st.StartTime;
                    original.EndTime = st.EndTime;
                    original.Description = st.Description;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception E)
                    {
                        log.Error("Message did not create becouse of: " + E);
                        throw E;
                    }
                }
                else
                {
                    throw new Exception("Your appointment version is not actualy");
                }
            }            
        }

        public void deleteAppointment(Appointment st)
        {
            using (var db = new StorageContext())
            { 
                var original = db.Appointments.Find(st.AppointmentID);
                if (original != null && Encoding.ASCII.GetString(original.timestamp) == Encoding.ASCII.GetString(st.timestamp))
                {
                    var at = db.Attendances.First(a => a.AppointmentID == st.AppointmentID);
                    db.Attendances.Remove(at);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception E)
                    {
                        log.Error("Message did not create becouse of: " + E);
                        throw E;
                    }
                }
                else
                {
                    throw new Exception("Your appointment version is not actualy");
                }
            }
        }
    }
}
