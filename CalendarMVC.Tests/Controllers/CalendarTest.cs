using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calendar.Model;
using WebApplication1.Controllers;
using Rhino.Mocks;
using System.Globalization;
using System.Data.Entity;

namespace CalendarMVC.Tests.Controllers
{
    [TestClass]
    public class CalendarTest
    {
        private AppointmentController vm;
        [TestInitialize()]
        public void Initialize()
        {
            vm = new AppointmentController();
            var calendarMock = MockRepository.GenerateMock<StorageContext>();
            calendarMock.Appointments = MockRepository.GenerateMock<DbSet<Appointment>>();
            calendarMock.People = MockRepository.GenerateMock<DbSet<Person>>();
            calendarMock.Attendances = MockRepository.GenerateMock<DbSet<Attendance>>();
            vm.CalendarModel = new CalendarModel(new StorageTest(calendarMock));
        }
        [TestMethod()]
        public void SaveTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = new byte[1]
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            
            Assert.AreEqual(1, ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person()).Count);
        }

        [TestMethod()]
        public void SaveTwoTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = new byte[1]
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            vm.CalendarModel.AddAppointment(eventMock, new Person());

            Assert.AreEqual(2, ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person()).Count);
        }
        [TestMethod()]
        public void DeleteTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = Encoding.ASCII.GetBytes("\0\0")
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            var a = ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person())[0];
            vm.CalendarModel.DeleteAppointment(a);

            Assert.AreEqual(0, ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person()).Count);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = Encoding.ASCII.GetBytes("\0\0")
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            var a = ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person())[0];
            a.Title = "tutle";
            vm.CalendarModel.UpdateAppointment(a);

            Assert.AreEqual("tutle", ((CalendarModel)vm.CalendarModel).storage.getAppointments(new Person())[0].Title);
        }


        [TestMethod()]
        [ExpectedException(typeof(Exception),
    "Your appointment version is not actualy")]
        public void DeleteWrongTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = Encoding.ASCII.GetBytes("\0\0")
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            eventMock.timestamp = Encoding.ASCII.GetBytes("a");
            vm.CalendarModel.DeleteAppointment(eventMock);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception),
    "Your appointment version is not actualy")]
        public void UpdateWrongTest()
        {
            List<Appointment> events = new List<Appointment>();
            Appointment eventMock = new Appointment
            {
                AppointmentID = Guid.Empty,
                Title = "New",
                Description = "asdads",
                AppointmentDate = DateTime.Now,
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("12:00"),
                timestamp = Encoding.ASCII.GetBytes("\0\0")
            };

            vm.CalendarModel.AddAppointment(eventMock, new Person());
            eventMock.timestamp = Encoding.ASCII.GetBytes("a");
            vm.CalendarModel.UpdateAppointment(eventMock);
        }

    }
}
