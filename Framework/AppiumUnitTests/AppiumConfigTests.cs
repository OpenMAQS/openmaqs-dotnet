﻿//--------------------------------------------------
// <copyright file="AppiumConfigTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseAppiumTest;
using OpenMAQS.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AppiumUnitTests
{
    /// <summary>
    /// Appium Config Unit Tests
    /// </summary>
    [TestClass]
    public class AppiumConfigTests
    {
        /// <summary>
        /// Setup config for test class
        /// </summary>
        /// <param name="testContext">The test context</param>
        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            Config.UpdateWithVSTestContext(testContext);
        }

        /// <summary>
        /// Test for getting Mobile Device OS
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetPlatformNameTest()
        {
            Assert.AreEqual("Android", AppiumConfig.GetPlatformName());
        }

        /// <summary>
        /// Test for getting mobile OS version
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetPlatformVersionTest()
        {
            Assert.AreEqual("13.0", AppiumConfig.GetPlatformVersion());
        }

        /// <summary>
        /// Test for getting device name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetDeviceNameTest()
        {
            Assert.AreEqual("Google Pixel 7 Pro", AppiumConfig.GetDeviceName());
        }

        /// <summary>
        /// Get command timeout test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetCommandTimeout()
        {
            TimeSpan initTimeout = AppiumConfig.GetMobileCommandTimeout();

            Assert.AreEqual(200, initTimeout.TotalSeconds);
        }

        /// <summary>
        /// Test for creating Mobile Device driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void MobileDeviceTest()
        {
            AppiumDriver driver = AppiumDriverFactory.GetDefaultMobileDriver();

            try
            {
                Assert.IsNotNull(driver);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        /// <summary>
        /// Test for getting Mobile Hub Url
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileHubUrlTest()
        {
            Assert.AreEqual("https://hub.browserstack.com/wd/hub/", AppiumConfig.GetMobileHubUrl().AbsoluteUri);
        }

        /// <summary>
        /// Test for getting instance of Wait Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetWaitDriverTest()
        {
            AppiumDriver driver = AppiumDriverFactory.GetDefaultMobileDriver();
            WebDriverWait wait = AppiumUtilities.GetDefaultWaitDriver(driver);
            try
            {
                Assert.IsNotNull(wait);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}