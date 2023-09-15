//--------------------------------------------------
// <copyright file="AppiumTestObject.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Holds Appium context data</summary>
//--------------------------------------------------
using OpenMaqs.BaseTest;
using OpenMaqs.Utilities.Logging;
using OpenQA.Selenium.Appium;
using System;

namespace OpenMaqs.BaseAppiumTest
{
    /// <summary>
    /// Appium test context data
    /// </summary>
    public class AppiumTestObject : BaseTestObject, IAppiumTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">The test's Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(AppiumDriver appiumDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(AppiumDriverManager).FullName, new AppiumDriverManager(() => appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">Function for initializing a Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(Func<AppiumDriver> appiumDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(AppiumDriverManager).FullName, new AppiumDriverManager(appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <inheritdoc /> 
        public AppiumDriver AppiumDriver
        {
            get
            {
                return this.AppiumManager.GetAppiumDriver();
            }
        }

        /// <inheritdoc /> 
        public AppiumDriverManager AppiumManager
        {
            get
            {
                return this.ManagerStore.GetManager<AppiumDriverManager>();
            }
        }

        /// <inheritdoc /> 
        public void OverrideAppiumDriver(AppiumDriver appiumDriver)
        {
            this.AppiumManager.OverrideDriver(appiumDriver);
        }

        /// <inheritdoc /> 
        public void OverrideAppiumDriver(Func<AppiumDriver> appiumDriver)
        {
            this.AppiumManager.OverrideDriver(appiumDriver);
        }
    }
}
