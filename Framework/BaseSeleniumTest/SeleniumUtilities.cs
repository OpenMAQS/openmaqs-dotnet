//--------------------------------------------------
// <copyright file="SeleniumUtilities.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>Utilities class for generic selenium methods</summary>
//--------------------------------------------------
using Maqs.BaseSeleniumTest.Extensions;
using Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using Selenium.Axe;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Static class for the selenium utilities
    /// </summary>
    public static class SeleniumUtilities
    {
        /// <summary>
        /// Capture a screenshot during execution and associate to the testObject
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The test object to associate and log to</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        public static bool CaptureScreenshot(this IWebDriver webDriver, ISeleniumTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                if (testObject.Log is IHtmlFileLogger htmlLogger)
                {
                    htmlLogger.EmbedImage(((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString);
                }
                else if (testObject.Log is IFileLogger fileLogger)
                {
                    // Calculate the file name
                    string fullpath = fileLogger.FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(fullpath)}{appendName}";
                    path = CaptureScreenshot(webDriver, testObject, directory, fileNameWithoutExtension, GetScreenShotFormat());
                }
                else
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(webDriver, testObject, LoggingConfig.GetLogDirectory(), $"ScreenCap{appendName}", GetScreenShotFormat());
                }

                testObject.Log.LogMessage(MessageType.INFORMATION, $"Screenshot saved: {path}");
                return true;
            }
            catch (Exception exception)
            {
                testObject.Log.LogMessage(MessageType.ERROR, $"Screenshot error: {exception}");
                return false;
            }
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The test object to associate the screenshot with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <param name="imageFormat">Optional Screenshot Image format parameter; Default imageFormat is PNG</param>
        /// <returns>Path to the log file</returns>
        public static string CaptureScreenshot(this IWebDriver webDriver, ISeleniumTestObject testObject, string directory, string fileNameWithoutExtension, ScreenshotImageFormat imageFormat = ScreenshotImageFormat.Png)
        {
            Screenshot screenShot = ((ITakesScreenshot)webDriver).GetScreenshot();

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, $"{fileNameWithoutExtension}.{imageFormat}");

            // Save the screenshot
            screenShot.SaveAsFile(path, imageFormat);
            testObject.AddAssociatedFile(path);

            return path;
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        public static bool SavePageSource(this IWebDriver webDriver, ISeleniumTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (testObject.Log is IFileLogger logger)
                {
                    // Calculate the file name
                    string fullpath = logger.FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(fullpath)}_PS{ appendName}";

                    path = SavePageSource(webDriver, testObject, directory, fileNameWithoutExtension);
                }
                else
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(webDriver, testObject, LoggingConfig.GetLogDirectory(), $"PageSource{appendName}");
                }

                testObject.Log.LogMessage(MessageType.INFORMATION, $"Page Source saved: {path}");
                return true;
            }
            catch (Exception exception)
            {
                testObject.Log.LogMessage(MessageType.ERROR, $"Page Source error: {exception}");
                return false;
            }
        }

        /// <summary>
        /// To capture Page Source during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        public static string SavePageSource(this IWebDriver webDriver, ISeleniumTestObject testObject, string directory, string fileNameWithoutExtension)
        {
            // Save the current page source into a string
            string pageSource = webDriver.PageSource;

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, $"{fileNameWithoutExtension}.txt");

            // Create new instance of Streamwriter and Auto Flush after each call
            StreamWriter writer = new StreamWriter(path, false)
            {
                AutoFlush = true
            };

            // Write page source to a new file
            writer.Write(pageSource);
            writer.Close();
            testObject.AddAssociatedFile(path);
            return path;
        }

        /// <summary>
        /// Get the web driver from a web element
        /// </summary>
        /// <param name="element">The web element</param>
        /// <returns>The web driver</returns>
        public static IWebDriver WebElementToWebDriver(IWebElement element)
        {
            // Handle lazy elements differently 
            if (element is AbstractLazyIWebElement)
            {
                var el = element as AbstractLazyIWebElement;
                EventFiringWebDriver eventfir = el.WebDriver as EventFiringWebDriver;

                if (eventfir != null)
                {
                    return eventfir.WrappedDriver;
                }

                return el.WebDriver;
            }

            // Extract the web driver from the element
            IWebDriver driver;

            // Get the parent driver - this is a protected property so we need to user reflection to access it
            var eventFiringPropertyInfo = element.GetType().GetProperty("ParentDriver", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            if (eventFiringPropertyInfo != null)
            {
                // This means we are using an event firing web driver
                driver = (IWebDriver)eventFiringPropertyInfo.GetValue(element, null);
            }
            else
            {
                // We failed to get the event firing web driver so they to get the wrapped web driver
                var propertyInfo = element.GetType().GetProperty("WrappedElement");

                if (propertyInfo != null)
                {
                    // This means we are likely using an event firing web driver
                    var value = (IWebElement)propertyInfo.GetValue(element, null);
                    driver = ((IWrapsDriver)value).WrappedDriver;
                }
                else
                {
                    driver = ((IWrapsDriver)element).WrappedDriver;
                }
            }

            return driver;
        }

        /// <summary>
        /// Gets the Screenshot Format to save images
        /// </summary>
        /// <returns>Desired ImageFormat Type</returns>
        public static ScreenshotImageFormat GetScreenShotFormat()
        {
            switch (SeleniumConfig.GetImageFormat().ToUpper())
            {
                case "BMP":
                    return ScreenshotImageFormat.Bmp;
                case "GIF":
                    return ScreenshotImageFormat.Gif;
                case "JPEG":
                    return ScreenshotImageFormat.Jpeg;
                case "PNG":
                    return ScreenshotImageFormat.Png;
                case "TIFF":
                    return ScreenshotImageFormat.Tiff;
                default:
                    throw new ArgumentException($"ImageFormat '{SeleniumConfig.GetImageFormat()}' is not a valid option");
            }
        }

        /// <summary>
        /// Make sure the web driver is shut down
        /// </summary>
        /// <param name="driver">The web driver</param>
        public static void KillDriver(this IWebDriver driver)
        {
            try
            {
                driver?.Close();
            }
            finally
            {
                driver?.Quit();
            }
        }

        /// <summary>
        /// Set the script and page timeouts using the default configuration timeout
        /// </summary>
        /// <param name="driver">Driver who's timeouts you want set</param>
        public static void SetTimeouts(IWebDriver driver)
        {
            SetTimeouts(driver, SeleniumConfig.GetTimeoutTime());
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Driver who's timeouts you want set</param>
        /// <param name="timeoutTime">Page load and JavaScript timeouts</param>
        public static void SetTimeouts(IWebDriver driver, TimeSpan timeoutTime)
        {
            driver.Manage().Timeouts().PageLoad = timeoutTime;
            driver.Manage().Timeouts().AsynchronousJavaScript = timeoutTime;
        }
    }
}
