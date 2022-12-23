//--------------------------------------------------
// <copyright file="ActionBuilder.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>Utilities class for generic accessibility methods</summary>
//--------------------------------------------------
using Maqs.BaseSeleniumTest;
using Maqs.BaseSeleniumTest.Extensions;
using Maqs.Utilities.Helper;
using Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Selenium.Axe;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Accecibility Utility tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AccessibilityUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static readonly string TestSiteUrl = SeleniumConfig.GetWebSiteBase();


        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static readonly string TestSiteAutomationUrl = TestSiteUrl + "index.html";

        /// <summary>
        /// Unit testing accessibility site URL - Login page
        /// </summary>
        private static readonly string TestSiteAccessibilityUrl = TestSiteUrl + "../Training1/LoginPage.html";

        /// <summary>
        /// Axe JSON with an error
        /// </summary>
        private const string AxeResultWithError = "{\"error\":\"AutomationError\", \"testEngine\": { \"name\":\"axe-core\",\"version\":\"3.4.1\"}, \"testRunner\": { \"name\":\"axe\"}, \"testEnvironment\": { \"userAgent\":\"AutoAgent\",\"windowWidth\": 1200, \"windowHeight\": 646, \"orientationAngle\": 0, \"orientationType\":\"landscape-primary\"},\"timestamp\":\"2020-04-14T01:33:59.139Z\",\"url\":\"url\",\"toolOptions\":{\"reporter\":\"v1\"},\"violations\":[],\"passes\":[],\"incomplete\":[],\"inapplicable\": []}";


        /// <summary>
        /// Verify we get verbose message back
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityCheckVerbose()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = ((IFileLogger)Log).FilePath;

            SeleniumUtilities.CheckAccessibility(this.TestObject);
            string logContent = File.ReadAllText(filePath);

            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 15 items"), "Expected to find 15 pass matches."));
            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 66 items"), "Expected to find 66 inapplicable matches."));
            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 6 items"), "Expected to find 6 violations matches."));
            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 0 items"), "Expected to find 0 incomplete matches."));
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify message levels are respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityCheckRespectsMessageLevel()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = Path.GetDirectoryName(((IFileLogger)Log).FilePath);
            FileLogger fileLogger = new FileLogger(filePath, "LevTest.txt", MessageType.WARNING);

            try
            {
                SeleniumUtilities.CheckAccessibility(TestObject.WebDriver, fileLogger);

                string logContent = File.ReadAllText(fileLogger.FilePath);

                Assert.IsTrue(!logContent.Contains("Passes check for"), "Did not expect expected to check for pass matches.");
                Assert.IsTrue(!logContent.Contains("Inapplicable check for"), "Did not expect expected to check for inapplicable matches.");
                Assert.IsTrue(!logContent.Contains("Incomplete check for"), "Did not expected to find any incomplete matches.");
                Assert.IsTrue(logContent.Contains("Found 6 items"), "Expected to find 6 violations matches.");
            }
            finally
            {
                File.Delete(fileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify inapplicable only check respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityInapplicableCheckRespectsMessageLevel()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = Path.GetDirectoryName(((IFileLogger)Log).FilePath);
            FileLogger fileLogger = new FileLogger(filePath, this.TestContext.TestName + ".txt", MessageType.INFORMATION);

            try
            {
                SeleniumUtilities.CheckAccessibilityInapplicable(TestObject.WebDriver, fileLogger, MessageType.WARNING, false);
                string logContent = File.ReadAllText(fileLogger.FilePath);
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Violations check"), "Did not expect violation check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Passes check"), "Did not expect pass check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Incomplete check"), "Did not expect incomplete check"));
                SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Inapplicable check"), "Did expect inapplicable check"));
            }
            finally
            {
                File.Delete(fileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify incomplete only check respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityIncompleteCheckRespectsMessageLevel()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = Path.GetDirectoryName(((IFileLogger)Log).FilePath);
            FileLogger fileLogger = new FileLogger(filePath, this.TestContext.TestName + ".txt", MessageType.INFORMATION);

            try
            {
                SeleniumUtilities.CheckAccessibilityIncomplete(TestObject.WebDriver, fileLogger, MessageType.WARNING, false);
                string logContent = File.ReadAllText(fileLogger.FilePath);
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Violations check"), "Did not expect violation check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Passes check"), "Did not expect pass check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Inapplicable check"), "Did not expect inapplicable check"));
                SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Incomplete check"), "Did expect incomplete check"));
            }
            finally
            {
                File.Delete(fileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify passes only check respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityPassesCheckRespectsMessageLevel()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = Path.GetDirectoryName(((IFileLogger)Log).FilePath);
            FileLogger fileLogger = new FileLogger(filePath, this.TestContext.TestName + ".txt", MessageType.INFORMATION);

            try
            {
                SeleniumUtilities.CheckAccessibilityPasses(TestObject.WebDriver, fileLogger, MessageType.SUCCESS);
                string logContent = File.ReadAllText(fileLogger.FilePath);
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Violations check"), "Did not expect violation check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Inapplicable check"), "Did not expect inapplicable check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Incomplete check"), "Did not expect incomplete check"));
                SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Passes check"), "Did expect pass check"));
            }
            finally
            {
                File.Delete(fileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify violation only check respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityViolationsCheckRespectsMessageLevel()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            string filePath = Path.GetDirectoryName(((IFileLogger)Log).FilePath);
            FileLogger fileLogger = new FileLogger(filePath, this.TestContext.TestName + ".txt", MessageType.INFORMATION);

            try
            {
                SeleniumUtilities.CheckAccessibilityViolations(TestObject.WebDriver, fileLogger, MessageType.ERROR, false);
                string logContent = File.ReadAllText(fileLogger.FilePath);

                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Passes check"), "Did not expect pass check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Inapplicable check"), "Did not expect inapplicable check"));
                SoftAssert.Assert(() => Assert.IsTrue(!logContent.Contains("Incomplete check"), "Did not expect incomplete check"));
                SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Violations check"), "Did expect violation check"));
            }
            finally
            {
                File.Delete(fileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify accessibility exception will be thrown
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(InvalidOperationException), "Expected an accessibility exception to be thrown")]
        public void AccessibilityCheckThrows()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            SeleniumUtilities.CheckAccessibility(this.TestObject, true);
        }

        /// <summary>
        /// Verify accessibility does not throw when no exception are found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityCheckNoThrowOnNoResults()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            // There should be 0 incomplete items found
            SeleniumUtilities.CheckAccessibilityIncomplete(TestObject.WebDriver, TestObject.Log, MessageType.WARNING, false);
        }

        /// <summary>
        /// Verify we can get readable results directly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityReadableResults()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            SeleniumUtilities.GetReadableAxeResults("TEST", this.WebDriver, this.WebDriver.Analyze().Violations, out string messages);

            Assert.IsTrue(messages.Contains("TEST check for"), "Expected header.");
            Assert.IsTrue(messages.Contains("Found 6 items"), "Expected to find 6 violations matches.");
        }

        /// <summary>
        /// Driver level accessibility report with no report type filter
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void DriverAccessibilityHtmlWithoutFilter()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            var report = File.ReadAllText(WebDriver.CreateAccessibilityHtmlReport(this.TestObject));

            Assert.IsTrue(report.Contains("Violation:"), "Expected to have violation check");
            Assert.IsTrue(report.Contains("Incomplete:"), "Expected to have incomplete check");
            Assert.IsTrue(report.Contains("Pass:"), "Expected to have pass check");
            Assert.IsTrue(report.Contains("Inapplicable:"), "Expected to have inapplicable check");
        }

        /// <summary>
        /// Element level accessibility report with no report type filter
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ElementAccessibilityHtmlWithoutFilter()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            var report = File.ReadAllText(WebDriver.CreateAccessibilityHtmlReport(this.TestObject, WebDriver.FindElement(By.CssSelector("BODY"))));

            Assert.IsTrue(report.Contains("Violation:"), "Expected to have violation check");
            Assert.IsTrue(report.Contains("Incomplete:"), "Expected to have incomplete check");
            Assert.IsTrue(report.Contains("Pass:"), "Expected to have pass check");
            Assert.IsTrue(report.Contains("Inapplicable:"), "Expected to have inapplicable check");
        }

        /// <summary>
        /// Driver level accessibility report with report type filter
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void DriverAccessibilityHtmlWithFilter()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            var report = File.ReadAllText(WebDriver.CreateAccessibilityHtmlReport(this.TestObject, false, ReportTypes.Violations | ReportTypes.Inapplicable));

            Assert.IsTrue(report.Contains("Violation:"), "Expected to have violation check");
            Assert.IsFalse(report.Contains("Incomplete:"), "Expected not to have incomplete check");
            Assert.IsFalse(report.Contains("Pass:"), "Expected not to have pass check");
            Assert.IsTrue(report.Contains("Inapplicable:"), "Expected to have inapplicable check");
        }

        /// <summary>
        /// Element level accessibility report with report type filter
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ElementAccessibilityHtmlWithFilter()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            var report = File.ReadAllText(WebDriver.CreateAccessibilityHtmlReport(this.TestObject, WebDriver.FindElement(By.CssSelector("BODY")), false, ReportTypes.Incomplete));

            Assert.IsFalse(report.Contains("Violation:"), "Expected not to have violation check");
            Assert.IsTrue(report.Contains("Incomplete:"), "Expected to have incomplete check");
            Assert.IsFalse(report.Contains("Pass:"), "Expected not to have pass check");
            Assert.IsFalse(report.Contains("Inapplicable:"), "Expected not to have inapplicable check");
        }

        /// <summary>
        /// Verify we can create and associate an accessibility HTML report
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityHtmlReport()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject);

            string file = this.TestObject.GetArrayOfAssociatedFiles().Last(x => x.EndsWith(".html"));
            Assert.IsTrue(new FileInfo(file).Length > 0, "Accessibility report is empty");
        }

        /// <summary>
        /// Verify we can create and associate multiple accessibility HTML reports
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityMultipleHtmlReports()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            // Create 3 reports
            WebDriver.CreateAccessibilityHtmlReport(this.TestObject);
            WebDriver.CreateAccessibilityHtmlReport(this.TestObject);
            WebDriver.CreateAccessibilityHtmlReport(this.TestObject);

            int count = this.TestObject.GetArrayOfAssociatedFiles().Count(x => x.EndsWith(".html"));
            Assert.IsTrue(count == 3, $"Expected 3 accessibility reports but see {count} instead");
        }

        /// <summary>
        /// Verify we throw an exception if the scan has an error
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessibilityHtmlReportWithError()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject, () => new AxeResult(JObject.Parse(AxeResultWithError)));

            string file = this.TestObject.GetArrayOfAssociatedFiles().Last(x => x.EndsWith(".html"));
            Assert.IsTrue(new FileInfo(file).Length > 0, "Accessibility report is empty");
        }

        /// <summary>
        /// Verify we throw an exception if the scan has an error and are using lazy elements
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessibilityHtmlReportWithErrorFromLazyElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            LazyElement foodTable = new LazyElement(this.TestObject, By.Id("FoodTable"));

            foodTable.CreateAccessibilityHtmlReport(this.TestObject, () => new AxeResult(JObject.Parse(AxeResultWithError)));

            string file = this.TestObject.GetArrayOfAssociatedFiles().Last(x => x.EndsWith(".html"));
            Assert.IsTrue(new FileInfo(file).Length > 0, "Accessibility report is empty");
        }

        /// <summary>
        /// Verify we throw an exception if there are violations and we choose the throw exception option
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessibilityHtmlReportWithViolation()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAccessibilityUrl);
            WebDriver.Wait().ForPageLoad();

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject, true);
        }

        /// <summary>
        /// Verify we can create an accessibility HTML report off a lazy element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityHtmlReportWithLazyElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().ForPageLoad();

            LazyElement foodTable = new LazyElement(this.TestObject, By.Id("FoodTable"));

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject, foodTable);

            string file = this.TestObject.GetArrayOfAssociatedFiles().Last(x => x.EndsWith(".html"));
            Assert.IsTrue(new FileInfo(file).Length > 0, "Accessibility report is empty");
        }

        /// <summary>
        /// Verify we can create an accessibility HTML report off a normal web element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityHtmlReportWithElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().ForPageLoad();

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject, WebDriver.FindElement(By.Id("FoodTable")));

            string file = this.TestObject.GetArrayOfAssociatedFiles().Last(x => x.EndsWith(".html"));
            Assert.IsTrue(new FileInfo(file).Length > 0, "Accessibility report is empty");
        }


        /// <summary>
        /// Verify we suppress the JS logging assoicated with running Axe
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void AccessibilityHtmlLogSuppression()
        {
            // Make sure we are not using verbose logging
            this.Log.SetLoggingLevel(MessageType.INFORMATION);

            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().ForPageLoad();

            WebDriver.CreateAccessibilityHtmlReport(this.TestObject);

            // The script executed message should be suppressed when we run the accessablity check
            Assert.IsFalse(File.ReadAllText(((IFileLogger)this.Log).FilePath).Contains("Script executed"), "Logging was not suppressed as expected");
        }
    }
}
