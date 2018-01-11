using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalendarMVC;
using CalendarMVC.Controllers;

namespace CalendarMVC.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            controller.stateManager = new TestSessionStateManager<string>();

            // Act
            ViewResult result = controller.Index("0") as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DayInWindow()
        {
            // Arrange
            HomeController controller = new HomeController();

            controller.stateManager = new TestSessionStateManager<string>();

            // Act
            ViewResult result = controller.Index("0") as ViewResult;

            Assert.IsTrue(controller.Days[0].Any(d => d.Date.Date == DateTime.Today));
            
        }

        [TestMethod]
        public void IndexWithId()
        {
            // Arrange
            HomeController controller = new HomeController();

            controller.stateManager = new TestSessionStateManager<string>();

            // Act
            ViewResult result = controller.Index("7") as ViewResult;

            Assert.IsFalse(controller.Days[0].Any(d => d.Date.Date == DateTime.Today));

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexWithIdInWindow()
        {
            // Arrange
            HomeController controller = new HomeController();

            controller.stateManager = new TestSessionStateManager<string>();

            // Act
            ViewResult result = controller.Index("-14") as ViewResult;

            Assert.IsTrue(controller.Days[2].Any(d => d.Date.Date == DateTime.Today));

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
