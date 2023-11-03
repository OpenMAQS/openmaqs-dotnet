//--------------------------------------------------
// <copyright file="AppiumIosUnitTests.cs" company="OpenMAQS">
//  Copyright 2022 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test class for ios related functions</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseAppiumTest;
using OpenMAQS.Maqs.Utilities.Helper;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace AppiumUnitTests
{
    /// <summary>
    /// iOS related Appium tests
    /// </summary>
    [TestClass]
    public class AppiumIosUnitTests : BaseAppiumTest
    {
        private static readonly string BuildDate = DateTime.Now.ToString("MMddyyyy hhmmss");

        /// <summary>
        /// Tests the creation of the Appium iOS Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AppiumIOSDriverTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
        }

        /// <summary>
        /// Tests lazy element with Appium iOS Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AppiumIOSDriverLazyTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.AppiumDriver.Navigate().GoToUrl(Config.GetValueForSection(ConfigSection.AppiumMaqs, "WebSiteBase"));
            LazyMobileElement lazy = new LazyMobileElement(this.TestObject, By.XPath("//button[@class=\"navbar-toggle\"]"), "Nav toggle");

            Assert.IsTrue(lazy.Enabled, "Expect enabled");
            Assert.IsTrue(lazy.Displayed, "Expect displayed");
            Assert.IsTrue(lazy.ExistsNow, "Expect exists now");
            lazy.Click();
        }

        /// <summary>
        /// Assert function fail path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AssertFuncFailPath()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.AppiumDriver.Navigate().GoToUrl(Config.GetValueForSection(ConfigSection.AppiumMaqs, "WebSiteBase"));

            Log = new FileLogger(string.Empty, "AssertFuncFailPath.txt", MessageType.GENERIC, true);
            AppiumSoftAssert appiumSoftAssert = new AppiumSoftAssert(TestObject);
            string logLocation = ((IFileLogger)Log).FilePath;
            string screenShotLocation = $"{logLocation.Substring(0, logLocation.LastIndexOf('.'))} assertName (1).Png";

            bool isFalse = appiumSoftAssert.Assert(() => Assert.IsTrue(false), "assertName");
            Assert.IsTrue(File.Exists(screenShotLocation), "Fail to find screenshot");
            File.Delete(screenShotLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Sets capabilities for testing the iOS Driver creation
        /// </summary>
        /// <returns>iOS instance of the Appium Driver</returns>
        protected override AppiumDriver GetMobileDevice()
        {
            AppiumOptions options = new AppiumOptions
            {
                DeviceName = "iPhone 14",
                PlatformName = "iOS",
                PlatformVersion = "16",
                BrowserName = "Safari"
            };

            var name = this.TestContext.FullyQualifiedTestClassName + "." + this.TestContext.TestName;

            var bstackOptions = AppiumConfig.GetCapabilitiesAsObjects();

            // Use Appium 1.22 for running iOS tests
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["appiumVersion"] = "1.22.0";
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["deviceName"] = "iPhone 14";
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["osVersion"] = "16";
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["buildName"] = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["projectName"] = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BROWSERSTACK_PROJECT_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("BROWSERSTACK_PROJECT_NAME");
            (bstackOptions["bstack:options"] as Dictionary<string, object>)["sessionName"] = name;
            options.SetMobileOptions(bstackOptions);

            return AppiumDriverFactory.GetIOSDriver(AppiumConfig.GetMobileHubUrl(), options, AppiumConfig.GetMobileCommandTimeout());
        }
    }
}
