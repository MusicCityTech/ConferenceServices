using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConferenceWebAPI;
using ConferenceWebAPI.Controllers;

namespace ConferenceWebAPI.Tests.Controllers
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
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
