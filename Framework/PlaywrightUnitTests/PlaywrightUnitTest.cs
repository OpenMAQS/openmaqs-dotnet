//-----------------------------------------------------
// <copyright file="PlaywrightUnitTest.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test the playwright framework</summary>
//-----------------------------------------------------
using OpenMAQS.Maqs.BasePlaywrightTest;
using OpenMAQS.Maqs.Utilities.Helper;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightUnitTests
{
    /// <summary>
    /// Test the playwright framework
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PlaywrightUnitTest : BasePlaywrightTest
    {
        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static readonly string TestSiteUrl = PlaywrightConfig.WebBase();

        /// <summary>
        /// Unit testing site URL - Async page
        /// </summary>
        private static readonly string TestSiteAsyncUrl = TestSiteUrl + "async.html";

        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static readonly string TestSiteAutomationUrl = TestSiteUrl + "index.html";

        /// <summary>
        /// Home button css selector
        /// </summary>
        private PlaywrightSyncElement HomeButtonCssSelector
        {
            get {return new PlaywrightSyncElement(this.PageDriver, "#homeButton > a"); }
        }
        /// <summary>
        /// Home button css selector
        /// </summary>
        private PlaywrightSyncElement DropdownToggleClassSelector
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "dropdown-toggle");
            }
        }

        /// <summary>
        /// Dropdown selector
        /// </summary>
        private PlaywrightSyncElement AsyncDropdownCssSelector
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#Selector");
            }
        }

        /// <summary>
        /// Dropdown label
        /// </summary>
        private PlaywrightSyncElement AsyncOptionsLabel
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#Label");
            }
        }

        /// <summary>
        /// Dropdown label - hidden once dropdown loads
        /// </summary>
        private PlaywrightSyncElement AsyncLoadingLabel
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#LoadingLabel");
            }
        }

        /// <summary>
        /// Asynchronous div that loads after a delay on Async Testing Page
        /// </summary>
        private PlaywrightSyncElement AsyncLoadingTextDiv
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#loading-div-text");
            }
        }

        /// <summary>
        /// Names label
        /// </summary>
        private PlaywrightSyncElement AutomationNamesLabel
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#Dropdown > p > strong > label");
            }
        }

        /// <summary>
        /// Selector that is not in page
        /// </summary>
        private PlaywrightSyncElement NotInPage
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "NOTINPAGE");
            }
        }

        /// <summary>
        /// First dialog button
        /// </summary>
        private PlaywrightSyncElement AutomationShowDialog1
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#showDialog1");
            }
        }

        /// <summary>
        /// Food table
        /// </summary>
        private PlaywrightSyncElement FoodTable
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#FoodTable");
            }
        }

        /// <summary>
        /// Flower table
        /// </summary>
        private PlaywrightSyncElement FlowerTable
        {
            get
            {
                return new PlaywrightSyncElement(this.PageDriver, "#FlowerTable TD");
            }
        }

        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void OpenBrowser()
        {
            PageDriver.Goto(TestSiteUrl);
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertTest()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            string title = PageDriver.Title();
            SoftAssert.Assert(() => Assert.AreEqual("Automation - MAQS Test Site", PageDriver.Title()), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsFalseTest()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            SoftAssert.Assert(() => Assert.IsFalse("Automation".Equals(PageDriver.Title())), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsTrueTest()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            SoftAssert.Assert(() => Assert.IsTrue(PageDriver.Title().Contains("Automation")), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertEmptyMessageTest()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            SoftAssert.Assert(() => Assert.IsTrue(PageDriver.Title().Contains("Automation")), "", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        //[MyExpectedException(typeof(AggregateException))]
        public void PlaywrightSoftAssertIsTrueFalseBoolean()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            var result = SoftAssert.Assert(() => Assert.IsTrue(!PageDriver.Title().Contains("Automation")), "", "Title is incorrect");
            Assert.IsFalse(result);
            Assert.Throws<AggregateException>(() => this.SoftAssert.FailTestIfAssertFailed());
        }

        /// <summary>
        /// Verify that a screenshot is taken if the PlaywrightSoftAssert.IsTrue gets a false condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsTrueFalseCondition()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "PlaywrightSoftAssertIsTrueFalseCondition.txt", MessageType.GENERIC, true);
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Png";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsTrue(false), "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), $"Fail to find screenshot: {screenShotLocation}");
            File.Delete(screenShotLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is taken if the PlaywrightSoftAssert.IsTrue gets a false condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertWithAssertIsTrueFalseCondition()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "PlaywrightSoftAssertWithAssertIsTrueFalseCondition.txt", MessageType.GENERIC, true);
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string screenShotLocation = $"{logLocation.Substring(0, logLocation.LastIndexOf('.'))} 1 (1).Png";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.Fail("testSoftAssert"), "TestAssertOne", "Test assert fail message");

            Assert.IsTrue(File.Exists(screenShotLocation), $"Fail to find screenshot: {screenShotLocation}");
            File.Delete(screenShotLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is saved if the PlaywrightSoftAssert.IsTrue gets a false condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsTrueFalseConditionPageSource()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "PlaywrightSoftAssertIsTrueFalseConditionPageSource.txt", MessageType.GENERIC, true);
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsTrue(false, "testSoftAssert", "message"));

            Assert.IsTrue(File.Exists(pageSourceLocation), "Fail to find page source");
            File.Delete(pageSourceLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is saved if the PlaywrightSoftAssert.IsTrue gets a false condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertWithAssertIsTrueFalseConditionPageSource()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "PlaywrightSoftAssertIsTrueFalseConditionPageSource.txt", MessageType.GENERIC, true);
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsTrue(false), "1", "message");

            Assert.IsTrue(File.Exists(pageSourceLocation), "Fail to find page source");
            File.Delete(pageSourceLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is taken if the playwrightSoftAssert.IsFalse gets a true condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void playwrightSoftAssertIsFalseTrueCondition()
        {
            // Make sure we initialized the web driver
            Assert.IsNotNull(this.PageDriver);

            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Png";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsFalse(true), "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), $"Fail to find screenshot: {screenShotLocation}");
            File.Delete(screenShotLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is not taken if no browser is initialized and the PlaywrightSoftAssert.IsFalse gets a true condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsFalseTrueConditionNoBrowser()
        {
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Jpeg";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsFalse(true), "testSoftAssert", "message");

            Assert.IsFalse(File.Exists(screenShotLocation), "Should not have taken screenshot");
            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is saved if the PlaywrightSoftAssert.IsFalse gets a true condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsFalseTrueConditionPageSource()
        {
            // Make sure we initialized the web driver
            Assert.IsNotNull(this.PageDriver);

            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsFalse(true), "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(pageSourceLocation), "Fail to find page source");
            File.Delete(pageSourceLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is not saved if no browser is initialized and the playwrightSoftAssert.IsFalse gets a true condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertIsFalseTrueConditionPageSourceNoBrowser()
        {
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = playwrightSoftAssert.Assert(() => Assert.IsFalse(true), "testSoftAssert", "message");

            Assert.IsTrue(!File.Exists(pageSourceLocation), "Should not have captured page source");
            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that PlaywrightSoftAssert.AreEqual works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightSoftAssertAreEqual()
        {
            PlaywrightSoftAssert playwrightSoftAssert = new PlaywrightSoftAssert(TestObject);
            bool isTrue = playwrightSoftAssert.Assert(() => Assert.AreEqual("test string", "test string"), "test message");
            Assert.IsTrue(isTrue);
        }

        /// <summary>
        /// Test to check if the soft assert fails.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        //[MyExpectedException(typeof(AggregateException))]
        public void PlaywrightSoftAssertExpectFail()
        {
            this.PageDriver.Goto(TestSiteAutomationUrl);
            SoftAssert.Assert(() => Assert.AreEqual("Wrong Title", PageDriver.Title()), "Title Test", "Title is incorrect");
            Assert.Throws<AggregateException>(() => SoftAssert.FailTestIfAssertFailed());
        }



        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        [TestCategory(TestCategories.Utilities)]
        public void PlaywrightTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PageDriver, this.PageDriver, "Web drivers don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        [TestCategory(TestCategories.Utilities)]
        public void PlaywrightTestObjectValuesCanBeUsed()
        {
            TestObject.SetValue("1", "one");

            Assert.AreEqual("one", TestObject.Values["1"]);
            Assert.IsFalse(TestObject.Values.TryGetValue("2", out string outValue), "Didn't expect to get value for key '2', but got " + outValue);
        }

        /// <summary>
        /// Make sure the test object objects are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        [TestCategory(TestCategories.Utilities)]
        public void PlaywrightTestObjectObjectssCanBeUsed()
        {
            StringBuilder builder = new StringBuilder();
            TestObject.SetObject("1", builder);

            Assert.AreEqual(TestObject.Objects["1"], builder);

            Assert.IsFalse(TestObject.Objects.TryGetValue("2", out object outObject), "Didn't expect to get value for key '2'");
            Assert.IsNull(outObject, "Object should be null");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)TestObject.Objects["1"]).ToString(), builder.ToString());
        }

    
        /// <summary>
        /// Verify that CreateNewTestObject creates the correct Test Object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Playwright)]
        public void PlaywrightCreateNewTestObject()
        {
            this.SetupTestObject();
            IPlaywrightTestObject newTestObject = TestObject;

            var expectedWebDriver = PageDriver.ToString();
            var actualWebDriver = newTestObject.PageDriver.ToString();
            var expectedLog = Log.ToString();
            var actualLog = newTestObject.Log.ToString();
            var expectedSoftAssert = SoftAssert.ToString();
            var actualSoftAssert = newTestObject.SoftAssert.ToString();
            var expectedPerf = PerfTimerCollection.ToString();
            var actualPerf = newTestObject.PerfTimerCollection.ToString();

            // Need to quit webdriver here in order for remote browser to close browser correctly.
            newTestObject.PageDriver.Dispose();
            Assert.AreEqual(expectedWebDriver, actualWebDriver);
            Assert.AreEqual(expectedLog, actualLog);
            Assert.AreEqual(expectedSoftAssert, actualSoftAssert);
            Assert.AreEqual(expectedPerf, actualPerf);
        }

        /// <summary>
        /// Helper function to remove created custom file logs and return contained string
        /// </summary>
        /// <returns>string contained in custom log</returns>
        protected string GetAndRemoveCustomFileLog()
        {
            IFileLogger outputLog = (IFileLogger)Log;
            string log = File.ReadAllText(outputLog.FilePath);
            try
            {
                File.Delete(outputLog.FilePath);
                string screenShotLocation = outputLog.FilePath.Substring(0, outputLog.FilePath.LastIndexOf('.')) + ".Png";
                File.Delete(screenShotLocation);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found exception " + e);
            }

            this.Log = new FileLogger();
            return log;
        }
    }
}