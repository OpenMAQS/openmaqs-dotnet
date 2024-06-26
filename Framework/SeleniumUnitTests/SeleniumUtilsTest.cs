﻿//-----------------------------------------------------
// <copyright file="SeleniumUtilsTest.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test the selenium framework</summary>
//-----------------------------------------------------
using OpenMAQS.Maqs.BaseSeleniumTest;
using OpenMAQS.Maqs.BaseSeleniumTest.Extensions;
using OpenMAQS.Maqs.Utilities.Helper;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Deque.AxeCore.Selenium;
using TWP.Selenium.Axe.Html;
using Deque.AxeCore.Commons;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Utility tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumUtilsTest : BaseSeleniumTest
    {
        /// <summary>
        /// Axe JSON with an error
        /// </summary>
        private const string AxeResultWithError = "{\"error\":\"AutomationError\", \"testEngine\": { \"name\":\"axe-core\",\"version\":\"3.4.1\"}, \"testRunner\": { \"name\":\"axe\"}, \"testEnvironment\": { \"userAgent\":\"AutoAgent\",\"windowWidth\": 1200, \"windowHeight\": 646, \"orientationAngle\": 0, \"orientationType\":\"landscape-primary\"},\"timestamp\":\"2020-04-14T01:33:59.139Z\",\"url\":\"url\",\"toolOptions\":{\"reporter\":\"v1\"},\"violations\":[],\"passes\":[],\"incomplete\":[],\"inapplicable\": []}";

        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static readonly string TestSiteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Unit testing accessibility site URL - Login page
        /// </summary>
        private static readonly string TestSiteAccessibilityUrl = TestSiteUrl + "../Training1/LoginPage.html";

        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static readonly string TestSiteAutomationUrl = TestSiteUrl + "index.html";

        /// <summary>
        /// First dialog button
        /// </summary>
        private static readonly By AutomationShowDialog1 = By.CssSelector("#showDialog1");

        /// <summary>
        /// Verify CaptureScreenshot works - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshot()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((IFileLogger)TestObject.Log).FilePath, ".Png");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works with console logger - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshotWithConsoleLogger()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            // Create a console logger and calculate the file location
            ConsoleLogger consoleLogger = new ConsoleLogger();
            TestObject.Log = consoleLogger;
            string expectedPath = Path.Combine(LoggingConfig.GetLogDirectory(), "ScreenCapDelete.Png");

            // Take a screenshot
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject, "Delete");

            // Make sure we got the screenshot and than cleanup
            Assert.IsTrue(File.Exists(expectedPath), $"Fail to find screenshot at {expectedPath}");
            File.Delete(expectedPath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works with HTML File logger - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshotWithHTMLFileLogger()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            // Create a console logger and calculate the file location
            HtmlFileLogger htmlFileLogger = new HtmlFileLogger(LoggingConfig.GetLogDirectory());
            TestObject.Log = htmlFileLogger;

            // Take a screenshot
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject, "Delete");

            Stream fileStream = null;
            string logContents = string.Empty;

            // This will open the Log file and read in the text
            try
            {
                fileStream = new FileStream(htmlFileLogger.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using StreamReader textReader = new StreamReader(fileStream);
                fileStream = null;
                logContents = textReader.ReadToEnd();
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            // Find the base64 encoded string
            Regex pattern = new Regex("src='data:image/png;base64, (?<image>[^']+)'");
            var matches = pattern.Match(logContents);

            // Try to convert the Base 64 string to find if it is a valid string
            try
            {
                Convert.FromBase64String(matches.Groups["image"].Value);
            }
            catch (FormatException)
            {
                Assert.Fail("image saves was not a Base64 string");
            }
            finally
            {
                File.Delete(htmlFileLogger.FilePath);
            }
        }

        /// <summary>
        /// Verify that CaptureScreenshot properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotThrownException()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "<>\0" // illegal file path
            };

            TestObject.Log = tempLogger;
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            bool successfullyCaptured = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject);
            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that CaptureScreenshot creates Directory if it does not exist already
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotNoExistingDirectory()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "CapScreenShotNoDir", SeleniumUtilities.GetScreenShotFormat());
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that the captured screenshot is associated to the test object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CapturedScreenshotTestObjectAssociation()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string pagePicPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TestObjAssocTest");

            Assert.IsTrue(TestObject.ContainsAssociatedFile(pagePicPath), "The captured screenshot wasn't added to the associated files");

            File.Delete(pagePicPath);
        }

        /// <summary>
        /// Verify that page source file is being created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumPageSourceFileIsCreated()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string pageSourcePath = SeleniumUtilities.SavePageSource(WebDriver, TestObject, "TempTestDirectory", "SeleniumPSFile");
            Assert.IsTrue(File.Exists(pageSourcePath), "Failed to find Page Source");
            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Verify SavePageSource works with console logger - Validating that page source was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumPageSourceWithConsoleLogger()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();

            // Create a console logger and calculate the file location
            ConsoleLogger consoleLogger = new ConsoleLogger();
            string expectedPath = Path.Combine(LoggingConfig.GetLogDirectory(), "PageSourceConsole.txt");
            TestObject.Log = consoleLogger;

            // Take a screenshot
            SeleniumUtilities.SavePageSource(this.WebDriver, this.TestObject, "Console");

            // Make sure we got the screenshot and than cleanup
            Assert.IsTrue(File.Exists(expectedPath), "Fail to find screenshot");
            File.Delete(expectedPath);
        }

        /// <summary>
        /// Verify that SavePageSource properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SavePageSourceThrownException()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "<>\0" // illegal file path
            };

            TestObject.Log = tempLogger;
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            bool successfullySaved = SeleniumUtilities.SavePageSource(WebDriver, TestObject);
            Assert.IsFalse(successfullySaved);
        }

        /// <summary>
        /// Verify that the captured screenshot is associated to the test object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SavedPageSourceTestObjectAssociation()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string pageSourcePath = SeleniumUtilities.SavePageSource(WebDriver, TestObject, "TempTestDirectory", "TestObjAssocTest");

            Assert.IsTrue(TestObject.ContainsAssociatedFile(pageSourcePath), "The saved page source wasn't added to the associated files");

            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Test Lazy WebElementToDriver with an unwrappedDriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyWebElementToWebDriverUnwrappedDriver()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebDriver driver = this.WebDriver.GetLowLevelDriver();
            LazyElement lazy = new LazyElement(this.TestObject, driver, AutomationShowDialog1);

            IWebDriver basedriver = SeleniumUtilities.WebElementToWebDriver(lazy);
            Assert.AreEqual("OpenQA.Selenium.Chrome.ChromeDriver", basedriver.GetType().ToString());
        }

        /// <summary>
        /// Test WebElementToDriver with an unwrappedDriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WebElementToWebDriverUnwrappedDriver()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebDriver driver = ((IWrapsDriver)WebDriver).WrappedDriver;
            IWebElement element = driver.FindElement(AutomationShowDialog1);

            IWebDriver basedriver = SeleniumUtilities.WebElementToWebDriver(element);
            Assert.AreEqual("OpenQA.Selenium.Chrome.ChromeDriver", basedriver.GetType().ToString());
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the bitmap image format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotBmpFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", "bmp");
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".bmp", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Bmp' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Graphics Interchange Format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotGifFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", "gif");
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".gif", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Gif' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Joint Photographic Experts Group format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotJpegFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", imageFormat: "jpeg");
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".jpeg", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Jpeg' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Portable Network Graphics format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotPngFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", imageFormat: "png");
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".png", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Png' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Tagged Image File Format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotTiffFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", "tiff");
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".tiff", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Tiff' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works - With Modified ImageFormat Config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshotImageFormat()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((IFileLogger)this.Log).FilePath, SeleniumUtilities.GetScreenShotFormat().ToString());
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(filePath), "." + SeleniumUtilities.GetScreenShotFormat().ToString(), "The screenshot format was not in correct Format");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify the GetScreenShotFormat function gets the correct value from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetImageFormatFromConfig()
        {
            Assert.AreEqual("png", SeleniumUtilities.GetScreenShotFormat(), "The Incorrect Image Format was returned, expected: " + Config.GetGeneralValue("ImageFormat"));
        }

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

            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 13 items"), "Expected to find 13 pass matches."));
            SoftAssert.Assert(() => Assert.IsTrue(logContent.Contains("Found 70 items"), "Expected to find 70 inapplicable matches."));
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
        [ExpectedException(typeof(Exception))]
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
        [ExpectedException(typeof(Exception))]
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
