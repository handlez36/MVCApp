using BudgetApp.WebUI.HtmlHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using BudgetApp.WebUI.Models;
using System.Web;
using Moq;
using System.Web.Routing;
using System.Reflection;

namespace BudgetApp.Tests
{
    
    
    /// <summary>
    ///This is a test class for HtmlLinkHelperTest and is intended
    ///to contain all HtmlLinkHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MvcApplicationTest
    {

        [TestMethod()]
        public void Test_Routes()
        {
            TestRouteSuccess("~/Home/List", "Home", "List", "GET");
            TestRouteSuccess("~/Home/List2", "Home", "List2", "GET");

            //TestRouteFail("~/One");           There is a rule in the Global.asax file that allows this
        }

        public HttpContextBase GetHttpContextBase(string requestURL, string HttpMethod = "GET")
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns(requestURL);
            mockRequest.Setup(r => r.HttpMethod).Returns(HttpMethod);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s=>s);

            Mock<HttpContextBase> contextBase = new Mock<HttpContextBase>();
            contextBase.Setup(c => c.Request).Returns(mockRequest.Object);
            contextBase.Setup(c => c.Response).Returns(mockResponse.Object);

            return contextBase.Object;
        }


        public void TestRouteSuccess(string URL, string controller, string action, string HttpMethod, object parameterSet = null)
        {
            // Arrange
            RouteCollection routeCollection = new RouteCollection();
            MvcApplication.RegisterRoutes(routeCollection);

            // Act
            RouteData routeData = routeCollection.GetRouteData(GetHttpContextBase(URL, HttpMethod));

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual(true, TestIncomingRouteResults(routeData, controller, action, parameterSet));
        }


        public void TestRouteFail(string URL)
        {
            // Arrange
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            // Act
            RouteData result = routes.GetRouteData(GetHttpContextBase(URL));

            // Assert
            Assert.IsTrue(result == null || result.Route == null);

        }


        public bool TestIncomingRouteResults(RouteData routeData, string controllerName, string actionName, object parameterSet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
            {
                return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            };

            bool result = valCompare(routeData.Values["controller"], controllerName)
                && valCompare(routeData.Values["action"], actionName); 


            // Do the parameters expected match what's in the routeData
            if (parameterSet != null)
            {
                PropertyInfo[] parameters = parameterSet.GetType().GetProperties();

                foreach (PropertyInfo pInfo in parameters)
                {
                    if (routeData.Values.ContainsKey(pInfo.Name) &&
                        valCompare(routeData.Values[pInfo.Name], pInfo.GetValue(parameters, null)))
                    {
                        result = false;
                        break;
                    }
                }
            }
                
            return result;
        }


        
    }
}
