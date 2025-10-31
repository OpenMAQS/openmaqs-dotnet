//--------------------------------------------------
// <copyright file="BasePlaywrightTest.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>This is the base Playwright test class</summary>
//--------------------------------------------------
using Microsoft.Playwright;
using OpenMAQS.Maqs.Utilities.Logging;
using System;
using System.IO;

namespace OpenMAQS.Maqs.BasePlaywrightTest
{
    /// <summary>
    /// Static class for the playwright utilities
    /// </summary>
    public static class PlaywrightUtilities
    {
        /// <summary>
        /// Capture a screenshot during execution and associate to the testObject
        /// </summary>
        /// <param name="pageDriver">The PageDriver</param>
        /// <param name="testObject">The test object to associate and log to</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        public static bool CaptureScreenshot(this PageDriver pageDriver, IPlaywrightTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                if (testObject.Log is IHtmlFileLogger htmlLogger)
                {
                    htmlLogger.EmbedImage(Convert.ToBase64String(pageDriver.AsyncPage.ScreenshotAsync().Result));
                }
                else if (testObject.Log is IFileLogger fileLogger)
                {
                    // Calculate the file name
                    string fullpath = fileLogger.FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(fullpath)}{appendName}";
                    path = CaptureScreenshot(pageDriver, testObject, directory, fileNameWithoutExtension, GetScreenShotFormat());
                }
                else
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(pageDriver, testObject, LoggingConfig.GetLogDirectory(), $"ScreenCap{appendName}", GetScreenShotFormat());
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
        /// <param name="pageDriver">The WebDriver</param>
        /// <param name="testObject">The test object to associate the screenshot with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <param name="imageFormat">Optional Screenshot Image format parameter; Default imageFormat is PNG, hardcoded due to enum being deprecated</param>
        /// <returns>Path to the log file</returns>
        public static string CaptureScreenshot(this PageDriver pageDriver, IPlaywrightTestObject testObject, string directory, string fileNameWithoutExtension, ScreenshotType imageFormat = ScreenshotType.Jpeg)
        {

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, $"{fileNameWithoutExtension}.{imageFormat.ToString()}");

            // Save the screenshot
            pageDriver.AsyncPage.ScreenshotAsync(new()
            {
                Path = path,
                FullPage = true,
                Type = imageFormat
            }).Wait();
            testObject.AddAssociatedFile(path);
            return path;
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="pageDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        public static bool SavePageSource(this PageDriver pageDriver, IPlaywrightTestObject testObject, string appendName = "")
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
                    string fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(fullpath)}_PS{appendName}";

                    path = SavePageSource(pageDriver, testObject, directory, fileNameWithoutExtension);
                }
                else
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(pageDriver, testObject, LoggingConfig.GetLogDirectory(), $"PageSource{appendName}");
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
        /// <param name="pageDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        public static string SavePageSource(this PageDriver pageDriver, IPlaywrightTestObject testObject, string directory, string fileNameWithoutExtension)
        {
            // Save the current page source into a string
            string pageSource = pageDriver.AsyncPage.ContentAsync().Result;

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
        /// Gets the Screenshot Format to save images
        /// </summary>
        /// <returns>Desired ImageFormat Type</returns>
        public static ScreenshotType GetScreenShotFormat()
        {
            switch (PlaywrightConfig.GetImageFormat().ToUpper())
            {
                case "JPEG":
                    return ScreenshotType.Jpeg;
                case "PNG":
                    return ScreenshotType.Png;
                default:
                    throw new ArgumentException($"ImageFormat '{PlaywrightConfig.GetImageFormat()}' is not a valid option");
            }
        }
    }
}