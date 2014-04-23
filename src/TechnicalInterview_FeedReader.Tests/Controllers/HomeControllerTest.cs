using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechnicalInterview_FeedReader;
using TechnicalInterview_FeedReader.Controllers;

namespace TechnicalInterview_FeedReader.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index("", "") as ViewResult;

            // Assert
            Assert.AreEqual("Find out whatsUP", result.ViewBag.Message);
        }
    }
}
