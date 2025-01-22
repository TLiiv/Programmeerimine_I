using KooliProjekt.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_should_return_correct_view()
        {
            // Arrange
            var controller = new HomeController(); //System Under Test SUT

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
     
            //Assert.Equal("Index", result.ViewName);//Error, gives null, ASP special, if view method is used and view is returned. ASP net itself finds the name but after
                                                   //excecuteResultAsync method - in a nutshell timing is off. So i should make a test case with null instead of index. With that another problem arises
                                                   // if someone is used to return the method with name, the test case would fail so the test should be check if isNull or name is "index" 
            Assert.True(result.ViewName == "Index" ||
                 string.IsNullOrEmpty(result.ViewName));
        }
        [Fact]
        public void Privacy_should_return_correct_view()
        {
            // Arrange
            var controller = new HomeController(); 

            // Act
            var result = controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Privacy" ||
                 string.IsNullOrEmpty(result.ViewName));
        }     
        [Fact]
        public void Error_should_return_correct_view_with_Httpcontext()
        {
            // Arrange
            var controller = new HomeController(); 
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act

            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Error" ||
                 string.IsNullOrEmpty(result.ViewName));
        }     
        [Fact]
        public void Error_should_return_correct_view_with_Current_Activity()
        {
            // Arrange
            var controller = new HomeController(); 
            Activity.Current = new Activity("acitvity").Start();

            // Act

            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Error" ||
                 string.IsNullOrEmpty(result.ViewName));
        }
    }
}