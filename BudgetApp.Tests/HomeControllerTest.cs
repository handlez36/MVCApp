using BudgetApp.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using BudgetApp.Domain.Abstract;
using BudgetApp.Domain.Concrete;
using System.Web.Mvc;
using System.Collections.Generic;
using Moq;
using System.Linq;

namespace BudgetApp.Tests
{
    
    /// <summary>
    ///This is a test class for HomeControllerTest and is intended
    ///to contain all HomeControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HomeControllerTest
    {
        private Mock<ILedgerEntries> SetMockEntries()
        {
            Mock<ILedgerEntries> MockEntries = new Mock<ILedgerEntries>();
            MockEntries.Setup(m => m.GetEntries).Returns(new LedgerEntry[] {
                new LedgerEntry { EntryType = "CREDIT", Category = "Test", Description = "First entry", LedgerName = "First Ledger", Time = DateTime.Now },
                new LedgerEntry { EntryType = "CREDIT", Category = "Test", Description = "Second entry", LedgerName = "First Ledger", Time = DateTime.Now },
                new LedgerEntry { EntryType = "CREDIT", Category = "Test", Description = "Third entry", LedgerName = "First Ledger", Time = DateTime.Now }
            }.AsQueryable());

            return MockEntries;
        }

        [TestMethod]
        public void List_Entries_Test()
        {
            // Arrange
            Mock<ILedgerEntries> MockEntries = SetMockEntries();

            // Act
            HomeController Controller = new HomeController(MockEntries.Object);
            ViewResult Result = Controller.List();
            IEnumerable<LedgerEntry> model = (IEnumerable<LedgerEntry>)Result.Model;

            // Assert
            Assert.AreEqual("First entry", model.ElementAt(0).Description);
            Assert.AreEqual("Second entry", model.ElementAt(1).Description);
            Assert.AreEqual("Third entry", model.ElementAt(2).Description);
        }


        /// <summary>
        ///A test for List
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void ListTest()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries); // TODO: Initialize to an appropriate value
            int page = 0; // TODO: Initialize to an appropriate value
            ViewResult expected = null; // TODO: Initialize to an appropriate value
            ViewResult actual;
            actual = target.List(page);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Edit
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void EditTest()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries); // TODO: Initialize to an appropriate value
            LedgerEntry entry = null; // TODO: Initialize to an appropriate value
            bool add = false; // TODO: Initialize to an appropriate value
            ActionResult expected = null; // TODO: Initialize to an appropriate value
            ActionResult actual;
            actual = target.Edit(entry, add);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Edit
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void EditTest1()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries); // TODO: Initialize to an appropriate value
            int entryID = 0; // TODO: Initialize to an appropriate value
            ViewResult expected = null; // TODO: Initialize to an appropriate value
            ViewResult actual;
            actual = target.Edit(entryID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void DeleteTest()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries); // TODO: Initialize to an appropriate value
            int entryID = 0; // TODO: Initialize to an appropriate value
            ActionResult expected = null; // TODO: Initialize to an appropriate value
            ActionResult actual;
            actual = target.Delete(entryID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for About
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void AboutTest()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries); // TODO: Initialize to an appropriate value
            ActionResult expected = null; // TODO: Initialize to an appropriate value
            ActionResult actual;
            actual = target.About();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for HomeController Constructor
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\BMac\\Documents\\Visual Studio 2010\\Projects\\BudgetApp\\BudgetApp", "/")]
        [UrlToTest("http://localhost:59865/")]
        public void HomeControllerConstructorTest()
        {
            ILedgerEntries budgetEntries = null; // TODO: Initialize to an appropriate value
            HomeController target = new HomeController(budgetEntries);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
