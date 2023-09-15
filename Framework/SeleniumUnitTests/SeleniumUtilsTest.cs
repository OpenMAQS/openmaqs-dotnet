//-----------------------------------------------------
// <copyright file="SeleniumUtilsTest.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test the selenium framework</summary>
//-----------------------------------------------------
using OpenMaqs.BaseSeleniumTest;
using OpenMaqs.BaseSeleniumTest.Extensions;
using OpenMaqs.Utilities.Helper;
using OpenMaqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Selenium.Axe;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        /// Unit testing site URL - Login page
        /// </summary>
        private static readonly string TestSiteUrl = SeleniumConfig.GetWebSiteBase();

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
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Bmp);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".Bmp", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Bmp' format");
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
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Gif);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".Gif", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Gif' format");
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
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", imageFormat: ScreenshotImageFormat.Jpeg);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".Jpeg", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Jpeg' format");
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
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", imageFormat: ScreenshotImageFormat.Png);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".Png", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Png' format");
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
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(WebDriver, TestObject, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Tiff);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(".Tiff", Path.GetExtension(screenShotPath), "The screenshot format was not in '.Tiff' format");
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
            Assert.AreEqual(ScreenshotImageFormat.Png, SeleniumUtilities.GetScreenShotFormat(), "The Incorrect Image Format was returned, expected: " + Config.GetGeneralValue("ImageFormat"));
        }
    }
}
